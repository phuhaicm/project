using PoiNarration.Web.Models;

namespace PoiNarration.Web.ViewModels;

public class DashboardIndexVm
{
    public DashboardSummaryDto Summary { get; set; } = new();
    public List<DashboardTopBoothDto> TopBooths { get; set; } = new();
    public List<LatestPlaybackLogDto> LatestLogs { get; set; } = new();
}
