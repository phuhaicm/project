using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PoiNarration.Web.ViewModels;

public class OwnerMenuItemVm
{
    public string BoothId { get; set; } = "";
    public string MenuId { get; set; } = "";

    [Required(ErrorMessage = "Tên món là bắt buộc")]
    [StringLength(100, ErrorMessage = "Tên món tối đa 100 ký tự")]
    public string Name { get; set; } = "";

    [StringLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
    public string Description { get; set; } = "";

    [Range(1000, 10000000, ErrorMessage = "Giá phải từ 1.000 đến 10.000.000")]
    public decimal Price { get; set; }

    public string ExistingImageUrl { get; set; } = "";
    public IFormFile? ImageFile { get; set; }
}
