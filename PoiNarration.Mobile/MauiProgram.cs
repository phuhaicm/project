using Microsoft.Extensions.Logging;
using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiMaps();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db3");

        // Database
        builder.Services.AddSingleton(new AppDatabase(dbPath));

        // Services
        builder.Services.AddSingleton<LocationService>();
        builder.Services.AddSingleton<NarrationService>();
        builder.Services.AddSingleton<GeofenceService>();

        return builder.Build();
    }
}