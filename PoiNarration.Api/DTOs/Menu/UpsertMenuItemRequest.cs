namespace PoiNarration.Api.DTOs.Menu;

public class UpsertMenuItemRequest
{
    public string Name { get; set; } = "";
    // Thêm ngăn chứa cho tên tiếng Anh
    public string NameEn { get; set; } = "";

    public string Description { get; set; } = "";
    // Thêm ngăn chứa cho mô tả tiếng Anh
    public string DescriptionEn { get; set; } = "";

    public decimal Price { get; set; }
    // Thêm ngăn chứa cho giá USD
    public decimal PriceUsd { get; set; }


    public string ImageUrl { get; set; } = "";
}