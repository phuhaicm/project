using PoiNarration.Core.Models; // Dùng hàng từ Core
using PoiNarration.Mobile.Models;
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

        var data = await _apiService.GetBootstrapAsync();
        if (data == null)
            throw new Exception("Không lấy được dữ liệu bootstrap từ API.");

        await _db.SaveBootstrapDataAsync(data);
    }


    public async Task SyncPlaybackLogsAsync()
    {
        // 1. Lấy danh sách log chưa sync. 
        // Lúc này 'log' sẽ tự hiểu là kiểu PoiNarration.Core.Models.PlaybackLog
        var unsyncedLogs = await _db.GetUnsyncedPlaybackLogsAsync();

        if (unsyncedLogs == null || !unsyncedLogs.Any()) return;

        foreach (var log in unsyncedLogs)
        {
            try
            {
                // 2. Đẩy lên API qua DTO Request
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

                // 3. Cập nhật trạng thái IsSynced = true dưới SQLite
                await _db.MarkPlaybackLogSyncedAsync(log.Id);
            }
            catch (Exception ex)
            {
                // Nếu mất mạng hoặc API lỗi, log này vẫn nằm dưới máy (IsSynced = false)
                // break để không cố gửi các log tiếp theo khi đang lỗi mạng
                Debug.WriteLine($"[Lỗi SyncLog]: {ex.Message}");
                break;
            }
        }
    }

}