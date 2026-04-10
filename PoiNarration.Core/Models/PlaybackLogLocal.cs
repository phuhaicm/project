using SQLite;

namespace PoiNarration.Core.Models;

public class PlaybackLogLocal
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string BoothId { get; set; } = "";
    public string TriggerType { get; set; } = "Manual";
    public string Language { get; set; } = "vi";

    public DateTime PlayedAtUtc { get; set; } = DateTime.UtcNow;
    public int DurationSeconds { get; set; }

    public double? Lat { get; set; }
    public double? Lng { get; set; }

    public bool IsCompleted { get; set; } = true;
    public string? SessionId { get; set; }

    public bool IsSynced { get; set; } = false;
}
