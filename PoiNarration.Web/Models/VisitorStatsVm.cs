namespace PoiNarration.Web.Models;

public class VisitorStatsVm
{
    public int TotalVisitors { get; set; }
    public int TotalVisitLogs { get; set; }
    public int TotalPlaybackLogs { get; set; }

    public List<LanguageCountVm> TopLanguages { get; set; } = new();
    public List<BoothCountVm> TopBoothsByVisit { get; set; } = new();
    public List<BoothCountVm> TopBoothsByPlayback { get; set; } = new();
}

public class LanguageCountVm
{
    public string Language { get; set; } = "";
    public int Count { get; set; }
}

public class BoothCountVm
{
    public string BoothId { get; set; } = "";
    public int Count { get; set; }
}
