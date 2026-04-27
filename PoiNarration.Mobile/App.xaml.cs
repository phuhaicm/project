using PoiNarration.Mobile.Services;
using System.Diagnostics;

namespace PoiNarration.Mobile;

public partial class App : Application
{
    private readonly SyncService? _syncService;
    private readonly VisitorSessionService _visitorSessionService;
    private readonly ApiService _apiService;

    private IDispatcherTimer? _heartbeatTimer;
    private string? _currentVisitorId;

    public App(
        SyncService? syncService = null,
        VisitorSessionService? visitorSessionService = null,
        ApiService? apiService = null)
    {
        InitializeComponent();

        _syncService = syncService;
        _visitorSessionService = visitorSessionService ?? new VisitorSessionService();
        _apiService = apiService ?? new ApiService();

        LanguageService.Initialize();

        // Chỉ đảm bảo device_key tồn tại, KHÔNG reset visitor nữa
        _visitorSessionService.EnsureInitialized();

        // Session mới mỗi lần mở app là OK
        _visitorSessionService.RefreshSession();
    }

    private void StartHeartbeat()
    {
        if (string.IsNullOrWhiteSpace(_currentVisitorId))
            return;

        StopHeartbeat();

        _heartbeatTimer = Dispatcher.CreateTimer();
        _heartbeatTimer.Interval = TimeSpan.FromSeconds(60);

        _heartbeatTimer.Tick += async (s, e) =>
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_currentVisitorId))
                {
                    await _apiService.TouchVisitorAsync(_currentVisitorId);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Heartbeat Error]: {ex}");
            }
        };

        _heartbeatTimer.Start();
    }

    private void StopHeartbeat()
    {
        if (_heartbeatTimer != null)
        {
            _heartbeatTimer.Stop();
            _heartbeatTimer = null;
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var serverVisitorId = _visitorSessionService.GetVisitorIdServer();

                // Nếu thiết bị này chưa từng đăng ký trên server
                if (string.IsNullOrWhiteSpace(serverVisitorId))
                {
                    var response = await _apiService.RegisterVisitorAsync(new VisitorRegisterRequest
                    {
                        DeviceKey = _visitorSessionService.GetDeviceKey(),
                        PreferredLanguage = LanguageService.CurrentLanguage,
                        Platform = DeviceInfo.Platform.ToString(),
                        AppVersion = AppInfo.VersionString
                    });

                    if (response != null)
                    {
                        _visitorSessionService.SaveRegisteredVisitor(
                            response.VisitorId,
                            response.VisitorCode,
                            response.DisplayName);

                        serverVisitorId = response.VisitorId;
                    }
                }

                // Sau khi đã có visitor server id thì mới touch + start heartbeat
                _currentVisitorId = serverVisitorId;

                if (!string.IsNullOrWhiteSpace(_currentVisitorId))
                {
                    await _apiService.TouchVisitorAsync(_currentVisitorId);
                    StartHeartbeat();
                }

                if (_syncService != null)
                {
                    await _syncService.SyncBootstrapAsync();
                    await _syncService.SyncBoothVisitLogsAsync();
                    await _syncService.SyncPlaybackLogsAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[App Startup Error]: {ex}");
                // fallback offline
            }
        });

        window.Stopped += (s, e) =>
        {
            StopHeartbeat();
        };

        window.Resumed += async (s, e) =>
        {
            try
            {
                _currentVisitorId = _visitorSessionService.GetVisitorIdServer();

                if (!string.IsNullOrWhiteSpace(_currentVisitorId))
                {
                    await _apiService.TouchVisitorAsync(_currentVisitorId);
                    StartHeartbeat();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Resume Error]: {ex}");
            }
        };

        return window;
    }
}