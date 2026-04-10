using PoiNarration.Core.Models;
using PoiNarration.Mobile.Models;
using System.Runtime.Versioning;

namespace PoiNarration.Mobile.Services;


public class NarrationService
{
    private readonly SemaphoreSlim _speakLock = new(1, 1);
    private CancellationTokenSource? _currentCts;

    private string? _lastBoothId;
    private DateTime _lastPlayedUtc = DateTime.MinValue;

    private readonly TimeSpan _cooldown = TimeSpan.FromMinutes(2);
    private readonly AppDatabase _db;
    private readonly ApiService _apiService;
    private bool _isSpeaking = false;
    private CancellationTokenSource? _speakCts;

    public NarrationService(AppDatabase db, ApiService apiService)
    {
        _db = db;
        _apiService = apiService;

    }

    public bool IsSpeaking => _isSpeaking;

    public async Task SpeakBoothAsync(Booth booth, string triggerType, Location? currentLocation = null)
    {
        // 1. Kiểm tra Cooldown (Chỉ chặn nếu là phát tự động qua GPS, bấm Manual thì cho qua)
        if (triggerType != "Manual" && _lastBoothId == booth.Id && DateTime.UtcNow - _lastPlayedUtc < _cooldown)
        {
            return;
        }
   

        // 2. Lock để đảm bảo tại một thời điểm chỉ xử lý một yêu cầu phát
        await _speakLock.WaitAsync();
        try
        {
            if (_isSpeaking) return;

            // --- BẮT ĐẦU PHẦN THÊM MỚI: LOGIC ĐA NGÔN NGỮ ---
            var lang = LanguageService.CurrentLanguage;

            // Tìm bản dịch theo thứ tự ưu tiên: Ngôn ngữ hiện tại -> Tiếng Anh -> Tiếng Việt
            var translation = await _db.GetBoothTranslationAsync(booth.Id, lang)
                             ?? await _db.GetBoothTranslationAsync(booth.Id, "en")
                             ?? await _db.GetBoothTranslationAsync(booth.Id, "vi");

            // Chọn kịch bản đọc (Script) theo thứ tự ưu tiên
            var script = translation?.TtsScript
                        ?? booth.TtsScriptEn
                        ?? booth.TtsScriptVi
                        ?? booth.DescVi;

            // Lấy Audio URL (nếu sau này bạn muốn phát file thay vì đọc text)
            var audioUrl = translation?.AudioUrl;

            if (string.IsNullOrWhiteSpace(script)) return;
            // --- KẾT THÚC PHẦN THÊM MỚI ---

            _isSpeaking = true;
            _currentCts?.Cancel();
            _currentCts = new CancellationTokenSource();
            _speakCts = CancellationTokenSource.CreateLinkedTokenSource(_currentCts.Token);

            try
            {
                var locales = await TextToSpeech.Default.GetLocalesAsync();
                Locale? locale = null;

                // Chọn giọng đọc phù hợp với ngôn ngữ hiện tại
                if (LanguageService.IsVi)
                    locale = locales.FirstOrDefault(x => x.Language.StartsWith("vi"));
                else
                    locale = locales.FirstOrDefault(x => x.Language.StartsWith("en"));

                // Thực hiện đọc văn bản
                await TextToSpeech.Default.SpeakAsync(script, new SpeechOptions
                {
                    Locale = locale,
                    Pitch = 1.0f,
                    Volume = 1.0f
                }, _speakCts.Token);

                // 3. Lưu Log Local sau khi phát xong
                var log = new PlaybackLog
                {
                    BoothId = booth.Id,
                    TriggerType = triggerType,
                    Language = lang, // Sử dụng ngôn ngữ thực tế đã chọn
                    PlayedAtUtc = DateTime.UtcNow,
                    Lat = currentLocation?.Latitude ?? 0,
                    Lng = currentLocation?.Longitude ?? 0,
                    DurationSeconds = 10, // Bạn có thể tính toán thời gian thực tế nếu cần
                    IsCompleted = true,
                    SessionId = Guid.NewGuid().ToString(),
                    IsSynced = false
                };
                await _db.SavePlaybackLogAsync(log);

                // 4. Đồng bộ lên API Server
                try
                {
                    await _apiService.PostPlaybackLogAsync(new PlaybackLogRequest
                    {
                        BoothId = booth.Id,
                        TriggerType = triggerType,
                        Language = lang,
                        DurationSeconds = 10,
                        IsCompleted = true,
                        SessionId = log.SessionId
                    });
                    log.IsSynced = true;
                    await _db.SavePlaybackLogAsync(log); // Cập nhật trạng thái đã sync
                }
                catch
                {
                    // Offline thì log vẫn nằm ở Local với IsSynced = false để sync sau
                }

                // Cập nhật trạng thái lần phát cuối
                _lastBoothId = booth.Id;
                _lastPlayedUtc = DateTime.UtcNow;
            }
            finally
            {
                _isSpeaking = false;
            }
        }
        finally
        {
            _speakLock.Release();
        }
    }
    public Task StopAsync()
    {
        _speakCts?.Cancel();
        _currentCts?.Cancel();
        return Task.CompletedTask;
    }


}