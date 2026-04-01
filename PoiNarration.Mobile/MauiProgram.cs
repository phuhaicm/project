using Microsoft.Extensions.Logging;
using PoiNarration.Mobile.Services;
using ZXing.Net.Maui.Controls; // 1. Đảm bảo đã có dòng này

namespace PoiNarration.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .UseBarcodeReader(); // 2. THÊM DÒNG NÀY VÀO ĐÂY (Cực kỳ quan trọng)

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