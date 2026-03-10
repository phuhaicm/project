using SQLite;

namespace PoiNarration.Core.Models;

public class Zone
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string NameVi { get; set; } = "";
    public string NameEn { get; set; } = "";

    // Tùy bạn: để sẵn cho GPS tuần 3
    public double CenterLat { get; set; }
    public double CenterLng { get; set; }
    public double RadiusMeters { get; set; } = 80;
}