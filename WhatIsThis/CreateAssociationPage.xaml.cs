using WhatIsThis.ViewModels;

namespace WhatIsThis.Views;

public partial class CreateAssociationPage : ContentPage
{
	public CreateAssociationPage(CreateAssociationPageViewModel createAssociationPageViewModel)
	{
		InitializeComponent();
        BindingContext = createAssociationPageViewModel;
	}
}

