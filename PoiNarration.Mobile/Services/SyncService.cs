using PoiNarration.Mobile.Models;

namespace PoiNarration.Mobile.Services;

public class SyncService
{
    private readonly ApiService _apiService;
    private readonly AppDatabase _db;

    public SyncService(ApiService apiService, AppDatabase db)
    {
        _apiService = apiService;
        _db = db;
    }

    public async Task SyncBootstrapAsync()
    {
        await _db.InitAsync();

        var data = await _apiService.GetBootstrapAsync();
        if (data == null) return;

        foreach (var zone in data.Zones)
            await _db.UpsertZoneAsync(zone);

        foreach (var booth in data.Booths)
            await _db.UpsertBoothAsync(booth);

        foreach (var item in data.MenuItems)
            await _db.UpsertMenuItemAsync(item);

        foreach (var item in data.BoothTranslations)
            await _db.UpsertBoothTranslationAsync(item);

        foreach (var item in data.MenuTranslations)
            await _db.UpsertMenuTranslationAsync(item);
    }
    public async Task SyncPlaybackLogsAsync()
    {
        var unsyncedLogs = await _db.GetUnsyncedPlaybackLogsAsync();

        foreach (var log in unsyncedLogs)
        {
            try
            {
                await _apiService.PostPlaybackLogAsync(new PlaybackLogRequest
                {
                    BoothId = log.BoothId,
                    TriggerType = log.TriggerType,
                    Language = log.Language,
                    DurationSeconds = log.DurationSeconds,
                    Lat = log.Lat,
                    Lng = log.Lng,
                    IsCompleted = log.IsCompleted,
                    SessionId = log.SessionId
                });

                await _db.MarkPlaybackLogSyncedAsync(log.Id);
            }
            catch
            {
                // gặp lỗi thì dừng, để lần sau sync tiếp
                break;
            }
        }
    }
}