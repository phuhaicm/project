using SQLite;

namespace PoiNarration.Core.Models;

public class Booth
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Indexed]
    public string ZoneId { get; set; } = "";

    public string NameVi { get; set; } = "";
    public string NameEn { get; set; } = "";

    public string DescVi { get; set; } = "";
    public string DescEn { get; set; } = "";

    public int Priority { get; set; } = 1;

    // Cho GPS tuần 3
    public double Lat { get; set; }
    public double Lng { get; set; }
    public double RadiusMeters { get; set; } = 10;

    // Offline: lưu local path hoặc URL (tuần 5 mới làm media)
    public string ImageUrl { get; set; } = "";
}