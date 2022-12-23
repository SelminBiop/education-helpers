using WhatIsThis.Services;
using WhatIsThis.ViewModels;
using WhatIsThis.Views;
using WhatIsThis.QuestionType;
using CommunityToolkit.Maui;

namespace WhatIsThis;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.Services
			.AddLocalization()
            .AddSingleton<AppShell>()
            .AddSingleton<AppShellViewModel>()
            .AddTransient<MainPage>()
			.AddTransient<MainPageViewModel>()
			.AddTransient<AssociationsPage>()
			.AddTransient<AssociationsPageViewModel>()
			.AddTransient<JeuPage>()
			.AddTransient<JeuPageViewModel>()
			.AddTransient<CreateAssociationPage>()
			.AddTransient<CreateAssociationPageViewModel>()
            .AddTransient<ChooseCategoryPage>()
            .AddTransient<ChooseCategoryPageViewModel>()
            .AddTransient<IQuestionType, AssociationQuestion>()
            .AddTransient<IAssociationStorageService, DeviceStorageService>();

        Routing.RegisterRoute("CreateAssociationPage", typeof(CreateAssociationPage));
        Routing.RegisterRoute("JeuPage", typeof(JeuPage));

        return builder.Build();
	}
}
