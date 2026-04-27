using PoiNarration.Core.Models;
using PoiNarration.Mobile.Models;

namespace PoiNarration.Mobile.Services;

public class NarrationService
{
    private readonly SemaphoreSlim _stateLock = new(1, 1);

    private CancellationTokenSource? _currentCts;
    private CancellationTokenSource? _speakCts;

    private string? _lastBoothId;
    private DateTime _lastPlayedUtc = DateTime.MinValue;
    private readonly TimeSpan _cooldown = TimeSpan.FromMinutes(2);

    private readonly AppDatabase _db;
    private readonly ApiService _apiService;

    private bool _isSpeaking = false;

    public NarrationService(AppDatabase db, ApiService apiService)
    {
        _db = db;
        _apiService = apiService;
    }

    public bool IsSpeaking => _isSpeaking;
    // Hàm này cho phép truyền vào bất kỳ chuỗi chữ nào, nó sẽ đọc ngay lập tức
    public async Task SpeakTextAsync(string text, string lang)
    {
        // 1. Dừng ngay câu đang đọc dở (nếu có) để không bị đè giọng
        await StopAsync();

        if (string.IsNullOrWhiteSpace(text)) return;

        try
        {
            // 2. Tìm đúng giọng điệu theo ngôn ngữ
            var locales = await TextToSpeech.Default.GetLocalesAsync();
            var localePrefix = LanguageService.GetTtsLocalePrefix(lang);
            var locale = locales.FirstOrDefault(x => x.Language.StartsWith(localePrefix))
                      ?? locales.FirstOrDefault(x => x.Language.StartsWith("en"));

            // 3. Khởi tạo Token mới và bắt đầu đọc
            _currentCts = new CancellationTokenSource();
            await TextToSpeech.Default.SpeakAsync(text, new SpeechOptions { Locale = locale }, _currentCts.Token);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Lỗi TTS đọc chữ]: {ex}");
        }
    }
    public async Task SpeakBoothAsync(Booth booth, string triggerType, Location? currentLocation = null)
    {
        // Chỉ chặn lặp với Manual/QR; GPS phải được ưu tiên chen vào ngay
        if (triggerType == "GPS")
        {
            if (_lastBoothId == booth.Id &&
                DateTime.UtcNow - _lastPlayedUtc < _cooldown)
            {
                return; // Đứng yên ở 1 booth chưa quá 2 phút thì không tự động đọc lại
            }
        }

        CancellationTokenSource speakCts;

        // Chuẩn bị state + hủy lời đọc cũ
        await _stateLock.WaitAsync();
        try
        {
            _currentCts?.Cancel();
            _speakCts?.Cancel();

            _currentCts = new CancellationTokenSource();
            _speakCts = CancellationTokenSource.CreateLinkedTokenSource(_currentCts.Token);
            speakCts = _speakCts;

            _isSpeaking = true;
        }
        finally
        {
            _stateLock.Release();
        }

        try
        {
            var lang = LanguageService.CurrentLanguage;

            var translation = await _db.GetBoothTranslationAsync(booth.Id, lang)
                              ?? await _db.GetBoothTranslationAsync(booth.Id, "en")
                              ?? await _db.GetBoothTranslationAsync(booth.Id, "vi");

            // --- ĐOẠN 1: Xử lý Script ưu tiên (Thay thế theo yêu cầu) ---
            var script = translation?.TtsScript
                         ?? (lang == "vi" ? booth.TtsScriptVi : booth.TtsScriptEn)
                         ?? booth.DescVi;

            if (string.IsNullOrWhiteSpace(script))
                return;
            try
            {
                // 1. Lấy danh sách món ăn của gian hàng này
                var menuItems = await _db.GetMenuByBoothAsync(booth.Id);

                if (menuItems != null && menuItems.Any())
                {
                    // 2. Thêm một câu chuyển ý để giọng đọc tự nhiên hơn
                    script += lang == "vi" ? " Thực đơn của gian hàng gồm có: " : " Our menu includes: ";

                    // 3. Lặp qua từng món và lấy đúng ngôn ngữ
                    foreach (var item in menuItems)
                    {
                        var itemTransList = await _db.GetMenuTranslationsAsync(item.Id);
                        var itemTrans = itemTransList?.FirstOrDefault(x => x.LanguageCode == lang)
                                     ?? itemTransList?.FirstOrDefault(x => x.LanguageCode == "en")
                                     ?? itemTransList?.FirstOrDefault(x => x.LanguageCode == "vi");

                        var itemName = itemTrans?.Name ?? item.Name;

                        // Thêm tên món vào kịch bản. 
                        // Cực kỳ quan trọng: Phải có DẤU CHẤM (.) ở cuối để AI biết ngắt nhịp thở, nếu không nó sẽ đọc rap dính liền nhau!
                        script += $"{itemName}. ";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Lỗi đọc menu]: {ex}");
            }

            // --- ĐOẠN 2: Mapping Locale linh hoạt (Thay thế theo yêu cầu) ---
            var locales = await TextToSpeech.Default.GetLocalesAsync();
            var localePrefix = LanguageService.GetTtsLocalePrefix(lang);

            Locale? locale = locales.FirstOrDefault(x => x.Language.StartsWith(localePrefix))
                          ?? locales.FirstOrDefault(x => x.Language.StartsWith("en"));

            // Debug danh sách locale nếu cần
            foreach (var l in locales)
            {
                System.Diagnostics.Debug.WriteLine($"TTS locale available: {l.Language} - {l.Name}");
            }

            await TextToSpeech.Default.SpeakAsync(
                script,
                new SpeechOptions
                {
                    Locale = locale,
                    Pitch = 1.0f,
                    Volume = 1.0f
                },
                speakCts.Token);

            var visitorId = Preferences.Get("visitor_id_server", "");
            var sessionId = Preferences.Get("session_id", Guid.NewGuid().ToString());

            var log = new PlaybackLog
            {
                VisitorUserId = visitorId,
                BoothId = booth.Id,
                TriggerType = triggerType,
                Language = lang,
                PlayedAtUtc = DateTime.UtcNow,
                Lat = currentLocation?.Latitude ?? 0,
                Lng = currentLocation?.Longitude ?? 0,
                DurationSeconds = 10,
                IsCompleted = true,
                SessionId = sessionId,
                IsSynced = false
            };

            // 1. lưu local trước
            await _db.SavePlaybackLogAsync(log);

            // 2. gửi ngay lên API để admin thấy gần realtime
            try
            {
                await _apiService.PostPlaybackLogAsync(new PlaybackLogRequest
                {
                    VisitorUserId = log.VisitorUserId,
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
                // API lỗi thì giữ local, SyncService sẽ retry sau
            }


            _lastBoothId = booth.Id;
            _lastPlayedUtc = DateTime.UtcNow;
        }
        catch (OperationCanceledException)
        {
            // Booth cũ bị hủy để ưu tiên booth mới
        }
        finally
        {
            await _stateLock.WaitAsync();
            try
            {
                if (_speakCts == speakCts)
                {
                    _isSpeaking = false;
                }
            }
            finally
            {
                _stateLock.Release();
            }
        }
    }

    public Task StopAsync()
    {
        _speakCts?.Cancel();
        _currentCts?.Cancel();
        return Task.CompletedTask;
    }
}