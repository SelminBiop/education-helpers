using WhatIsThis.ViewModels;

namespace WhatIsThis.Views;

public partial class JeuPage : ContentPage
{
	public JeuPage(JeuPageViewModel jeuPageViewModel)
	{
		InitializeComponent();
        BindingContext = jeuPageViewModel;
	}
}