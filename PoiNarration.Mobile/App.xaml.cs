using PoiNarration.Mobile.Services;
namespace PoiNarration.Mobile;

public partial class App : Application
{
    public App(AppDatabase db)
    {
        // Seed async (fire-and-forget an toàn)
        Task.Run(async () =>
        {
            var seedService = new SeedService(db);
            await seedService.EnsureSeededAsync();
        });
    }

    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var services = Current?.Handler?.MauiContext?.Services;
                var syncService = services?.GetService<PoiNarration.Mobile.Services.SyncService>();

                if (syncService != null)
                {
                    await syncService.SyncBootstrapAsync();
                }
            }
            catch
            {
                // fallback offline
            }
        });
        return new Window(new AppShell());
    }
    protected override async void OnStart()
    {
        base.OnStart();

        var services = Current?.Handler?.MauiContext?.Services;
        if (services == null) return;

        var syncService = services.GetRequiredService<PoiNarration.Mobile.Services.SyncService>();
        await syncService.SyncBootstrapAsync();
    }


}