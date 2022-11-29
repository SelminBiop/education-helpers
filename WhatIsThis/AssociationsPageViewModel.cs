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

    private IList<AssociationsGroup> _associations = new List<AssociationsGroup>();
    public IList<AssociationsGroup> Associations
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

        Associations = storedAssociations.GroupBy(
            association => association.Category,
            association => association,
            (category, associations) =>
            {
                var associationItems = associations.Select(association => new AssociationItem(
                association.word,
                association.correspondingResource,
                async () =>
                {
                    await OnAssociationSelected(association);
                })).ToList();
                return new AssociationsGroup(category, associationItems);
            }).ToList();
    }
    
    private async Task OnAssociationSelected(Association association) 
    {
        string choice = await Application.Current.MainPage.DisplayActionSheet($"Que voulez-vous faire avec {association.word}?", "Annuler", "Effacer", "Modifier");
        if (choice == "Effacer") {
            _storageService.Remove(AssociationsKey, association);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateAssociations();
            });
        }
        else if(choice == "Modifier")
        {
            var navigationParameters = new Dictionary<string, object>
            {
                { "AssociationToModify", association.word }
            };
            await Shell.Current.GoToAsync("CreateAssociationPage", navigationParameters);
        }
    }

    public sealed class AssociationsGroup : List<AssociationItem>
    {
        private const string NoCategoryName = "Aucune categorie";

        public string Name { get; private set; } = NoCategoryName;

        public AssociationsGroup(string name, List<AssociationItem> associations) : base(associations)
        {
            Name = string.IsNullOrEmpty(name) ? NoCategoryName : name;
        }
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

        public AssociationItem(
            string word,
            string resourcePath,
            Action onAssociationSelectedAction)
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