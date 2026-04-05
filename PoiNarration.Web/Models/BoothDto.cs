namespace PoiNarration.Web.Models;

public class BoothDto
{
    public string Id { get; set; } = "";
    public string ZoneId { get; set; } = "";
    public string NameVi { get; set; } = "";
    public string NameEn { get; set; } = "";
    public string DescVi { get; set; } = "";
    public string DescEn { get; set; } = "";
    public double Lat { get; set; }
    public double Lng { get; set; }
    public int RadiusMeters { get; set; }
    public int Priority { get; set; }
    public string? OwnerUserId { get; set; }
}
