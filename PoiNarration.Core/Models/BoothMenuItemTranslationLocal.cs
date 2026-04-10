using SQLite;

namespace PoiNarration.Core.Models;

public class BoothMenuItemTranslationLocal
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string MenuItemId { get; set; } = "";
    public string LanguageCode { get; set; } = "vi";

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}