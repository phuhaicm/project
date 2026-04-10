using SQLite;

namespace PoiNarration.Core.Models;

public class PlaybackLog
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

    // Trường này cực kỳ quan trọng để Mobile biết cái nào đã sync lên API rồi
    public bool IsSynced { get; set; } = false;
}