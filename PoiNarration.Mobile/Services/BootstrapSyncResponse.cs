using PoiNarration.Core.Models;

namespace PoiNarration.Mobile.Services;

public class BootstrapSyncResponse
{
    public List<Booth> Booths { get; set; } = new();
    public List<BoothMenuItem> MenuItems { get; set; } = new();
}