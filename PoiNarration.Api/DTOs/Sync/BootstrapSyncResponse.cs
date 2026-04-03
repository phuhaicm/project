using PoiNarration.Api.Models.Entities;

namespace PoiNarration.Api.DTOs.Sync;

public class BootstrapSyncResponse
{
    public List<Booth> Booths { get; set; } = new();
    public List<BoothMenuItem> MenuItems { get; set; } = new();
}
