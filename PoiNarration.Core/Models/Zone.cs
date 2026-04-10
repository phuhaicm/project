using SQLite;

namespace PoiNarration.Core.Models;

public class Zone
{
    [PrimaryKey]
    public string Id { get; set; } = "";

    public string Name { get; set; } = "";
}