namespace PoiNarration.Api.DTOs.PlaybackLogs;

public class CreatePlaybackLogRequest
{
    public string BoothId { get; set; } = "";
    public string TriggerType { get; set; } = "Manual";
    public string Language { get; set; } = "vi";
    public int DurationSeconds { get; set; }
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public bool IsCompleted { get; set; } = true;
    public string? SessionId { get; set; }
}