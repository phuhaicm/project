namespace PoiNarration.Web.Models;

public class VisitorUserDto
{
    public string Id { get; set; } = "";
    public string VisitorCode { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string DeviceKey { get; set; } = "";
    public string PreferredLanguage { get; set; } = "vi";
    public string? Platform { get; set; }
    public string? AppVersion { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime LastActiveAtUtc { get; set; }
    public bool IsActive { get; set; }
}
