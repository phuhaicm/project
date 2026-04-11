using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile;

public partial class App : Application
{
    private readonly SyncService? _syncService;

    public App(SyncService? syncService = null)
    {
        InitializeComponent();
        _syncService = syncService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                if (_syncService != null)
                {
                    await _syncService.SyncBootstrapAsync();
                }
            }
            catch
            {
                // fallback offline
            }
        });

        return window;
    }
}