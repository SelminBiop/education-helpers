using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using WhatIsThis.Services;

namespace WhatIsThis.ViewModels
{
    public sealed class ChooseCategoryPageViewModel : ObservableObject
    {
        private const string AssociationsKey = "AssociationsKey";

        private readonly IAssociationStorageService _associationStorageService;

        private IList<CategoryItem> _categories = new List<CategoryItem>();
        public IList<CategoryItem> Categories {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public ChooseCategoryPageViewModel(
            IAssociationStorageService associationStorageService) 
        {
            _associationStorageService = associationStorageService;

            var storedAssociations = _associationStorageService.Get(AssociationsKey);

            Categories = storedAssociations.GroupBy(
                association => association.category,
                association => association,
                (category, associations) => {
                    return new CategoryItem
                    {
                        Name = category ?? "Aucune categorie",
                        OnCategorySelected = new Command(async async => await OnCategorySelected(category))
                    };
                }).ToList();
        }

        private async Task OnCategorySelected(string category) 
        {
            var navigationParameters = new Dictionary<string, object>
            {
                { "Category", category }
            };
            await Shell.Current.GoToAsync("JeuPage", navigationParameters);
        }

        public sealed class CategoryItem : ObservableObject 
        {
            private string _name;
            public string Name 
            {
                get => _name;
                set => SetProperty(ref _name, value);
            }

            private ICommand _onCategorySelected;
            public ICommand OnCategorySelected {
                get => _onCategorySelected;
                set => SetProperty(ref _onCategorySelected, value);
            }
        }
    }
}
