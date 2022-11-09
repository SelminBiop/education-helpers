using WhatIsThis.ViewModels;

namespace WhatIsThis.Views;

public partial class JeuPage : ContentPage
{
	public JeuPage(JeuPageViewModel jeuPageViewModel)
	{
		InitializeComponent();
        BindingContext = jeuPageViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(BindingContext is JeuPageViewModel jeuPageViewModel)
        {
            jeuPageViewModel.SetupForNewGame();
        }
    }
}