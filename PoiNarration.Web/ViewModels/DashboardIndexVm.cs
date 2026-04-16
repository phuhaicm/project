using PoiNarration.Web.Models;

namespace PoiNarration.Web.ViewModels;

public class DashboardIndexVm
{
    public DashboardSummaryDto Summary { get; set; } = new();
    public List<DashboardTopBoothDto> TopBooths { get; set; } = new();
    public List<LatestPlaybackLogDto> LatestLogs { get; set; } = new();

    // THÊM MỚI
    public List<LanguageCountVm> TopVisitorLanguages { get; set; } = new();
    public List<BoothCountVm> TopVisitedBooths { get; set; } = new();
}