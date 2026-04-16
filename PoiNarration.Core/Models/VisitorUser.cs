using SQLite;

namespace PoiNarration.Core.Models;

public class VisitorUser
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string VisitorCode { get; set; } = "";       // VIS-8F29A3
    public string DisplayName { get; set; } = "";       // Khách VIS-8F29A3

    public string DeviceKey { get; set; } = "";         // định danh app/device
    public string PreferredLanguage { get; set; } = "vi";

    public string? Platform { get; set; }               // Android
    public string? AppVersion { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastActiveAtUtc { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
