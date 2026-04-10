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
        try
        {
            await _db.InitAsync();

            // 1. Gọi API lấy dữ liệu
            var data = await _apiService.GetBootstrapAsync();
            if (data == null) return;

            // 2. Thay vì foreach từng cái, hãy gọi 1 hàm duy nhất để xử lý hàng loạt (Bulk Insert)
            // Việc này nhanh hơn gấp 50-100 lần so với insert từng dòng
            await _db.SaveBootstrapDataAsync(data);

            Debug.WriteLine("Đồng bộ Bootstrap thành công!");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[Lỗi SyncBootstrap]: {ex.Message}");
            throw; // Ném lỗi để UI hiển thị thông báo cho người dùng
        }
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