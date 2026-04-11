namespace PoiNarration.Mobile.Models;

public class BoothCardVm
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";
    public string ZoneText { get; set; } = "";
    public string PriorityText { get; set; } = "";
    public string RadiusText { get; set; } = "";

    public string ImageUrl { get; set; } = "";

    public string? MapUrl { get; set; }
    public string? TtsScriptVi { get; set; }
    public string? TtsScriptEn { get; set; }
    public string? AudioUrlVi { get; set; }
    public string? AudioUrlEn { get; set; }

    public bool IsActive { get; set; }
}