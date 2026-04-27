namespace PoiNarration.Web.Models;

public class VisitorActivityDetailDto
{
    public string VisitorId { get; set; } = "";
    public string VisitorCode { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string PreferredLanguage { get; set; } = "vi";
    public string? Platform { get; set; }
    public string? AppVersion { get; set; }
    public DateTime LastActiveAtUtc { get; set; }
    public bool IsOnline { get; set; }

    public int TotalVisitedBooths { get; set; }
    public int TotalPlayedBooths { get; set; }

    public List<VisitorBoothActivityItemDto> VisitedBooths { get; set; } = new();
    public List<VisitorBoothActivityItemDto> PlayedBooths { get; set; } = new();
}

public class VisitorBoothActivityItemDto
{
    public string VisitorUserId { get; set; } = "";
    public string BoothId { get; set; } = "";
    public string BoothName { get; set; } = "";
    public int Count { get; set; }
    public DateTime LastAtUtc { get; set; }
}
