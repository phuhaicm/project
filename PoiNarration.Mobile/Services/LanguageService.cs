namespace PoiNarration.Mobile.Services;

public static class LanguageService
{
    private const string Key = "lang";
    public static string Current => Preferences.Get(Key, "vi");

    public static void Set(string lang)
    {
        Preferences.Set(Key, lang);
    }

    public static bool IsVi => Current == "vi";
}