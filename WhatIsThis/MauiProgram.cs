using WhatIsThis.ViewModels;
using WhatIsThis.Views;

namespace WhatIsThis;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.Services
			.AddTransient<MainPage>()
			.AddTransient<MainPageViewModel>()
			.AddTransient<AssociationsPage>()
			.AddTransient<AssociationsPageViewModel>()
			.AddTransient<JeuPage>()
			.AddTransient<JeuPageViewModel>();

		return builder.Build();
	}
}
