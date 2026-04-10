namespace PoiNarration.Api.Models.Entities;

public class Booth
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ZoneId { get; set; } = "";

    public string NameVi { get; set; } = "";
    public string NameEn { get; set; } = "";

    public string DescVi { get; set; } = "";
    public string DescEn { get; set; } = "";

    public double Lat { get; set; }
    public double Lng { get; set; }

    public int RadiusMeters { get; set; } = 25;
    public int Priority { get; set; } = 1;

    public string? OwnerUserId { get; set; }

    public string? ImageUrl { get; set; }
    public string? MapUrl { get; set; }

    public string? TtsScriptVi { get; set; }
    public string? TtsScriptEn { get; set; }

    public string? AudioUrlVi { get; set; }
    public string? AudioUrlEn { get; set; }

    public bool IsActive { get; set; } = true;
}
