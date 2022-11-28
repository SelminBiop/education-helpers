using WhatIsThis.ViewModels;

namespace WhatIsThis.Views;

public partial class ChooseCategoryPage : ContentPage
{
	public ChooseCategoryPage(ChooseCategoryPageViewModel chooseCategoryPageViewModel)
	{
		InitializeComponent();
		BindingContext = chooseCategoryPageViewModel;
	}
}