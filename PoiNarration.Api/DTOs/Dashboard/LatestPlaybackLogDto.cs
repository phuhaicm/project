namespace PoiNarration.Api.DTOs.Dashboard;

public class LatestPlaybackLogDto
{
    public int Id { get; set; }
    public string BoothId { get; set; } = "";
    public string BoothName { get; set; } = "";
    public string TriggerType { get; set; } = "";
    public string Language { get; set; } = "";
    public int DurationSeconds { get; set; }
    public DateTime PlayedAtUtc { get; set; }
}