namespace PoiNarration.Api.DTOs.Booths;

public class BoothTranslationDto
{
    public string LanguageCode { get; set; } = "vi";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? TtsScript { get; set; }
    public string? AudioUrl { get; set; }
}
