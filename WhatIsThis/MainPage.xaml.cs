﻿using WhatIsThis.ViewModels;

namespace WhatIsThis.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
        BindingContext = mainPageViewModel;
	}
}

