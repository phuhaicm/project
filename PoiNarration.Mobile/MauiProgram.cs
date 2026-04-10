//using Android.Net;
using Microsoft.Extensions.Logging;
using PoiNarration.Mobile.Services;
using PoiNarration.Mobile.Views;
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
        builder.Services.AddSingleton<AppDatabase>(_ =>
    new AppDatabase(Constants.DatabasePath));


        // Services


        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<SyncService>();
        builder.Services.AddSingleton<NarrationService>();
        builder.Services.AddSingleton<GeofenceService>();
        builder.Services.AddSingleton<LocationTrackingService>();

        // Pages
        builder.Services.AddTransient<BoothListPage>();
        builder.Services.AddTransient<BoothByZonePage>();
        builder.Services.AddTransient<BoothDetailPage>();
        builder.Services.AddTransient<MapPage>();
        builder.Services.AddTransient<QrScanPage>();




        return builder.Build();
    }
}