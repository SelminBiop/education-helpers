using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.Services;

namespace WhatIsThis.ViewModels;

public sealed class CreateAssociationPageViewModel : ObservableObject
{
    private const string AssociationsKey = "AssociationsKey";

    private FileResult _fileResult;

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
            storageService.Add(AssociationsKey, new Association(Word, _fileResult.FullPath));

            Word = string.Empty;
            ShowImage = false;
            _fileResult = null;
            ImageSource = null;
        }, canExecute: () => !string.IsNullOrEmpty(Word) && ImageSource != null);
    }
}