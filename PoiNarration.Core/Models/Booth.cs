using SQLite;

namespace PoiNarration.Core.Models;

public class Booth
{
    [PrimaryKey]
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

    public string? ImageUrl { get; set; }
    [Ignore]
    public string DisplayImageUrl
    {
        get
        {
            if (string.IsNullOrEmpty(ImageUrl))
                return "default_image.png";

            // Trả về thẳng ImageUrl vì link này sẽ được mình xử lý "sạch" ở Bước 2
            return ImageUrl;
        }
    }
    public string? MapUrl { get; set; }

    public string? TtsScriptVi { get; set; }
    public string? TtsScriptEn { get; set; }

    public string? AudioUrlVi { get; set; }
    public string? AudioUrlEn { get; set; }

    public bool IsActive { get; set; } = true;
    public string? QrCodeUrl { get; set; }
}