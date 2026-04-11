using SQLite; // Nhớ thêm using này
namespace PoiNarration.Core.Models;

public class BoothMenuItem
{
    [PrimaryKey] // BẮT BUỘC PHẢI CÓ DÒNG NÀY
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BoothId { get; set; } = "";

    // Tên món ăn (Việt - Anh)
    public string Name { get; set; } = "";        // Tên chính (thường là tiếng Việt)
    public string? NameEn { get; set; }           // Tên tiếng Anh

    // Mô tả món ăn (Việt - Anh)
    public string Description { get; set; } = ""; // Mô tả tiếng Việt
    public string? DescriptionEn { get; set; }    // Mô tả tiếng Anh

    // Giá cả (VND - USD)
    public decimal Price { get; set; }            // Giá VND
    public decimal PriceUsd { get; set; }        // Giá USD (nếu có)

    public string ImageUrl { get; set; } = "";

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}