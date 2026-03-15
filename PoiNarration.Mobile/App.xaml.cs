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
        return new Window(new AppShell());
    }

}