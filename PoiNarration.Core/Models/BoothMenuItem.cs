using SQLite;

namespace PoiNarration.Core.Models;

public class BoothMenuItem
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Indexed]
    public string BoothId { get; set; } = "";

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public double Price { get; set; }

    public string ImageUrl { get; set; } = "";
}