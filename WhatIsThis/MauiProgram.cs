using WhatIsThis.Services;
using WhatIsThis.ViewModels;
using WhatIsThis.Views;
using WhatIsThis.QuestionType;
using CommunityToolkit.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

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

#if __ANDROID__
		//Workaround for the recycled bitmap crash
        ImageHandler.Mapper.PrependToMapping(nameof(Microsoft.Maui.IImage.Source), (handler, view) => PrependToMappingImageSource(handler, view));
#endif

        return builder.Build();
	}

#if __ANDROID__
    public static void PrependToMappingImageSource(IImageHandler handler, Microsoft.Maui.IImage image)
    {
        handler.PlatformView?.Clear();
    }
#endif
}
