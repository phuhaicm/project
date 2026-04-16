using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile;

public partial class App : Application
{
    private readonly SyncService? _syncService;

    public App(SyncService? syncService = null)
    {
        InitializeComponent();
        _syncService = syncService;

        LanguageService.Initialize();
        EnsureVisitorIdentity();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                EnsureVisitorIdentity();

                var api = new PoiNarration.Mobile.Services.ApiService();
                var serverVisitorId = Preferences.Get("visitor_id_server", "");

                if (string.IsNullOrWhiteSpace(serverVisitorId))
                {
                    var response = await api.RegisterVisitorAsync(new PoiNarration.Mobile.Services.VisitorRegisterRequest
                    {
                        DeviceKey = Preferences.Get("device_key", ""),
                        PreferredLanguage = LanguageService.CurrentLanguage,
                        Platform = DeviceInfo.Platform.ToString(),
                        AppVersion = AppInfo.VersionString
                    });

                    if (response != null)
                    {
                        Preferences.Set("visitor_id_server", response.VisitorId);
                        Preferences.Set("visitor_code", response.VisitorCode);
                    }
                }

                if (_syncService != null)
                {
                    await _syncService.SyncBootstrapAsync();
                    await _syncService.SyncBoothVisitLogsAsync();
                    await _syncService.SyncPlaybackLogsAsync();
                }
            }
            catch
            {
                // fallback offline
            }
        });

        return window;
    }

    private void EnsureVisitorIdentity()
    {
        var deviceKey = Preferences.Get("device_key", "");
        if (string.IsNullOrWhiteSpace(deviceKey))
        {
            Preferences.Set("device_key", $"device-{Guid.NewGuid():N}");
        }

        var visitorCode = Preferences.Get("visitor_code", "");
        if (string.IsNullOrWhiteSpace(visitorCode))
        {
            Preferences.Set("visitor_code", $"VIS-{Guid.NewGuid():N}".Substring(0, 10).ToUpper());
        }

        var sessionId = Preferences.Get("session_id", "");
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            Preferences.Set("session_id", Guid.NewGuid().ToString());
        }
    }
}