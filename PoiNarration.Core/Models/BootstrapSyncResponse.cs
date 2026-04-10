// Lưu ý: Namespace bây giờ là Core chứ không phải Mobile nữa nhé
namespace PoiNarration.Core.Models
{
    public class BootstrapSyncResponse
    {
        public List<Zone> Zones { get; set; } = new();
        public List<Booth> Booths { get; set; } = new();
        public List<BoothMenuItem> MenuItems { get; set; } = new();

        public List<BoothTranslationLocal> BoothTranslations { get; set; } = new();
        public List<BoothMenuItemTranslationLocal> MenuTranslations { get; set; } = new();
    }
}