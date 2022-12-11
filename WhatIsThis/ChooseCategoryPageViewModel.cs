﻿using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WhatIsThis.Services;

namespace WhatIsThis.ViewModels
{
    public sealed partial class ChooseCategoryPageViewModel : ObservableObject
    {
        private const string AssociationsKey = "AssociationsKey";

        private readonly IAssociationStorageService _associationStorageService;

        private ObservableCollection<string> _selectedCategories = new();

        [ObservableProperty]
        private IList<CategoryItem> _categories = new List<CategoryItem>();

        [ObservableProperty]
        private ICommand _onSelectionChangedCommand;

        [ObservableProperty]
        private ICommand _startGameCommand;

        public ChooseCategoryPageViewModel(
            IAssociationStorageService associationStorageService) 
        {
            _associationStorageService = associationStorageService;

            var storedAssociations = _associationStorageService.Get(AssociationsKey);

            Categories = storedAssociations.GroupBy(
                association => association.Category,
                association => association,
                (category, associations) => {
                    if(associations.Count() >= JeuPageViewModel.NumberOfPossibleAnswer) {
                        return new CategoryItem
                        {
                            Name = category
                        };
                    }
                    return null;
                }).Where(category => category is not null).ToList();

            OnSelectionChangedCommand = new Command(
                categories => {
                    _selectedCategories = (categories as IList<object>).Cast<CategoryItem>().Select(category => category.Name).ToObservableCollection();
                    ((Command)StartGameCommand).ChangeCanExecute();
                });

            StartGameCommand = new Command(async _ => await OnCategoriesSelectedAsync(_selectedCategories), _ => _selectedCategories.Count() > 0);
        }

        private async Task OnCategoriesSelectedAsync(ObservableCollection<string> categories) 
        {
            var navigationParameters = new Dictionary<string, object>
            {
                { "Categories", categories }
            };
            await Shell.Current.GoToAsync("JeuPage", navigationParameters);
        }

        public sealed class CategoryItem : ObservableObject
        {
            public string? Name { get; set; }

            public string FriendlyName => string.IsNullOrEmpty(Name) ? "Aucune Categorie" : Name;
        }
    }
}
