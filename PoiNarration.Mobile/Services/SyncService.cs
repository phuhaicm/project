using PoiNarration.Core.Models;
using System.Diagnostics;

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

        try
        {
            var data = await _apiService.GetBootstrapAsync();
            if (data == null)
                throw new Exception("API trả về dữ liệu bootstrap null.");

            await _db.SaveBootstrapDataAsync(data);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[SyncBootstrapAsync lỗi]: {ex}");
            throw new Exception($"Không đồng bộ được bootstrap. Chi tiết: {ex.Message}", ex);
        }
    }

    public async Task SyncPlaybackLogsAsync()
    {
        var unsyncedLogs = await _db.GetUnsyncedPlaybackLogsAsync();

        if (unsyncedLogs == null || !unsyncedLogs.Any()) return;

        foreach (var log in unsyncedLogs)
        {
            try
            {
                await _apiService.PostPlaybackLogAsync(new PlaybackLogRequest
                {
                    VisitorUserId = string.IsNullOrWhiteSpace(log.VisitorUserId)
        ? Preferences.Get("visitor_id_server", "")
        : log.VisitorUserId,
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
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi SyncPlaybackLog]: {ex.Message}");
                break;
            }
        }
    }

    // TẠM THỜI CHƯA SYNC API ĐỂ TRÁNH LỖI BUILD
    public async Task SyncBoothVisitLogsAsync()
    {
        await _db.InitAsync();

        var unsyncedLogs = await _db.GetUnsyncedBoothVisitLogsAsync();
        if (unsyncedLogs == null || !unsyncedLogs.Any()) return;

        foreach (var log in unsyncedLogs)
        {
            try
            {
                log.VisitorUserId = Preferences.Get("visitor_id_server", "");

                await _apiService.PostBoothVisitLogAsync(log);
                await _db.MarkBoothVisitLogSyncedAsync(log.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Lỗi SyncBoothVisitLogs]: {ex.Message}");
                break;
            }
        }
    }

}
