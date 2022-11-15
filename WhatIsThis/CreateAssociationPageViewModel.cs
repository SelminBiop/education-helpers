using System.Text.Json;
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
        set => SetProperty(ref _word, value);
    }

    private ImageSource _imageSource;
    public ImageSource ImageSource
    {
        get => _imageSource;
        set => SetProperty(ref _imageSource, value);
    }

    public ICommand OnImageChosenCommand { get; set; }
    public ICommand OnSaveCommand { get; set; }
    public ICommand OnWordEnteredCommand { get; set; }

    public CreateAssociationPageViewModel(IAssociationStorageService storageService)
    {
        OnImageChosenCommand = new Command(async () =>
        {
            _fileResult = await MediaPicker.PickPhotoAsync();
            ImageSource = ImageSource.FromFile(_fileResult?.FullPath);
        });

        OnSaveCommand = new Command(() =>
        {
            storageService.Add(AssociationsKey, new Association(Word, _fileResult.FullPath));

            Word = string.Empty;
            _fileResult = null;
            ImageSource = null;
        });
    }
}