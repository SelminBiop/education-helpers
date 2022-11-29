using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.Services;

namespace WhatIsThis.ViewModels;

[QueryProperty(nameof(AssociationToModify), "AssociationToModify")]
public sealed class CreateAssociationPageViewModel : ObservableObject
{
    private const string AssociationsKey = "AssociationsKey";

    private readonly IAssociationStorageService _associationStorageService;

    private FileResult _fileResult;

    private Association _associationToRemove;

    private string _associationToModify;
    public string AssociationToModify {
        get => _associationToModify;
        set 
        {
            _associationToModify = value;
            _associationToRemove = _associationStorageService
                .Get(AssociationsKey)
                .Where(association => association.word == _associationToModify)
                .FirstOrDefault();
            Word = _associationToRemove.word;
            Category = _associationToRemove.Category;
            ImageSource = ImageSource.FromFile(_associationToRemove.correspondingResource);
        }        
    }

    private string _word;
    public string Word
    {
        get => _word;
        set 
        {
            if(SetProperty(ref _word, value))
            {
                ((Command)OnSaveCommand).ChangeCanExecute();
            }
        }
    }

    private string _category;
    public string Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }


    private bool _showImage;
    public bool ShowImage
    {
        get => _showImage;
        set => SetProperty(ref _showImage, value);
    }

    private ImageSource _imageSource;
    public ImageSource ImageSource
    {
        get => _imageSource;
        set 
        {
            if(SetProperty(ref _imageSource, value))
            {
                ((Command)OnSaveCommand).ChangeCanExecute();
            }
        }
    }

    public ICommand OnImageChosenCommand { get; set; }
    public ICommand OnSaveCommand { get; set; }

    public CreateAssociationPageViewModel(IAssociationStorageService storageService)
    {
        _associationStorageService = storageService;

        OnImageChosenCommand = new Command(async () =>
        {
            _fileResult = await MediaPicker.PickPhotoAsync();
            ShowImage = !string.IsNullOrEmpty(_fileResult?.FullPath);
            if(ShowImage)
            {
                ImageSource = ImageSource.FromFile(_fileResult?.FullPath);
            }
        });

        OnSaveCommand = new Command(() =>
        {
            if(_associationToRemove is not null) {
                _associationStorageService.Remove(AssociationsKey, _associationToRemove);
            }

            _associationStorageService.Add(AssociationsKey, new Association(Word, _fileResult?.FullPath ?? _associationToRemove.correspondingResource, Category));

            Word = string.Empty;
            ShowImage = false;
            _fileResult = null;
            ImageSource = null;
        }, canExecute: () => !string.IsNullOrEmpty(Word) && ImageSource != null);
    }
}