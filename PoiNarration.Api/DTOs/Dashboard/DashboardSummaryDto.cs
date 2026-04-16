namespace PoiNarration.Api.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalBooths { get; set; }
    public int TotalOwners { get; set; }
    public int TotalPlaybackLogs { get; set; }
    public int PlaybackToday { get; set; }
    public double AverageDurationSeconds { get; set; }

    // THÊM MỚI - user analytics
    public int TotalVisitors { get; set; }
    public int ActiveVisitorsToday { get; set; }
    public int OnlineVisitors { get; set; }
    public int TotalBoothVisits { get; set; }
}