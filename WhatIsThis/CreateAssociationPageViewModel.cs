using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.Services;
using WhatIsThis.Data;

namespace WhatIsThis.ViewModels;

[QueryProperty(nameof(AssociationToModify), "AssociationToModify")]
public sealed partial class CreateAssociationPageViewModel : ObservableObject
{
    private const string AssociationsKey = "AssociationsKey";

    private readonly IAssociationStorageService _associationStorageService;

    private FileResult? _fileResult;
    private Association? _associationToRemove;

    private string? _associationToModify;
    public string? AssociationToModify {
        get => _associationToModify;
        set 
        {
            _associationToModify = value;
            _associationToRemove = _associationStorageService
                .Get(AssociationsKey)
                .Where(association => association.Word == _associationToModify)
                .FirstOrDefault();

            if(_associationToRemove != null ) 
            {
                Word = _associationToRemove.Word;
                Category = _associationToRemove.Category;
                ImageSource = ImageSource.FromFile(_associationToRemove.CorrespondingResource);
            }
        }        
    }

    private string? _word;
    public string? Word
    {
        get => _word;
        set 
        {
            if(SetProperty(ref _word, value))
            {
                SaveAssociationCommand.ChangeCanExecute();
            }
        }
    }

    private ImageSource? _imageSource;
    public ImageSource? ImageSource
    {
        get => _imageSource;
        set 
        {
            if(SetProperty(ref _imageSource, value))
            {
                SaveAssociationCommand.ChangeCanExecute();
            }
        }
    }

    [ObservableProperty]
    private string? _category;

    [ObservableProperty]
    private bool _showImage;

    [ObservableProperty]
    private ICommand _imageChosenCommand;

    [ObservableProperty]
    private Command _saveAssociationCommand;

    public CreateAssociationPageViewModel(IAssociationStorageService storageService)
    {
        _associationStorageService = storageService;

        _imageChosenCommand = new Command(async () =>
        {
            _fileResult = await MediaPicker.PickPhotoAsync();
            ShowImage = !string.IsNullOrEmpty(_fileResult?.FullPath);
            if(ShowImage)
            {
                ImageSource = ImageSource.FromFile(_fileResult?.FullPath);
            }
        });

        _saveAssociationCommand = new Command(() =>
        {
            if(_associationToRemove is not null) {
                _associationStorageService.Remove(AssociationsKey, _associationToRemove);
            }

            var correspondingResource = _fileResult?.FullPath ?? _associationToRemove?.CorrespondingResource;

            if (!string.IsNullOrEmpty(Word) && !string.IsNullOrEmpty(correspondingResource)) 
            {
                _associationStorageService.Add(AssociationsKey, 
                    string.IsNullOrEmpty(Category) ? 
                    new Association(Word, correspondingResource) : 
                    new Association(Word, correspondingResource, Category));
            }            

            Word = string.Empty;
            Category = string.Empty;
            ShowImage = false;
            _fileResult = null;
            ImageSource = null;
        }, canExecute: () => !string.IsNullOrEmpty(Word) && ImageSource != null);
    }
}