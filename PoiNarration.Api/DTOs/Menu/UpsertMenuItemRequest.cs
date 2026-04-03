namespace PoiNarration.Api.DTOs.Menu;

public class UpsertMenuItemRequest
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = "";
}
