using WhatIsThis.ViewModels;

namespace WhatIsThis.Views;

public partial class AssociationsPage : ContentPage
{
	public AssociationsPage(AssociationsPageViewModel associationsPageViewModel)
	{
		InitializeComponent();
        BindingContext = associationsPageViewModel;
	}
}