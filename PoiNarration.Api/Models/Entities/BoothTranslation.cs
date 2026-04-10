namespace PoiNarration.Api.Models.Entities;

public class BoothTranslation
{
    public int Id { get; set; }

    public string BoothId { get; set; } = "";
    public string LanguageCode { get; set; } = "vi";

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public string? TtsScript { get; set; }
    public string? AudioUrl { get; set; }
}