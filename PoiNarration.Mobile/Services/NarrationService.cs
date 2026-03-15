using PoiNarration.Core.Models;

namespace PoiNarration.Mobile.Services;

public class NarrationService
{
    private readonly AppDatabase _db;
    private bool _isSpeaking = false;

    public NarrationService(AppDatabase db)
    {
        _db = db;
    }

    public bool IsSpeaking => _isSpeaking;

    public async Task SpeakBoothAsync(Booth booth, string triggerType, Location? currentLocation = null)
    {
        if (_isSpeaking) return;

        var text = LanguageService.IsVi ? booth.DescVi : booth.DescEn;
        if (string.IsNullOrWhiteSpace(text)) return;

        _isSpeaking = true;

        try
        {
            var locales = await TextToSpeech.Default.GetLocalesAsync();
            Locale? locale = null;

            if (LanguageService.IsVi)
                locale = locales.FirstOrDefault(x => x.Language.StartsWith("vi"));
            else
                locale = locales.FirstOrDefault(x => x.Language.StartsWith("en"));

            await TextToSpeech.Default.SpeakAsync(text, new SpeechOptions
            {
                Locale = locale,
                Pitch = 1.0f,
                Volume = 1.0f
            });

            var log = new PlaybackLog
            {
                BoothId = booth.Id,
                TriggerType = triggerType,
                Language = LanguageService.Current,
                PlayedAtUtc = DateTime.UtcNow,
                Lat = currentLocation?.Latitude ?? 0,
                Lng = currentLocation?.Longitude ?? 0,
                IsCompleted = true
            };

            await _db.InsertPlaybackLogAsync(log);
        }
        finally
        {
            _isSpeaking = false;
        }
    }
}