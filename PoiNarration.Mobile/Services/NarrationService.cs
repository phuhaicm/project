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

    public async Task SpeakBoothAsync(Booth booth, string triggerType, Location? currentLocation = null)
    {
        // Chỉ chặn lặp với Manual/QR; GPS phải được ưu tiên chen vào ngay
        if (triggerType != "GPS")
        {
            if (_lastBoothId == booth.Id &&
                DateTime.UtcNow - _lastPlayedUtc < _cooldown)
            {
                return;
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

            // Ghi log kết quả
            var log = new PlaybackLog
            {
                BoothId = booth.Id,
                TriggerType = triggerType,
                Language = lang,
                PlayedAtUtc = DateTime.UtcNow,
                Lat = currentLocation?.Latitude ?? 0,
                Lng = currentLocation?.Longitude ?? 0,
                DurationSeconds = 10,
                IsCompleted = true,
                SessionId = Guid.NewGuid().ToString(),
                IsSynced = false
            };

            await _db.SavePlaybackLogAsync(log);

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
                await _db.SavePlaybackLogAsync(log);
            }
            catch
            {
                // offline -> giữ local để sync sau
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