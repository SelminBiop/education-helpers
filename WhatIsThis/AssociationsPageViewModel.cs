using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using WhatIsThis.Services;
using WhatIsThis.Data;

namespace WhatIsThis.ViewModels;

public sealed partial class AssociationsPageViewModel : ObservableObject
{
    private const string AssociationsKey = "AssociationsKey";

    private readonly IAssociationStorageService _storageService;

    [ObservableProperty]
    private IList<AssociationsGroup> _associations = new List<AssociationsGroup>();

    public AssociationsPageViewModel(IAssociationStorageService storageService)
    {
        _storageService = storageService;

        UpdateAssociations();   
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
                association.Word,
                association.CorrespondingResource,
                async () =>
                {
                    await OnAssociationSelected(association);
                })).ToList();
                return new AssociationsGroup(category, associationItems);
            }).ToList();
    }
    
    private async Task OnAssociationSelected(Association association) 
    {
        if(Application.Current?.MainPage is Page mainPage)
        {
            string choice = await mainPage.DisplayActionSheet($"Que voulez-vous faire avec {association.Word}?", "Annuler", "Effacer", "Modifier");
            if (choice == "Effacer") {
                _storageService.Remove(AssociationsKey, association);
                MainThread.BeginInvokeOnMainThread(UpdateAssociations);
            } else if (choice == "Modifier") {
                var navigationParameters = new Dictionary<string, object>
                {
                    { "AssociationToModify", association.Word }
                };
                await Shell.Current.GoToAsync("CreateAssociationPage", navigationParameters);
            }
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

    public sealed partial class AssociationItem : ObservableObject
    {
        [ObservableProperty]
        private string _word;

        [ObservableProperty]
        private ImageSource? _resource;

        [ObservableProperty]
        private ICommand? _onAssociationTappedCommand;

        public AssociationItem(
            string word,
            string resourcePath,
            Action onAssociationSelectedAction)
        {
            _word = word;
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