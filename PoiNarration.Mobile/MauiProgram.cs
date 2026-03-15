using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiMaps(); // thêm dòng này

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db3");
        builder.Services.AddSingleton(new AppDatabase(dbPath));
        builder.Services.AddSingleton<LocationService>();

        return builder.Build();
    }
}
