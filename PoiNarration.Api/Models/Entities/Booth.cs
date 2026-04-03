namespace PoiNarration.Api.Models.Entities;

public class Booth
{
    public string Id { get; set; } = "";
    public string ZoneId { get; set; } = "";

    public string NameVi { get; set; } = "";
    public string NameEn { get; set; } = "";

    public string DescVi { get; set; } = "";
    public string DescEn { get; set; } = "";

    public double Lat { get; set; }
    public double Lng { get; set; }

    public int RadiusMeters { get; set; } = 30;
    public int Priority { get; set; } = 1;

    public string? OwnerUserId { get; set; }
}
