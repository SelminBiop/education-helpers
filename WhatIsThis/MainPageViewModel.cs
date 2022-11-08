using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WhatIsThis.ViewModels;

public sealed class MainPageViewModel : ObservableObject
{
    private const string AssociationsKey = "AssociationsKey";
    private IList<Association> _associations = new List<Association>();

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

    public MainPageViewModel()
    {
        OnImageChosenCommand = new Command(async () => {
            _fileResult = await MediaPicker.PickPhotoAsync();
            FileName = _fileResult?.FileName;
            });
        OnSaveCommand = new Command(() => {
            _associations.Add(new Association(Word, _fileResult.FullPath));
            var associationsJson = JsonSerializer.Serialize<IList<Association>>(_associations);
            Preferences.Set(AssociationsKey, associationsJson);

            Word = string.Empty;
            _fileResult = null;
            FileName = string.Empty;
            });

        var associationsJson = Preferences.Get(AssociationsKey, string.Empty);
        if(associationsJson != string.Empty){
            _associations = JsonSerializer.Deserialize<List<Association>>(associationsJson);
        }
    }
}