using Microsoft.Extensions.Localization;
using WhatIsThis.Resources.Strings;
using WhatIsThis.Services;

namespace WhatIsThis.MarkupExtensions;

[ContentProperty(nameof(Key))]
public sealed class LocalizeExtension : IMarkupExtension
{
    IStringLocalizer<AppResources>? _localizer;

    public string Key { get; set; } = string.Empty;

    public LocalizeExtension() 
    {
        _localizer = ServiceHelper.GetService<IStringLocalizer<AppResources>>();
    }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        string localizedText = _localizer?.GetString(Key) ?? string.Empty;
        
        return localizedText;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
