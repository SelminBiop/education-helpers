using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.Services;

namespace WhatIsThis.ViewModels;

public sealed class MainPageViewModel : ObservableObject
{
    private const string AssociationsKey = "AssociationsKey";

    private FileResult _fileResult;
    private string _word;
    public string Word
    {
        get => _word;
        set => SetProperty(ref _word, value);
    }

    private string _fileName;
    public string FileName
    {
        get => _fileName;
        set => SetProperty(ref _fileName, value);
    }

    public ICommand OnImageChosenCommand { get; set; }
    public ICommand OnSaveCommand { get; set; }
    public ICommand OnWordEnteredCommand { get; set; }

    public MainPageViewModel(IAssociationStorageService storageService)
    {
        OnImageChosenCommand = new Command(async () =>
        {
            _fileResult = await MediaPicker.PickPhotoAsync();
            FileName = _fileResult?.FileName;
        });

        OnSaveCommand = new Command(() =>
        {
            storageService.Add(AssociationsKey, new Association(Word, _fileResult.FullPath));

            Word = string.Empty;
            _fileResult = null;
            FileName = string.Empty;
        });
    }
}