using PoiNarration.Mobile.Services;
namespace PoiNarration.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "app.db3");
        builder.Services.AddSingleton(new AppDatabase(dbPath));

        return builder.Build();
    }
}
