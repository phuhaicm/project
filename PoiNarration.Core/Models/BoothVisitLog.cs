using SQLite;

namespace PoiNarration.Core.Models;

public class BoothVisitLog
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string VisitorUserId { get; set; } = "";
    public string BoothId { get; set; } = "";

    // ManualOpen / QR / GPS / MapTap / NearestButton
    public string TriggerType { get; set; } = "ManualOpen";

    public string Language { get; set; } = "vi";
    public DateTime VisitedAtUtc { get; set; } = DateTime.UtcNow;

    public string? SessionId { get; set; }

    public double? Lat { get; set; }
    public double? Lng { get; set; }

    public bool IsSynced { get; set; } = false;
}
