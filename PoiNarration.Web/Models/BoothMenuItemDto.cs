namespace PoiNarration.Web.Models;

public class BoothMenuItemDto
{
    public string Id { get; set; } = "";
    public string BoothId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = "";
    public DateTime UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}