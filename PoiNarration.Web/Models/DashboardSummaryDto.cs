namespace PoiNarration.Web.Models;

public class DashboardSummaryDto
{
    public int TotalBooths { get; set; }
    public int TotalOwners { get; set; }
    public int TotalPlaybackLogs { get; set; }
    public int PlaybackToday { get; set; }
    public double AverageDurationSeconds { get; set; }
}
