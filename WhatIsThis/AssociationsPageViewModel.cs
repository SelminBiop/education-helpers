using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.Services;

namespace WhatIsThis.ViewModels;

public sealed class AssociationsPageViewModel : ObservableObject
{
    private const string AssociationsKey = "AssociationsKey";

    private readonly IAssociationStorageService _storageService;

    private List<Association> _storedAssociations = new();  

    private IList<AssociationItem> _associations = new List<AssociationItem>();
    public IList<AssociationItem> Associations
    {
        get => _associations;
        set => SetProperty(ref _associations, value);
    }

    public AssociationsPageViewModel(IAssociationStorageService storageService)
    {
        _storageService = storageService;
    }

    public void UpdateAssociations()
    {
        var storedAssociations = _storageService.Get(AssociationsKey);

        Associations = storedAssociations.Select(association => new AssociationItem(
            association.word,
            association.correspondingResource,
            async () => 
            {
                bool removeAssociation = await Application.Current.MainPage.DisplayAlert($"{association.word}", "Voulez-vous enlever cette association?", "Oui", "Non");
                if(removeAssociation)
                {
                    _storageService.Remove(AssociationsKey, association);
                    MainThread.BeginInvokeOnMainThread(()=>
                    {
                        UpdateAssociations();
                    });
                }
            })).ToList();
    }

    public sealed class AssociationItem : ObservableObject
    {

        private string _word;
        public string Word
        {
            get => _word;
            set => SetProperty(ref _word, value);
        }

        private ImageSource _resource;
        public ImageSource Resource
        {
            get => _resource;
            set => SetProperty(ref _resource, value);
        }

        private ICommand _onAssociationTappedCommand;
        public ICommand OnAssociationTappedCommand
        {
            get => _onAssociationTappedCommand;
            set => SetProperty(ref _onAssociationTappedCommand, value);
        }

        public AssociationItem(string word, string resourcePath, Action onAssociationSelectedAction)
        {
            Word = word;
            SetAction(onAssociationSelectedAction);
            SetResource(resourcePath);
        }

        public void SetAction(Action onAssociationSelectedAction)
        {
            OnAssociationTappedCommand = new Command(_ => onAssociationSelectedAction.Invoke());
        }

        public void SetResource(string resource)
        {
            Resource = ImageSource.FromFile(resource);
        }
    }
}