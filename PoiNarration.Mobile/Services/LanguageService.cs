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
        // Tự động cập nhật giao diện ngay khi set ngôn ngữ mới
        UpdateUiResources(languageCode);
    }

    public static string Get()
    {
        return CurrentLanguage;
    }

    // --- HÀM MỚI: ĐỔ CHỮ VÀO RESOURCE ---
    public static void UpdateUiResources(string lang)
    {
        var res = Application.Current.Resources;
        bool isVi = (lang == "vi");

        // Gán giá trị cho cái tên bị báo lỗi
        res["Ui_PageTitle"] = isVi ? "Danh sách Trạm" : "Booths";

        // Gán giá trị cho các nút bấm khác
        res["Ui_SyncReady"] = isVi ? "Sẵn sàng đồng bộ dữ liệu" : "Ready to sync";
        res["Ui_Sync"] = isVi ? "Đồng bộ" : "Sync";
        res["Ui_ScanQR"] = isVi ? "Quét QR" : "Scan QR";
        res["Ui_GpsMode"] = "GPS Mode";
        res["Ui_Map"] = isVi ? "Bản đồ" : "Map";
        res["Ui_TotalBooth"] = isVi ? "Tổng Booth" : "Total Booths";
        res["Ui_CurrentLang"] = isVi ? "Ngôn ngữ hiện tại" : "Current Language";
        res["Ui_SearchPlaceholder"] = isVi ? "Tìm booth..." : "Search booth...";
        res["Ui_LangTitle"] = isVi ? "Ngôn ngữ" : "Language";
        res["Ui_Back"] = isVi ? "← Quay lại" : "← Back";
        res["Ui_PlayAudio"] = isVi ? "▶ Phát âm thanh" : "▶ Play Audio";
        res["Ui_StopAudio"] = isVi ? "■ Dừng" : "■ Stop";
        res["Ui_MenuHeader"] = isVi ? "Menu / Sản phẩm" : "Menu / Products";
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