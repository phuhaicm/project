namespace PoiNarration.Api.DTOs.Booths;

public class UpdateBoothRequest
{
    public string ZoneId { get; set; } = "";

    public string NameVi { get; set; } = "";
    public string DescVi { get; set; } = "";

    public double Lat { get; set; }
    public double Lng { get; set; }

    public int RadiusMeters { get; set; } = 25;
    public int Priority { get; set; } = 1;

    public string? ImageUrl { get; set; }
    public string? MapUrl { get; set; }

    public string? TtsScriptVi { get; set; }
    public string? AudioUrlVi { get; set; }

    public bool IsActive { get; set; } = true;
}