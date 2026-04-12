using Microsoft.Maui.Storage;

namespace PoiNarration.Mobile.Services;

public static class LanguageService
{
    private const string Key = "app_language";

    public static readonly List<string> SupportedLanguages = new()
    {
        "vi", "en", "zh", "ja", "ko", "fr", "es", "it", "ru"
    };

    public static event Action? LanguageChanged;

    public static string CurrentLanguage
    {
        get => Preferences.Get(Key, "vi");
        set => Preferences.Set(Key, value);
    }

    public static bool IsVi => CurrentLanguage == "vi";

    private static readonly Dictionary<string, Dictionary<string, string>> UiTexts = new()
    {
        ["vi"] = new()
        {
            ["Ui_AppTitle"] = "PoiNarration",
            ["Ui_Tab_Booths"] = "Booths",
            ["Ui_Tab_Map"] = "Bản đồ",
            ["Ui_Tab_Qr"] = "QR",
            ["Ui_Page_BoothList"] = "Danh sách trạm",
            ["Ui_Page_BoothDetail"] = "Chi tiết trạm",
            ["Ui_Page_Map"] = "Bản đồ",
            ["Ui_Page_Qr"] = "Quét QR gian hàng",
            ["Ui_Page_GateMode"] = "Chọn chế độ",
            ["Ui_Page_ZoneList"] = "Chọn khu vực",
            ["Ui_Page_BoothByZone"] = "Danh sách gian hàng",

            ["Ui_SyncReady"] = "Sẵn sàng đồng bộ dữ liệu",
            ["Ui_SyncSuccess"] = "Đồng bộ thành công",
            ["Ui_SyncFailed"] = "Đồng bộ thất bại",
            ["Ui_Syncing"] = "Đang đồng bộ...",
            ["Ui_Sync"] = "Đồng bộ",
            ["Ui_ScanQR"] = "Quét QR",
            ["Ui_GpsMode"] = "GPS Mode",
            ["Ui_Map"] = "Bản đồ",
            ["Ui_PlayAudio"] = "▶ Phát âm thanh",
            ["Ui_StopAudio"] = "■ Dừng",
            ["Ui_TotalBooth"] = "Tổng số booth",
            ["Ui_CurrentLang"] = "Ngôn ngữ hiện tại",
            ["Ui_Back"] = "← Quay lại",
            ["Ui_MenuHeader"] = "Menu / Sản phẩm",
            ["Ui_SearchPlaceholder"] = "Tìm booth...",
            ["Ui_LangTitle"] = "Ngôn ngữ",
            ["Ui_Detail"] = "Chi tiết",
            ["Ui_Preview"] = "Nghe thử",
            ["Ui_OpenDetail"] = "Mở chi tiết",
            ["Ui_Refresh"] = "Làm mới",

            ["Ui_LocationChecking"] = "Vị trí: Đang xác định...",
            ["Ui_GpsChecking"] = "GPS: Đang kiểm tra...",
            ["Ui_GpsActive"] = "GPS: Đang hoạt động",
            ["Ui_GpsAutoOn"] = "GPS: ĐANG BẬT AUTO",
            ["Ui_GpsTrackingOnly"] = "GPS: CHỈ THEO DÕI",
            ["Ui_NearestBoothHeader"] = "BOOTH GẦN NHẤT",
            ["Ui_NearestUnknown"] = "Chưa xác định",
            ["Ui_NearestNotFound"] = "Chưa tìm thấy booth gần nhất",
            ["Ui_CurrentLocation"] = "Vị trí hiện tại",
            ["Ui_BoothCount"] = "Số booth",
            ["Ui_Distance"] = "Khoảng cách",
            ["Ui_EnteredZone"] = "Đã vào vùng",

            ["Ui_QrInstruction"] = "Đưa mã QR của gian hàng vào khung hình",
            ["Ui_GateScanned"] = "Bạn vừa quét QR ở cổng",
            ["Ui_ChooseMode"] = "Chọn chế độ",
            ["Ui_ModeGpsAuto"] = "1) GPS Tracking (Tự động)",
            ["Ui_ModeManual"] = "2) Thủ công (Chọn khu vực)",
            ["Ui_ModeBoothQr"] = "Quét QR Booth",
            ["Ui_ChooseZone"] = "Chọn khu vực",
            ["Ui_ZoneA"] = "Khu A",
            ["Ui_ZoneB"] = "Khu B",

            ["Ui_Alert_Error"] = "Lỗi",
            ["Ui_Alert_SyncError"] = "Lỗi sync",
            ["Ui_Alert_GpsMode"] = "GPS Mode",
            ["Ui_Alert_QrError"] = "Lỗi mã QR",
            ["Ui_Alert_NotFoundBooth"] = "Không tìm thấy booth.",
            ["Ui_Alert_QrInvalid"] = "Mã QR này không thuộc hệ thống PoiNarration!",
            ["Ui_Alert_TryAgain"] = "Thử lại",
            ["Ui_Alert_Ok"] = "OK",
            ["Ui_Alert_GpsEnableFail"] = "Không bật được GPS hoặc chưa cấp quyền vị trí.",
            ["Ui_Alert_GpsAutoOnMessage"] = "Đã bật GPS mode. Đứng gần gian hàng khoảng 2 giây là app sẽ tự nhảy vào và thuyết minh.",
            ["Ui_Alert_GpsTrackingOnlyMessage"] = "Đã tắt tự động thuyết minh. Hệ thống vẫn tiếp tục theo dõi vị trí của bạn.",
            ["Ui_Alert_GpsMapAutoOn"] = "Đã bật chế độ tự động thuyết minh khi đến gần gian hàng.",
            ["Ui_Alert_GpsMapTrackingOnly"] = "Đã tắt chế độ tự động. Bản đồ vẫn tiếp tục hiển thị vị trí của bạn."
        },

        ["en"] = new()
        {
            ["Ui_AppTitle"] = "PoiNarration",
            ["Ui_Tab_Booths"] = "Booths",
            ["Ui_Tab_Map"] = "Map",
            ["Ui_Tab_Qr"] = "QR",
            ["Ui_Page_BoothList"] = "Booth List",
            ["Ui_Page_BoothDetail"] = "Booth Detail",
            ["Ui_Page_Map"] = "Map",
            ["Ui_Page_Qr"] = "Scan Booth QR",
            ["Ui_Page_GateMode"] = "Choose Mode",
            ["Ui_Page_ZoneList"] = "Choose Zone",
            ["Ui_Page_BoothByZone"] = "Booths by Zone",

            ["Ui_SyncReady"] = "Ready to sync",
            ["Ui_SyncSuccess"] = "Sync successful",
            ["Ui_SyncFailed"] = "Sync failed",
            ["Ui_Syncing"] = "Syncing...",
            ["Ui_Sync"] = "Sync",
            ["Ui_ScanQR"] = "Scan QR",
            ["Ui_GpsMode"] = "GPS Mode",
            ["Ui_Map"] = "Map",
            ["Ui_PlayAudio"] = "▶ Play Audio",
            ["Ui_StopAudio"] = "■ Stop",
            ["Ui_TotalBooth"] = "Total Booths",
            ["Ui_CurrentLang"] = "Current Language",
            ["Ui_Back"] = "← Back",
            ["Ui_MenuHeader"] = "Menu / Products",
            ["Ui_SearchPlaceholder"] = "Search booth...",
            ["Ui_LangTitle"] = "Language",
            ["Ui_Detail"] = "Details",
            ["Ui_Preview"] = "Preview",
            ["Ui_OpenDetail"] = "Open Detail",
            ["Ui_Refresh"] = "Refresh",

            ["Ui_LocationChecking"] = "Location: Detecting...",
            ["Ui_GpsChecking"] = "GPS: Checking...",
            ["Ui_GpsActive"] = "GPS: Active",
            ["Ui_GpsAutoOn"] = "GPS: AUTO ON",
            ["Ui_GpsTrackingOnly"] = "GPS: TRACKING ONLY",
            ["Ui_NearestBoothHeader"] = "NEAREST BOOTH",
            ["Ui_NearestUnknown"] = "Unknown",
            ["Ui_NearestNotFound"] = "No nearby booth found",
            ["Ui_CurrentLocation"] = "Current location",
            ["Ui_BoothCount"] = "Booths",
            ["Ui_Distance"] = "Distance",
            ["Ui_EnteredZone"] = "Entered zone",

            ["Ui_QrInstruction"] = "Place the booth QR code inside the frame",
            ["Ui_GateScanned"] = "You scanned the gate QR",
            ["Ui_ChooseMode"] = "Choose mode",
            ["Ui_ModeGpsAuto"] = "1) GPS Tracking (Auto)",
            ["Ui_ModeManual"] = "2) Manual (Choose zone)",
            ["Ui_ModeBoothQr"] = "Scan Booth QR",
            ["Ui_ChooseZone"] = "Choose zone",
            ["Ui_ZoneA"] = "Zone A",
            ["Ui_ZoneB"] = "Zone B",

            ["Ui_Alert_Error"] = "Error",
            ["Ui_Alert_SyncError"] = "Sync Error",
            ["Ui_Alert_GpsMode"] = "GPS Mode",
            ["Ui_Alert_QrError"] = "QR Error",
            ["Ui_Alert_NotFoundBooth"] = "Booth not found.",
            ["Ui_Alert_QrInvalid"] = "This QR code does not belong to PoiNarration!",
            ["Ui_Alert_TryAgain"] = "Try Again",
            ["Ui_Alert_Ok"] = "OK",
            ["Ui_Alert_GpsEnableFail"] = "Unable to enable GPS or location permission is missing.",
            ["Ui_Alert_GpsAutoOnMessage"] = "GPS mode is on. Stay near a booth for about 2 seconds and the app will auto-open and narrate.",
            ["Ui_Alert_GpsTrackingOnlyMessage"] = "Auto narration is off. The system still keeps tracking your location.",
            ["Ui_Alert_GpsMapAutoOn"] = "Auto narration has been enabled when approaching a booth.",
            ["Ui_Alert_GpsMapTrackingOnly"] = "Auto mode has been disabled. The map still shows your location."
        }
    };

    public static void Initialize()
    {
        UpdateUiResources(CurrentLanguage);
    }

    public static void Set(string languageCode)
    {
        if (!SupportedLanguages.Contains(languageCode))
            languageCode = "en";

        CurrentLanguage = languageCode;
        UpdateUiResources(languageCode);
        LanguageChanged?.Invoke();
    }

    public static string Get() => CurrentLanguage;

    public static string T(string key)
    {
        var lang = CurrentLanguage;

        if (UiTexts.TryGetValue(lang, out var dict) && dict.TryGetValue(key, out var value))
            return value;

        if (UiTexts.TryGetValue("en", out var enDict) && enDict.TryGetValue(key, out var enValue))
            return enValue;

        return key;
    }

    public static void UpdateUiResources(string lang)
    {
        if (Application.Current?.Resources == null)
            return;

        var keys = UiTexts["en"].Keys.ToList();

        foreach (var key in keys)
        {
            Application.Current.Resources[key] = T(key);
        }
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
