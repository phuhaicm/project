using Microsoft.Extensions.Logging;
using PoiNarration.Mobile.Services;
using PoiNarration.Mobile.Views;
using ZXing.Net.Maui.Controls;

namespace PoiNarration.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .UseBarcodeReader();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // 1. Database
        builder.Services.AddSingleton<AppDatabase>(_ =>
            new AppDatabase(Constants.DatabasePath));

        // 2. Services
        // Thêm IGeolocation mặc định của máy để tính khoảng cách và lấy tọa độ
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);

        builder.Services.AddSingleton<LocationService>();
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<SyncService>();
        builder.Services.AddSingleton<NarrationService>();
        builder.Services.AddSingleton<GeofenceService>();
        builder.Services.AddSingleton<LocationTrackingService>();
        builder.Services.AddSingleton<AutoBoothNavigatorService>();
        builder.Services.AddSingleton<GpsModeStateService>();

        // 3. Pages
        builder.Services.AddTransient<BoothListPage>();
        builder.Services.AddTransient<BoothByZonePage>();
        builder.Services.AddTransient<BoothDetailPage>();
        builder.Services.AddTransient<MapPage>();
        builder.Services.AddTransient<QrScanPage>();
        builder.Services.AddSingleton<VisitorSessionService>();

        return builder.Build();
    }
}