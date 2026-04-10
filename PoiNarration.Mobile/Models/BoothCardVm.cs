namespace PoiNarration.Mobile.Models
{
    public class BoothCardVm
    {
        public string Id { get; set; }
        public string ZoneId { get; set; }

        public string NameVi { get; set; }
        public string NameEn { get; set; }

        public string DescVi { get; set; }
        public string DescEn { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

        public int RadiusMeters { get; set; }
        public int Priority { get; set; }

        // Có thể bỏ OwnerUserId đi nếu trên App không cần xem thông tin người tạo
        public string OwnerUserId { get; set; }

        public string ImageUrl { get; set; }
        public string MapUrl { get; set; }

        public string TtsScriptVi { get; set; }
        public string TtsScriptEn { get; set; }

        public string AudioUrlVi { get; set; }
        public string AudioUrlEn { get; set; }

        public bool IsActive { get; set; }
    }
}