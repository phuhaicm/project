using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PoiNarration.Web.ViewModels;

public class BoothCreateVm
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Zone là bắt buộc")]
    public string ZoneId { get; set; } = "zone-a";

    [Required(ErrorMessage = "Tên tiếng Việt là bắt buộc")]
    public string NameVi { get; set; } = "";

    public string DescVi { get; set; } = "";

    public double Lat { get; set; }
    public double Lng { get; set; }

    public int RadiusMeters { get; set; } = 25;
    public int Priority { get; set; } = 1;

    public string? ExistingImageUrl { get; set; }
    public IFormFile? ImageFile { get; set; }

    public string? MapUrl { get; set; }

    // Chỉ nhập kịch bản tiếng Việt gốc, các ngôn ngữ khác API tự sinh
    public string? TtsScriptVi { get; set; }
    public string? AudioUrlVi { get; set; }

    public bool IsActive { get; set; } = true;
}