using WhatIsThis.Services;
using WhatIsThis.ViewModels;
using WhatIsThis.Views;
using WhatIsThis.QuestionType;

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
