using WhatIsThis.ViewModels;

namespace WhatIsThis.Views;

public partial class AssociationsPage : ContentPage
{
	public AssociationsPage(AssociationsPageViewModel associationsPageViewModel)
	{
		InitializeComponent();
        BindingContext = associationsPageViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(BindingContext is AssociationsPageViewModel associationsPageViewModel)
        {
            associationsPageViewModel.UpdateAssociations();
        }
    }
}