using Microsoft.Maui.Storage;

namespace PoiNarration.Mobile.Services;

public static class LanguageService
{
    private const string Key = "app_language";

    public static readonly List<string> SupportedLanguages = new()
    {
        "vi", "en", "zh", "ja", "ko", "fr", "es", "it", "ru"
    };

    public static string CurrentLanguage
    {
        get => Preferences.Get(Key, "vi");
        set => Preferences.Set(Key, value);
    }

    public static bool IsVi => CurrentLanguage == "vi";

    public static void Set(string languageCode)
    {
        CurrentLanguage = languageCode;
    }

    public static string Get()
    {
        return CurrentLanguage;
    }

    public static string GetDefaultCurrencyCode(string languageCode)
    {
        return languageCode switch
        {
            "vi" => "VND",
            "en" => "USD",
            "zh" => "CNY",
            "ja" => "JPY",
            "ko" => "KRW",
            "fr" or "es" or "it" => "EUR",
            "ru" => "RUB",
            _ => "USD"
        };
    }

    public static string GetTtsLocalePrefix(string languageCode)
    {
        return languageCode switch
        {
            "vi" => "vi",
            "en" => "en",
            "zh" => "zh",
            "ja" => "ja",
            "ko" => "ko",
            "fr" => "fr",
            "es" => "es",
            "it" => "it",
            "ru" => "ru",
            _ => "en"
        };
    }
}