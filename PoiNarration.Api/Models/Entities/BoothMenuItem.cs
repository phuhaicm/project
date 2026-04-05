namespace PoiNarration.Api.Models.Entities;

public class BoothMenuItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BoothId { get; set; } = "";

    public string Name { get; set; } = "";
    public string? NameEn { get; set; } // <--- Thêm cột này

    public string Description { get; set; } = "";
    public string? DescriptionEn { get; set; } // <--- Thêm cột này

    public decimal Price { get; set; }
    public decimal? PriceUsd { get; set; } // <--- Thêm cột này

    public string ImageUrl { get; set; } = "";
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}