using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Models.Entities;
using PoiNarration.Core.Models;

namespace PoiNarration.Api.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (!await db.AppUsers.AnyAsync())
        {
            var users = new List<AppUser>
            {
                new AppUser { Id = "admin", Username = "admin", Password = "123456", PasswordHash = "123456", FullName = "Administrator", Role = "Admin" }
            };

            for (int i = 1; i <= 10; i++)
            {
                users.Add(new AppUser
                {
                    Id = $"owner{i}",
                    Username = $"owner{i}",
                    Password = "123456",
                    PasswordHash = "123456",
                    FullName = $"Owner Booth {i:D2}",
                    Role = "Owner"
                });
            }

            db.AppUsers.AddRange(users);
            await db.SaveChangesAsync();
        }
        if (!await db.VisitorUsers.AnyAsync())
        {
            var visitors = new List<VisitorUser>
    {
        new VisitorUser
        {
            Id = "visitor-001",
            VisitorCode = "VIS-001",
            DisplayName = "Khách VIS-001",
            DeviceKey = "device-demo-001",
            PreferredLanguage = "vi",
            Platform = "Android",
            AppVersion = "1.0.0",
            CreatedAtUtc = DateTime.UtcNow.AddDays(-3),
            LastActiveAtUtc = DateTime.UtcNow.AddMinutes(-30),
            IsActive = true
        },
        new VisitorUser
        {
            Id = "visitor-002",
            VisitorCode = "VIS-002",
            DisplayName = "Khách VIS-002",
            DeviceKey = "device-demo-002",
            PreferredLanguage = "en",
            Platform = "Android",
            AppVersion = "1.0.0",
            CreatedAtUtc = DateTime.UtcNow.AddDays(-2),
            LastActiveAtUtc = DateTime.UtcNow.AddMinutes(-10),
            IsActive = true
        },
        new VisitorUser
        {
            Id = "visitor-003",
            VisitorCode = "VIS-003",
            DisplayName = "Khách VIS-003",
            DeviceKey = "device-demo-003",
            PreferredLanguage = "ja",
            Platform = "Android",
            AppVersion = "1.0.0",
            CreatedAtUtc = DateTime.UtcNow.AddDays(-1),
            LastActiveAtUtc = DateTime.UtcNow.AddMinutes(-5),
            IsActive = true
        }
    };

            db.VisitorUsers.AddRange(visitors);
            await db.SaveChangesAsync();
        }

        if (!await db.Booths.AnyAsync())
        {
            var booths = new List<Booth>
            {
                new Booth { Id = "booth-01", ZoneId = "zone-a", NameVi = "Phở Hà Nội", NameEn = "Hanoi Pho", DescVi = "Gian hàng phở Hà Nội với nước dùng đậm đà và hương vị truyền thống.", DescEn = "Traditional Hanoi pho booth with rich broth and authentic flavor.", Lat = 10.7768d, Lng = 106.7008d, RadiusMeters = 25, Priority = 1, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-01-menu-01.png", OwnerUserId = "owner1" },
                new Booth { Id = "booth-02", ZoneId = "zone-a", NameVi = "Bún Bò Huế", NameEn = "Hue Beef Noodles", DescVi = "Gian hàng bún bò Huế với vị cay thơm đậm chất miền Trung.", DescEn = "Hue beef noodles booth with a spicy and aromatic central Vietnam taste.", Lat = 10.77698d, Lng = 106.7008d, RadiusMeters = 25, Priority = 2, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-02-menu-01.png", OwnerUserId = "owner2" },
                new Booth { Id = "booth-03", ZoneId = "zone-a", NameVi = "Cơm Tấm Sài Gòn", NameEn = "Saigon Broken Rice", DescVi = "Gian hàng cơm tấm Sài Gòn với sườn nướng và chả truyền thống.", DescEn = "Saigon broken rice booth with grilled pork and traditional sides.", Lat = 10.77716d, Lng = 106.7008d, RadiusMeters = 25, Priority = 3, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-03-menu-01.png", OwnerUserId = "owner3" },
                new Booth { Id = "booth-04", ZoneId = "zone-a", NameVi = "Bánh Mì Việt", NameEn = "Vietnamese Banh Mi", DescVi = "Gian hàng bánh mì Việt giòn thơm với nhiều loại nhân hấp dẫn.", DescEn = "Vietnamese banh mi booth with crispy bread and delicious fillings.", Lat = 10.77734d, Lng = 106.7008d, RadiusMeters = 25, Priority = 4, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-04-menu-01.png", OwnerUserId = "owner4" },
                new Booth { Id = "booth-05", ZoneId = "zone-a", NameVi = "Chè Ba Miền", NameEn = "Three-Region Sweet Soup", DescVi = "Gian hàng chè ba miền với nhiều món tráng miệng ngọt mát.", DescEn = "Three-region sweet soup booth with refreshing desserts.", Lat = 10.77752d, Lng = 106.7008d, RadiusMeters = 25, Priority = 5, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-05-menu-01.png", OwnerUserId = "owner5" },
                new Booth { Id = "booth-06", ZoneId = "zone-b", NameVi = "Nem Nướng Đà Lạt", NameEn = "Dalat Grilled Pork Rolls", DescVi = "Gian hàng nem nướng Đà Lạt với hương vị thơm ngon đặc trưng.", DescEn = "Dalat grilled pork rolls booth with authentic local flavor.", Lat = 10.7768d, Lng = 106.70102d, RadiusMeters = 25, Priority = 6, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-06-menu-01.png", OwnerUserId = "owner6" },
                new Booth { Id = "booth-07", ZoneId = "zone-b", NameVi = "Bánh Xèo Miền Tây", NameEn = "Mekong Crispy Pancake", DescVi = "Gian hàng bánh xèo miền Tây giòn rụm với nhân tôm thịt.", DescEn = "Mekong crispy pancake booth with shrimp and pork filling.", Lat = 10.77698d, Lng = 106.70102d, RadiusMeters = 25, Priority = 7, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-07-menu-01.png", OwnerUserId = "owner7" },
                new Booth { Id = "booth-08", ZoneId = "zone-b", NameVi = "Gỏi Cuốn Tươi", NameEn = "Fresh Spring Rolls", DescVi = "Gian hàng gỏi cuốn tươi thanh mát và tốt cho sức khỏe.", DescEn = "Fresh spring rolls booth with light and healthy flavors.", Lat = 10.77716d, Lng = 106.70102d, RadiusMeters = 25, Priority = 8, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-08-menu-01.png", OwnerUserId = "owner8" },
                new Booth { Id = "booth-09", ZoneId = "zone-b", NameVi = "Hải Sản Nướng", NameEn = "Grilled Seafood", DescVi = "Gian hàng hải sản nướng thơm lừng với nhiều loại tôm mực hấp dẫn.", DescEn = "Grilled seafood booth with fragrant squid and shrimp dishes.", Lat = 10.77734d, Lng = 106.70102d, RadiusMeters = 25, Priority = 9, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-09-menu-01.png", OwnerUserId = "owner9" },
                new Booth { Id = "booth-10", ZoneId = "zone-b", NameVi = "Cà Phê & Trà Sữa", NameEn = "Coffee & Milk Tea", DescVi = "Gian hàng đồ uống với cà phê phin và trà sữa được yêu thích.", DescEn = "Drink booth with Vietnamese coffee and popular milk tea.", Lat = 10.77752d, Lng = 106.70102d, RadiusMeters = 25, Priority = 10, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-10-menu-01.png", OwnerUserId = "owner10" }
            };

            db.Booths.AddRange(booths);
            await db.SaveChangesAsync();
        }

        if (!await db.BoothTranslations.AnyAsync())
        {
            var boothTranslations = new List<BoothTranslationLocal>();
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "vi",
                Name = "Phở Hà Nội",
                Description = "Gian hàng phở Hà Nội với nước dùng đậm đà và hương vị truyền thống.",
                TtsScript = "Gian hàng phở Hà Nội với nước dùng đậm đà và hương vị truyền thống.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "en",
                Name = "Hanoi Pho",
                Description = "Traditional Hanoi pho booth with rich broth and authentic flavor.",
                TtsScript = "Traditional Hanoi pho booth with rich broth and authentic flavor.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "zh",
                Name = "河内牛肉粉",
                Description = "河内牛肉粉展位，提供浓郁汤底与正宗风味。",
                TtsScript = "河内牛肉粉展位，提供浓郁汤底与正宗风味。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "ja",
                Name = "ハノイフォー",
                Description = "濃厚なスープと本場の味を楽しめるハノイフォーのブースです。",
                TtsScript = "濃厚なスープと本場の味を楽しめるハノイフォーのブースです。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "ko",
                Name = "하노이 퍼",
                Description = "진한 육수와 정통 풍미를 맛볼 수 있는 하노이 퍼 부스입니다.",
                TtsScript = "진한 육수와 정통 풍미를 맛볼 수 있는 하노이 퍼 부스입니다.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "fr",
                Name = "Pho de Hanoï",
                Description = "Stand de pho de Hanoï avec un bouillon riche et une saveur authentique.",
                TtsScript = "Stand de pho de Hanoï avec un bouillon riche et une saveur authentique.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "es",
                Name = "Pho de Hanói",
                Description = "Puesto de pho de Hanói con caldo intenso y sabor auténtico.",
                TtsScript = "Puesto de pho de Hanói con caldo intenso y sabor auténtico.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "it",
                Name = "Pho di Hanoi",
                Description = "Stand di pho di Hanoi con brodo ricco e sapore autentico.",
                TtsScript = "Stand di pho di Hanoi con brodo ricco e sapore autentico.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "ru",
                Name = "Ханойский фо",
                Description = "Стенд ханойского фо с насыщенным бульоном и аутентичным вкусом.",
                TtsScript = "Стенд ханойского фо с насыщенным бульоном и аутентичным вкусом.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "vi",
                Name = "Bún Bò Huế",
                Description = "Gian hàng bún bò Huế với vị cay thơm đậm chất miền Trung.",
                TtsScript = "Gian hàng bún bò Huế với vị cay thơm đậm chất miền Trung.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "en",
                Name = "Hue Beef Noodles",
                Description = "Hue beef noodles booth with a spicy and aromatic central Vietnam taste.",
                TtsScript = "Hue beef noodles booth with a spicy and aromatic central Vietnam taste.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "zh",
                Name = "顺化牛肉粉",
                Description = "顺化牛肉粉展位，呈现香辣浓郁的中部风味。",
                TtsScript = "顺化牛肉粉展位，呈现香辣浓郁的中部风味。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "ja",
                Name = "フエ牛肉麺",
                Description = "中部ベトナムらしい香り高く辛味のあるフエ牛肉麺のブースです。",
                TtsScript = "中部ベトナムらしい香り高く辛味のあるフエ牛肉麺のブースです。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "ko",
                Name = "후에 소고기 국수",
                Description = "중부 베트남 특유의 얼큰하고 향긋한 후에 소고기 국수 부스입니다.",
                TtsScript = "중부 베트남 특유의 얼큰하고 향긋한 후에 소고기 국수 부스입니다.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "fr",
                Name = "Bún Bò Huế",
                Description = "Stand de bún bò Huế aux saveurs épicées et parfumées du centre du Vietnam.",
                TtsScript = "Stand de bún bò Huế aux saveurs épicées et parfumées du centre du Vietnam.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "es",
                Name = "Bún Bò Huế",
                Description = "Puesto de bún bò Huế con sabor picante y aromático del centro de Vietnam.",
                TtsScript = "Puesto de bún bò Huế con sabor picante y aromático del centro de Vietnam.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "it",
                Name = "Bún Bò Huế",
                Description = "Stand di bún bò Huế con gusto speziato e aromatico del Vietnam centrale.",
                TtsScript = "Stand di bún bò Huế con gusto speziato e aromatico del Vietnam centrale.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "ru",
                Name = "Бун бо Хюэ",
                Description = "Стенд супа бун бо Хюэ с острым и ароматным вкусом центрального Вьетнама.",
                TtsScript = "Стенд супа бун бо Хюэ с острым и ароматным вкусом центрального Вьетнама.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "vi",
                Name = "Cơm Tấm Sài Gòn",
                Description = "Gian hàng cơm tấm Sài Gòn với sườn nướng và chả truyền thống.",
                TtsScript = "Gian hàng cơm tấm Sài Gòn với sườn nướng và chả truyền thống.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "en",
                Name = "Saigon Broken Rice",
                Description = "Saigon broken rice booth with grilled pork and traditional sides.",
                TtsScript = "Saigon broken rice booth with grilled pork and traditional sides.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "zh",
                Name = "西贡碎米饭",
                Description = "西贡碎米饭展位，搭配烤排骨与传统配菜。",
                TtsScript = "西贡碎米饭展位，搭配烤排骨与传统配菜。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "ja",
                Name = "サイゴンのコムタム",
                Description = "焼き豚と伝統的なおかずを添えたサイゴン風コムタムのブースです。",
                TtsScript = "焼き豚と伝統的なおかずを添えたサイゴン風コムタムのブースです。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "ko",
                Name = "사이공 껌땀",
                Description = "구운 돼지고기와 전통 반찬이 함께 제공되는 사이공 껌땀 부스입니다.",
                TtsScript = "구운 돼지고기와 전통 반찬이 함께 제공되는 사이공 껌땀 부스입니다.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "fr",
                Name = "Riz brisé de Saïgon",
                Description = "Stand de riz brisé de Saïgon avec porc grillé et accompagnements traditionnels.",
                TtsScript = "Stand de riz brisé de Saïgon avec porc grillé et accompagnements traditionnels.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "es",
                Name = "Arroz quebrado de Saigón",
                Description = "Puesto de arroz quebrado de Saigón con cerdo a la parrilla y guarniciones tradicionales.",
                TtsScript = "Puesto de arroz quebrado de Saigón con cerdo a la parrilla y guarniciones tradicionales.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "it",
                Name = "Riso spezzato di Saigon",
                Description = "Stand di riso spezzato di Saigon con maiale alla griglia e contorni tradizionali.",
                TtsScript = "Stand di riso spezzato di Saigon con maiale alla griglia e contorni tradizionali.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "ru",
                Name = "Сайгонский дроблёный рис",
                Description = "Стенд сайгонского дроблёного риса с жареной свининой и традиционными гарнирами.",
                TtsScript = "Стенд сайгонского дроблёного риса с жареной свининой и традиционными гарнирами.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "vi",
                Name = "Bánh Mì Việt",
                Description = "Gian hàng bánh mì Việt giòn thơm với nhiều loại nhân hấp dẫn.",
                TtsScript = "Gian hàng bánh mì Việt giòn thơm với nhiều loại nhân hấp dẫn.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "en",
                Name = "Vietnamese Banh Mi",
                Description = "Vietnamese banh mi booth with crispy bread and delicious fillings.",
                TtsScript = "Vietnamese banh mi booth with crispy bread and delicious fillings.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "zh",
                Name = "越式法棍",
                Description = "越式法棍展位，外脆内香，搭配多种美味馅料。",
                TtsScript = "越式法棍展位，外脆内香，搭配多种美味馅料。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "ja",
                Name = "ベトナムバインミー",
                Description = "香ばしいパンと多彩な具材を楽しめるベトナムバインミーのブースです。",
                TtsScript = "香ばしいパンと多彩な具材を楽しめるベトナムバインミーのブースです。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "ko",
                Name = "베트남 반미",
                Description = "바삭한 빵과 다양한 속재료가 어우러진 베트남 반미 부스입니다.",
                TtsScript = "바삭한 빵과 다양한 속재료가 어우러진 베트남 반미 부스입니다.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "fr",
                Name = "Bánh Mì vietnamien",
                Description = "Stand de bánh mì vietnamien avec pain croustillant et garnitures savoureuses.",
                TtsScript = "Stand de bánh mì vietnamien avec pain croustillant et garnitures savoureuses.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "es",
                Name = "Bánh Mì vietnamita",
                Description = "Puesto de bánh mì vietnamita con pan crujiente y rellenos sabrosos.",
                TtsScript = "Puesto de bánh mì vietnamita con pan crujiente y rellenos sabrosos.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "it",
                Name = "Bánh Mì vietnamita",
                Description = "Stand di bánh mì vietnamita con pane croccante e gustosi ripieni.",
                TtsScript = "Stand di bánh mì vietnamita con pane croccante e gustosi ripieni.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "ru",
                Name = "Вьетнамский баньми",
                Description = "Стенд вьетнамского баньми с хрустящим хлебом и вкусными начинками.",
                TtsScript = "Стенд вьетнамского баньми с хрустящим хлебом и вкусными начинками.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "vi",
                Name = "Chè Ba Miền",
                Description = "Gian hàng chè ba miền với nhiều món tráng miệng ngọt mát.",
                TtsScript = "Gian hàng chè ba miền với nhiều món tráng miệng ngọt mát.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "en",
                Name = "Three-Region Sweet Soup",
                Description = "Three-region sweet soup booth with refreshing desserts.",
                TtsScript = "Three-region sweet soup booth with refreshing desserts.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "zh",
                Name = "三地甜品",
                Description = "三地甜品展位，提供清凉香甜的越南传统甜点。",
                TtsScript = "三地甜品展位，提供清凉香甜的越南传统甜点。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "ja",
                Name = "三地域チェー",
                Description = "さっぱり甘いベトナムデザートを楽しめる三地域チェーのブースです。",
                TtsScript = "さっぱり甘いベトナムデザートを楽しめる三地域チェーのブースです。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "ko",
                Name = "삼지역 체",
                Description = "시원하고 달콤한 베트남 전통 디저트를 즐길 수 있는 체 부스입니다.",
                TtsScript = "시원하고 달콤한 베트남 전통 디저트를 즐길 수 있는 체 부스입니다.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "fr",
                Name = "Soupes sucrées des trois régions",
                Description = "Stand de desserts vietnamiens rafraîchissants des trois régions.",
                TtsScript = "Stand de desserts vietnamiens rafraîchissants des trois régions.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "es",
                Name = "Postres dulces de las tres regiones",
                Description = "Puesto de postres vietnamitas refrescantes de las tres regiones.",
                TtsScript = "Puesto de postres vietnamitas refrescantes de las tres regiones.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "it",
                Name = "Dolci delle tre regioni",
                Description = "Stand di dessert vietnamiti freschi provenienti dalle tre regioni.",
                TtsScript = "Stand di dessert vietnamiti freschi provenienti dalle tre regioni.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "ru",
                Name = "Сладкие десерты трёх регионов",
                Description = "Стенд освежающих вьетнамских десертов из трёх регионов.",
                TtsScript = "Стенд освежающих вьетнамских десертов из трёх регионов.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "vi",
                Name = "Nem Nướng Đà Lạt",
                Description = "Gian hàng nem nướng Đà Lạt với hương vị thơm ngon đặc trưng.",
                TtsScript = "Gian hàng nem nướng Đà Lạt với hương vị thơm ngon đặc trưng.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "en",
                Name = "Dalat Grilled Pork Rolls",
                Description = "Dalat grilled pork rolls booth with authentic local flavor.",
                TtsScript = "Dalat grilled pork rolls booth with authentic local flavor.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "zh",
                Name = "大叻烤肉卷",
                Description = "大叻烤肉卷展位，带来当地特色风味。",
                TtsScript = "大叻烤肉卷展位，带来当地特色风味。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "ja",
                Name = "ダラット焼き豚ロール",
                Description = "ダラット名物の焼き豚ロールを楽しめるご当地ブースです。",
                TtsScript = "ダラット名物の焼き豚ロールを楽しめるご当地ブースです。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "ko",
                Name = "달랏 넴느엉",
                Description = "달랏의 대표적인 맛을 느낄 수 있는 넴느엉 부스입니다.",
                TtsScript = "달랏의 대표적인 맛을 느낄 수 있는 넴느엉 부스입니다.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "fr",
                Name = "Rouleaux de porc grillé de Dalat",
                Description = "Stand de rouleaux de porc grillé de Dalat aux saveurs locales authentiques.",
                TtsScript = "Stand de rouleaux de porc grillé de Dalat aux saveurs locales authentiques.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "es",
                Name = "Rollos de cerdo a la parrilla de Dalat",
                Description = "Puesto de rollos de cerdo a la parrilla de Dalat con sabor local auténtico.",
                TtsScript = "Puesto de rollos de cerdo a la parrilla de Dalat con sabor local auténtico.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "it",
                Name = "Involtini di maiale grigliato di Dalat",
                Description = "Stand di involtini di maiale grigliato di Dalat dal gusto locale autentico.",
                TtsScript = "Stand di involtini di maiale grigliato di Dalat dal gusto locale autentico.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "ru",
                Name = "Далатские роллы из жареной свинины",
                Description = "Стенд далатских роллов из жареной свинины с аутентичным местным вкусом.",
                TtsScript = "Стенд далатских роллов из жареной свинины с аутентичным местным вкусом.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "vi",
                Name = "Bánh Xèo Miền Tây",
                Description = "Gian hàng bánh xèo miền Tây giòn rụm với nhân tôm thịt.",
                TtsScript = "Gian hàng bánh xèo miền Tây giòn rụm với nhân tôm thịt.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "en",
                Name = "Mekong Crispy Pancake",
                Description = "Mekong crispy pancake booth with shrimp and pork filling.",
                TtsScript = "Mekong crispy pancake booth with shrimp and pork filling.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "zh",
                Name = "湄公河煎饼",
                Description = "湄公河煎饼展位，酥脆可口，内馅为虾和猪肉。",
                TtsScript = "湄公河煎饼展位，酥脆可口，内馅为虾和猪肉。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "ja",
                Name = "メコン風バインセオ",
                Description = "海老と豚肉を包んだカリカリのメコン風バインセオのブースです。",
                TtsScript = "海老と豚肉を包んだカリカリのメコン風バインセオのブースです。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "ko",
                Name = "메콩식 반쎄오",
                Description = "새우와 돼지고기가 들어간 바삭한 메콩식 반쎄오 부스입니다.",
                TtsScript = "새우와 돼지고기가 들어간 바삭한 메콩식 반쎄오 부스입니다.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "fr",
                Name = "Crêpe croustillante du Mékong",
                Description = "Stand de crêpe croustillante du Mékong garnie de crevettes et de porc.",
                TtsScript = "Stand de crêpe croustillante du Mékong garnie de crevettes et de porc.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "es",
                Name = "Panqueque crujiente del Mekong",
                Description = "Puesto de panqueque crujiente del Mekong relleno de camarón y cerdo.",
                TtsScript = "Puesto de panqueque crujiente del Mekong relleno de camarón y cerdo.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "it",
                Name = "Pancake croccante del Mekong",
                Description = "Stand di pancake croccante del Mekong con ripieno di gamberi e maiale.",
                TtsScript = "Stand di pancake croccante del Mekong con ripieno di gamberi e maiale.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "ru",
                Name = "Хрустящий блин Меконга",
                Description = "Стенд хрустящих блинов Меконга с начинкой из креветок и свинины.",
                TtsScript = "Стенд хрустящих блинов Меконга с начинкой из креветок и свинины.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "vi",
                Name = "Gỏi Cuốn Tươi",
                Description = "Gian hàng gỏi cuốn tươi thanh mát và tốt cho sức khỏe.",
                TtsScript = "Gian hàng gỏi cuốn tươi thanh mát và tốt cho sức khỏe.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "en",
                Name = "Fresh Spring Rolls",
                Description = "Fresh spring rolls booth with light and healthy flavors.",
                TtsScript = "Fresh spring rolls booth with light and healthy flavors.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "zh",
                Name = "鲜春卷",
                Description = "鲜春卷展位，口感清爽健康。",
                TtsScript = "鲜春卷展位，口感清爽健康。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "ja",
                Name = "生春巻き",
                Description = "さっぱりとしてヘルシーな生春巻きのブースです。",
                TtsScript = "さっぱりとしてヘルシーな生春巻きのブースです。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "ko",
                Name = "생춘권",
                Description = "가볍고 건강한 맛을 즐길 수 있는 생춘권 부스입니다.",
                TtsScript = "가볍고 건강한 맛을 즐길 수 있는 생춘권 부스입니다.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "fr",
                Name = "Rouleaux de printemps frais",
                Description = "Stand de rouleaux de printemps frais aux saveurs légères et saines.",
                TtsScript = "Stand de rouleaux de printemps frais aux saveurs légères et saines.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "es",
                Name = "Rollitos frescos",
                Description = "Puesto de rollitos frescos con sabores ligeros y saludables.",
                TtsScript = "Puesto de rollitos frescos con sabores ligeros y saludables.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "it",
                Name = "Involtini freschi",
                Description = "Stand di involtini freschi dal gusto leggero e salutare.",
                TtsScript = "Stand di involtini freschi dal gusto leggero e salutare.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "ru",
                Name = "Свежие спринг-роллы",
                Description = "Стенд свежих спринг-роллов с лёгким и полезным вкусом.",
                TtsScript = "Стенд свежих спринг-роллов с лёгким и полезным вкусом.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "vi",
                Name = "Hải Sản Nướng",
                Description = "Gian hàng hải sản nướng thơm lừng với nhiều loại tôm mực hấp dẫn.",
                TtsScript = "Gian hàng hải sản nướng thơm lừng với nhiều loại tôm mực hấp dẫn.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "en",
                Name = "Grilled Seafood",
                Description = "Grilled seafood booth with fragrant squid and shrimp dishes.",
                TtsScript = "Grilled seafood booth with fragrant squid and shrimp dishes.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "zh",
                Name = "烤海鲜",
                Description = "烤海鲜展位，提供香气十足的鱿鱼和虾料理。",
                TtsScript = "烤海鲜展位，提供香气十足的鱿鱼和虾料理。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "ja",
                Name = "焼きシーフード",
                Description = "香ばしいイカや海老料理を楽しめる焼きシーフードのブースです。",
                TtsScript = "香ばしいイカや海老料理を楽しめる焼きシーフードのブースです。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "ko",
                Name = "구운 해산물",
                Description = "오징어와 새우를 맛있게 구워낸 해산물 부스입니다.",
                TtsScript = "오징어와 새우를 맛있게 구워낸 해산물 부스입니다.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "fr",
                Name = "Fruits de mer grillés",
                Description = "Stand de fruits de mer grillés avec calamars et crevettes parfumés.",
                TtsScript = "Stand de fruits de mer grillés avec calamars et crevettes parfumés.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "es",
                Name = "Mariscos a la parrilla",
                Description = "Puesto de mariscos a la parrilla con calamar y camarón aromáticos.",
                TtsScript = "Puesto de mariscos a la parrilla con calamar y camarón aromáticos.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "it",
                Name = "Frutti di mare alla griglia",
                Description = "Stand di frutti di mare alla griglia con calamari e gamberi profumati.",
                TtsScript = "Stand di frutti di mare alla griglia con calamari e gamberi profumati.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "ru",
                Name = "Жареные морепродукты",
                Description = "Стенд жареных морепродуктов с ароматными кальмарами и креветками.",
                TtsScript = "Стенд жареных морепродуктов с ароматными кальмарами и креветками.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "vi",
                Name = "Cà Phê & Trà Sữa",
                Description = "Gian hàng đồ uống với cà phê phin và trà sữa được yêu thích.",
                TtsScript = "Gian hàng đồ uống với cà phê phin và trà sữa được yêu thích.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "en",
                Name = "Coffee & Milk Tea",
                Description = "Drink booth with Vietnamese coffee and popular milk tea.",
                TtsScript = "Drink booth with Vietnamese coffee and popular milk tea.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "zh",
                Name = "咖啡与奶茶",
                Description = "饮品展位，提供越南咖啡与人气奶茶。",
                TtsScript = "饮品展位，提供越南咖啡与人气奶茶。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "ja",
                Name = "コーヒー＆ミルクティー",
                Description = "ベトナムコーヒーと人気のミルクティーを楽しめるドリンクブースです。",
                TtsScript = "ベトナムコーヒーと人気のミルクティーを楽しめるドリンクブースです。",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "ko",
                Name = "커피 & 밀크티",
                Description = "베트남 커피와 인기 밀크티를 즐길 수 있는 음료 부스입니다.",
                TtsScript = "베트남 커피와 인기 밀크티를 즐길 수 있는 음료 부스입니다.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "fr",
                Name = "Café et thé au lait",
                Description = "Stand de boissons avec café vietnamien et thé au lait populaire.",
                TtsScript = "Stand de boissons avec café vietnamien et thé au lait populaire.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "es",
                Name = "Café y té con leche",
                Description = "Puesto de bebidas con café vietnamita y popular té con leche.",
                TtsScript = "Puesto de bebidas con café vietnamita y popular té con leche.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "it",
                Name = "Caffè e tè al latte",
                Description = "Stand di bevande con caffè vietnamita e popolare tè al latte.",
                TtsScript = "Stand di bevande con caffè vietnamita e popolare tè al latte.",
                AudioUrl = null
            });
            boothTranslations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "ru",
                Name = "Кофе и молочный чай",
                Description = "Стенд напитков с вьетнамским кофе и популярным молочным чаем.",
                TtsScript = "Стенд напитков с вьетнамским кофе и популярным молочным чаем.",
                AudioUrl = null
            });

            db.BoothTranslations.AddRange(boothTranslations);
            await db.SaveChangesAsync();
        }

        if (!await db.BoothMenuItems.AnyAsync())
        {
            var menuItems = new List<BoothMenuItem>
            {
                new BoothMenuItem { Id = "booth-01-menu-01", BoothId = "booth-01", Name = "Phở Đặc Biệt", NameEn = "Special Pho", Description = "Phở bò truyền thống với đầy đủ topping.", DescriptionEn = "Traditional beef pho with full toppings.", Price = 65000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-01-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-01-menu-02", BoothId = "booth-01", Name = "Phở Tái Nạm", NameEn = "Rare & Brisket Pho", Description = "Phở tái nạm tươi ngon.", DescriptionEn = "Pho with rare beef and brisket.", Price = 72000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-01-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-01-menu-03", BoothId = "booth-01", Name = "Phở Bò Viên", NameEn = "Meatball Pho", Description = "Phở bò viên dai giòn.", DescriptionEn = "Pho with chewy beef meatballs.", Price = 59000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-01-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-02-menu-01", BoothId = "booth-02", Name = "Bún Bò Đặc Biệt", NameEn = "Special Hue Noodles", Description = "Bún bò Huế đặc biệt.", DescriptionEn = "Special Hue-style beef noodle soup.", Price = 68000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-02-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-02-menu-02", BoothId = "booth-02", Name = "Bún Bò Giò Heo", NameEn = "Hue Noodles with Pork Hock", Description = "Bún bò giò heo đặc trưng.", DescriptionEn = "Hue noodles with tender pork hock.", Price = 75000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-02-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-02-menu-03", BoothId = "booth-02", Name = "Bún Bò Thập Cẩm", NameEn = "Mixed Hue Noodles", Description = "Bún bò thập cẩm đầy đủ.", DescriptionEn = "Mixed Hue noodles with assorted toppings.", Price = 79000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-02-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-03-menu-01", BoothId = "booth-03", Name = "Cơm Tấm Sườn Bì Chả", NameEn = "Broken Rice Combo", Description = "Cơm tấm sườn bì chả truyền thống.", DescriptionEn = "Broken rice with grilled pork, shredded pork skin, and steamed egg meatloaf.", Price = 62000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-03-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-03-menu-02", BoothId = "booth-03", Name = "Cơm Tấm Sườn Nướng", NameEn = "Broken Rice Grilled Pork", Description = "Sườn nướng thơm ngon.", DescriptionEn = "Broken rice with fragrant grilled pork chops.", Price = 69000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-03-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-03-menu-03", BoothId = "booth-03", Name = "Cơm Tấm Đặc Biệt", NameEn = "Special Broken Rice", Description = "Cơm tấm đặc biệt.", DescriptionEn = "Special broken rice with premium toppings.", Price = 79000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-03-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-04-menu-01", BoothId = "booth-04", Name = "Bánh Mì Thịt Nướng", NameEn = "Grilled Pork Banh Mi", Description = "Bánh mì kẹp thịt nướng.", DescriptionEn = "Crispy baguette with grilled pork.", Price = 35000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-04-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-04-menu-02", BoothId = "booth-04", Name = "Bánh Mì Gà", NameEn = "Chicken Banh Mi", Description = "Bánh mì gà xé.", DescriptionEn = "Banh mi with shredded chicken.", Price = 38000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-04-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-04-menu-03", BoothId = "booth-04", Name = "Bánh Mì Đặc Biệt", NameEn = "Special Banh Mi", Description = "Bánh mì đặc biệt đầy đủ.", DescriptionEn = "Special banh mi with assorted fillings.", Price = 45000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-04-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-05-menu-01", BoothId = "booth-05", Name = "Chè Đậu Xanh", NameEn = "Mung Bean Sweet Soup", Description = "Chè đậu xanh thanh mát.", DescriptionEn = "Refreshing mung bean sweet soup.", Price = 28000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-05-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-05-menu-02", BoothId = "booth-05", Name = "Chè Thập Cẩm", NameEn = "Mixed Sweet Soup", Description = "Chè thập cẩm đủ loại.", DescriptionEn = "Sweet soup with assorted ingredients.", Price = 32000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-05-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-05-menu-03", BoothId = "booth-05", Name = "Chè Dừa Non", NameEn = "Young Coconut Sweet Soup", Description = "Chè dừa non béo ngậy.", DescriptionEn = "Creamy young coconut sweet soup.", Price = 36000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-05-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-06-menu-01", BoothId = "booth-06", Name = "Nem Nướng Phần", NameEn = "Grilled Pork Rolls Set", Description = "Nem nướng đặc sản Đà Lạt.", DescriptionEn = "Dalat grilled pork rolls set.", Price = 68000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-06-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-06-menu-02", BoothId = "booth-06", Name = "Nem Nướng Combo", NameEn = "Grilled Pork Combo", Description = "Combo nem nướng hấp dẫn.", DescriptionEn = "Combo with grilled pork rolls and sides.", Price = 79000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-06-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-06-menu-03", BoothId = "booth-06", Name = "Nem Nướng Đặc Biệt", NameEn = "Special Grilled Pork Rolls", Description = "Nem nướng đặc biệt.", DescriptionEn = "Special Dalat grilled pork rolls.", Price = 89000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-06-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-07-menu-01", BoothId = "booth-07", Name = "Bánh Xèo Tôm Thịt", NameEn = "Shrimp Pork Pancake", Description = "Bánh xèo nhân tôm thịt.", DescriptionEn = "Crispy pancake with shrimp and pork.", Price = 65000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-07-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-07-menu-02", BoothId = "booth-07", Name = "Bánh Xèo Chay", NameEn = "Vegetarian Pancake", Description = "Bánh xèo nhân đậu xanh.", DescriptionEn = "Vegetarian crispy pancake with mung bean filling.", Price = 58000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-07-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-07-menu-03", BoothId = "booth-07", Name = "Bánh Xèo Đặc Biệt", NameEn = "Special Pancake", Description = "Bánh xèo đặc biệt siêu to.", DescriptionEn = "Extra-large special crispy pancake.", Price = 76000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-07-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-08-menu-01", BoothId = "booth-08", Name = "Gỏi Cuốn Tôm Thịt", NameEn = "Shrimp Pork Spring Rolls", Description = "Gỏi cuốn tôm thịt tươi ngon.", DescriptionEn = "Fresh spring rolls with shrimp and pork.", Price = 42000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-08-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-08-menu-02", BoothId = "booth-08", Name = "Gỏi Cuốn Bò Nướng", NameEn = "Beef Spring Rolls", Description = "Gỏi cuốn nhân bò nướng.", DescriptionEn = "Fresh spring rolls with grilled beef.", Price = 48000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-08-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-08-menu-03", BoothId = "booth-08", Name = "Combo 6 Cuốn", NameEn = "6-roll Combo", Description = "Combo 6 cuốn đầy đủ.", DescriptionEn = "Combo of six assorted spring rolls.", Price = 75000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-08-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-09-menu-01", BoothId = "booth-09", Name = "Mực Nướng Sa Tế", NameEn = "Grilled Squid", Description = "Mực nướng sa tế cay nồng.", DescriptionEn = "Grilled squid with spicy sate sauce.", Price = 98000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-09-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-09-menu-02", BoothId = "booth-09", Name = "Tôm Nướng Muối Ớt", NameEn = "Grilled Shrimp", Description = "Tôm nướng muối ớt đậm đà.", DescriptionEn = "Grilled shrimp with chili salt.", Price = 115000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-09-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-09-menu-03", BoothId = "booth-09", Name = "Combo Hải Sản", NameEn = "Seafood Combo", Description = "Combo hải sản nướng thập cẩm.", DescriptionEn = "Mixed grilled seafood combo.", Price = 149000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-09-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-10-menu-01", BoothId = "booth-10", Name = "Cà Phê Sữa Đá", NameEn = "Iced Milk Coffee", Description = "Cà phê sữa đá truyền thống.", DescriptionEn = "Traditional Vietnamese iced milk coffee.", Price = 30000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-10-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-10-menu-02", BoothId = "booth-10", Name = "Trà Sữa Trân Châu", NameEn = "Bubble Milk Tea", Description = "Trà sữa trân châu đường đen.", DescriptionEn = "Milk tea with black sugar pearls.", Price = 42000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-10-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false },
                new BoothMenuItem { Id = "booth-10-menu-03", BoothId = "booth-10", Name = "Combo Đồ Uống", NameEn = "Drink Combo", Description = "Combo cà phê và trà sữa.", DescriptionEn = "Coffee and milk tea combo.", Price = 70000m, PriceUsd = 0m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-10-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false }
            };

            db.BoothMenuItems.AddRange(menuItems);
            await db.SaveChangesAsync();
        }

        if (!await db.BoothMenuItemTranslations.AnyAsync())
        {
            var itemTranslations = new List<BoothMenuItemTranslationLocal>();
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "vi",
                Name = "Phở Đặc Biệt",
                Description = "Phở bò truyền thống với đầy đủ topping.",
                CurrencyCode = "VND",
                LocalizedPrice = 65000m,
                PriceText = "65.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "en",
                Name = "Special Pho",
                Description = "Traditional beef pho with full toppings.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(65000m / 25000m, 2),
                PriceText = $"${Math.Round(65000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "zh",
                Name = "特制牛肉粉",
                Description = "配有完整配料的传统牛肉河粉。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(65000m / 3500m, 2),
                PriceText = $"¥{Math.Round(65000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "ja",
                Name = "特製フォー",
                Description = "具材たっぷりの伝統的な牛肉フォー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(65000m / 170m, 0),
                PriceText = $"¥{Math.Round(65000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "ko",
                Name = "스페셜 쌀국수",
                Description = "다양한 토핑이 들어간 전통 소고기 쌀국수.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(65000m / 18m, 0),
                PriceText = $"₩{Math.Round(65000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "fr",
                Name = "Pho spécial",
                Description = "Pho traditionnel au bœuf avec garnitures complètes.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(65000m / 27000m, 2),
                PriceText = $"€{Math.Round(65000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "es",
                Name = "Pho especial",
                Description = "Pho tradicional de ternera con todos los acompañamientos.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(65000m / 27000m, 2),
                PriceText = $"€{Math.Round(65000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "it",
                Name = "Pho speciale",
                Description = "Pho tradizionale di manzo con condimenti completi.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(65000m / 27000m, 2),
                PriceText = $"€{Math.Round(65000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "ru",
                Name = "Фо спешл",
                Description = "Традиционный фо с говядиной и полным набором топпингов.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(65000m / 300m, 2),
                PriceText = $"₽{Math.Round(65000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-02",
                LanguageCode = "vi",
                Name = "Phở Tái Nạm",
                Description = "Phở tái nạm tươi ngon.",
                CurrencyCode = "VND",
                LocalizedPrice = 72000m,
                PriceText = "72.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-02",
                LanguageCode = "en",
                Name = "Rare & Brisket Pho",
                Description = "Pho with rare beef and brisket.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(72000m / 25000m, 2),
                PriceText = $"${Math.Round(72000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-02",
                LanguageCode = "zh",
                Name = "生熟牛肉粉",
                Description = "配有生牛肉和牛腩的河粉。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(72000m / 3500m, 2),
                PriceText = $"¥{Math.Round(72000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-02",
                LanguageCode = "ja",
                Name = "レア＆ブリスケットフォー",
                Description = "レア牛肉とブリスケット入りのフォー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(72000m / 170m, 0),
                PriceText = $"¥{Math.Round(72000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-02",
                LanguageCode = "ko",
                Name = "타이남 쌀국수",
                Description = "얇은 소고기와 양지머리가 들어간 쌀국수.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(72000m / 18m, 0),
                PriceText = $"₩{Math.Round(72000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-02",
                LanguageCode = "fr",
                Name = "Pho bœuf saignant et poitrine",
                Description = "Pho avec bœuf saignant et poitrine.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(72000m / 27000m, 2),
                PriceText = $"€{Math.Round(72000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-02",
                LanguageCode = "es",
                Name = "Pho con carne poco hecha y pecho",
                Description = "Pho con ternera poco hecha y pecho.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(72000m / 27000m, 2),
                PriceText = $"€{Math.Round(72000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-02",
                LanguageCode = "it",
                Name = "Pho con manzo al sangue e punta di petto",
                Description = "Pho con manzo al sangue e punta di petto.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(72000m / 27000m, 2),
                PriceText = $"€{Math.Round(72000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-02",
                LanguageCode = "ru",
                Name = "Фо с сырым мясом и грудинкой",
                Description = "Фо с тонкими ломтиками говядины и грудинкой.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(72000m / 300m, 2),
                PriceText = $"₽{Math.Round(72000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-03",
                LanguageCode = "vi",
                Name = "Phở Bò Viên",
                Description = "Phở bò viên dai giòn.",
                CurrencyCode = "VND",
                LocalizedPrice = 59000m,
                PriceText = "59.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-03",
                LanguageCode = "en",
                Name = "Meatball Pho",
                Description = "Pho with chewy beef meatballs.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(59000m / 25000m, 2),
                PriceText = $"${Math.Round(59000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-03",
                LanguageCode = "zh",
                Name = "牛肉丸河粉",
                Description = "配有弹牙牛肉丸的河粉。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(59000m / 3500m, 2),
                PriceText = $"¥{Math.Round(59000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-03",
                LanguageCode = "ja",
                Name = "ミートボールフォー",
                Description = "弾力のある牛肉団子入りフォー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(59000m / 170m, 0),
                PriceText = $"¥{Math.Round(59000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-03",
                LanguageCode = "ko",
                Name = "소고기 완자 쌀국수",
                Description = "쫄깃한 소고기 완자가 들어간 쌀국수.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(59000m / 18m, 0),
                PriceText = $"₩{Math.Round(59000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-03",
                LanguageCode = "fr",
                Name = "Pho aux boulettes de bœuf",
                Description = "Pho avec boulettes de bœuf moelleuses.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(59000m / 27000m, 2),
                PriceText = $"€{Math.Round(59000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-03",
                LanguageCode = "es",
                Name = "Pho con albóndigas de ternera",
                Description = "Pho con albóndigas de ternera suaves.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(59000m / 27000m, 2),
                PriceText = $"€{Math.Round(59000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-03",
                LanguageCode = "it",
                Name = "Pho con polpette di manzo",
                Description = "Pho con morbide polpette di manzo.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(59000m / 27000m, 2),
                PriceText = $"€{Math.Round(59000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-03",
                LanguageCode = "ru",
                Name = "Фо с фрикадельками",
                Description = "Фо с упругими говяжьими фрикадельками.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(59000m / 300m, 2),
                PriceText = $"₽{Math.Round(59000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "vi",
                Name = "Bún Bò Đặc Biệt",
                Description = "Bún bò Huế đặc biệt.",
                CurrencyCode = "VND",
                LocalizedPrice = 68000m,
                PriceText = "68.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "en",
                Name = "Special Hue Noodles",
                Description = "Special Hue-style beef noodle soup.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(68000m / 25000m, 2),
                PriceText = $"${Math.Round(68000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "zh",
                Name = "特制顺化牛肉粉",
                Description = "特制顺化风味牛肉米粉。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(68000m / 3500m, 2),
                PriceText = $"¥{Math.Round(68000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "ja",
                Name = "特製フエ牛肉麺",
                Description = "特製フエ風牛肉麺。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(68000m / 170m, 0),
                PriceText = $"¥{Math.Round(68000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "ko",
                Name = "후에식 스페셜 소고기 국수",
                Description = "특별한 후에식 소고기 국수.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(68000m / 18m, 0),
                PriceText = $"₩{Math.Round(68000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "fr",
                Name = "Bún bò Huế spécial",
                Description = "Soupe spéciale de nouilles au bœuf de Hué.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(68000m / 27000m, 2),
                PriceText = $"€{Math.Round(68000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "es",
                Name = "Bún bò Huế especial",
                Description = "Sopa especial de fideos con ternera al estilo de Hué.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(68000m / 27000m, 2),
                PriceText = $"€{Math.Round(68000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "it",
                Name = "Bún bò Huế speciale",
                Description = "Zuppa speciale di noodle con manzo in stile Huế.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(68000m / 27000m, 2),
                PriceText = $"€{Math.Round(68000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "ru",
                Name = "Бун бо Хюэ спешл",
                Description = "Особый суп с говядиной в стиле Хюэ.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(68000m / 300m, 2),
                PriceText = $"₽{Math.Round(68000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-02",
                LanguageCode = "vi",
                Name = "Bún Bò Giò Heo",
                Description = "Bún bò giò heo đặc trưng.",
                CurrencyCode = "VND",
                LocalizedPrice = 75000m,
                PriceText = "75.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-02",
                LanguageCode = "en",
                Name = "Hue Noodles with Pork Hock",
                Description = "Hue noodles with tender pork hock.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(75000m / 25000m, 2),
                PriceText = $"${Math.Round(75000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-02",
                LanguageCode = "zh",
                Name = "猪蹄顺化牛肉粉",
                Description = "顺化风味猪蹄牛肉粉。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(75000m / 3500m, 2),
                PriceText = $"¥{Math.Round(75000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-02",
                LanguageCode = "ja",
                Name = "豚足入りフエ牛肉麺",
                Description = "豚足入りのフエ風牛肉麺。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(75000m / 170m, 0),
                PriceText = $"¥{Math.Round(75000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-02",
                LanguageCode = "ko",
                Name = "족발 후에 국수",
                Description = "부드러운 족발이 들어간 후에식 국수.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(75000m / 18m, 0),
                PriceText = $"₩{Math.Round(75000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-02",
                LanguageCode = "fr",
                Name = "Bún bò Huế au jarret",
                Description = "Bún bò Huế avec jarret de porc tendre.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(75000m / 27000m, 2),
                PriceText = $"€{Math.Round(75000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-02",
                LanguageCode = "es",
                Name = "Bún bò Huế con codillo",
                Description = "Bún bò Huế con codillo de cerdo tierno.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(75000m / 27000m, 2),
                PriceText = $"€{Math.Round(75000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-02",
                LanguageCode = "it",
                Name = "Bún bò Huế con stinco",
                Description = "Bún bò Huế con stinco di maiale tenero.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(75000m / 27000m, 2),
                PriceText = $"€{Math.Round(75000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-02",
                LanguageCode = "ru",
                Name = "Бун бо Хюэ с рулькой",
                Description = "Бун бо Хюэ с нежной свиной рулькой.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(75000m / 300m, 2),
                PriceText = $"₽{Math.Round(75000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-03",
                LanguageCode = "vi",
                Name = "Bún Bò Thập Cẩm",
                Description = "Bún bò thập cẩm đầy đủ.",
                CurrencyCode = "VND",
                LocalizedPrice = 79000m,
                PriceText = "79.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-03",
                LanguageCode = "en",
                Name = "Mixed Hue Noodles",
                Description = "Mixed Hue noodles with assorted toppings.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(79000m / 25000m, 2),
                PriceText = $"${Math.Round(79000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-03",
                LanguageCode = "zh",
                Name = "什锦顺化牛肉粉",
                Description = "配料丰富的顺化什锦牛肉粉。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(79000m / 3500m, 2),
                PriceText = $"¥{Math.Round(79000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-03",
                LanguageCode = "ja",
                Name = "ミックスフエ牛肉麺",
                Description = "具材たっぷりのフエ風ミックス牛肉麺。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(79000m / 170m, 0),
                PriceText = $"¥{Math.Round(79000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-03",
                LanguageCode = "ko",
                Name = "모둠 후에 국수",
                Description = "다양한 토핑이 올라간 후에식 모둠 국수.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(79000m / 18m, 0),
                PriceText = $"₩{Math.Round(79000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-03",
                LanguageCode = "fr",
                Name = "Bún bò Huế assorti",
                Description = "Bún bò Huế assorti avec plusieurs garnitures.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(79000m / 27000m, 2),
                PriceText = $"€{Math.Round(79000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-03",
                LanguageCode = "es",
                Name = "Bún bò Huế mixto",
                Description = "Bún bò Huế mixto con varios acompañamientos.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(79000m / 27000m, 2),
                PriceText = $"€{Math.Round(79000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-03",
                LanguageCode = "it",
                Name = "Bún bò Huế misto",
                Description = "Bún bò Huế misto con vari condimenti.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(79000m / 27000m, 2),
                PriceText = $"€{Math.Round(79000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-03",
                LanguageCode = "ru",
                Name = "Бун бо Хюэ ассорти",
                Description = "Бун бо Хюэ с разнообразными добавками.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(79000m / 300m, 2),
                PriceText = $"₽{Math.Round(79000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "vi",
                Name = "Cơm Tấm Sườn Bì Chả",
                Description = "Cơm tấm sườn bì chả truyền thống.",
                CurrencyCode = "VND",
                LocalizedPrice = 62000m,
                PriceText = "62.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "en",
                Name = "Broken Rice Combo",
                Description = "Broken rice with grilled pork, shredded pork skin, and steamed egg meatloaf.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(62000m / 25000m, 2),
                PriceText = $"${Math.Round(62000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "zh",
                Name = "排骨猪皮蒸蛋碎米饭",
                Description = "配烤排骨、猪皮丝和蒸蛋肉饼的碎米饭。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(62000m / 3500m, 2),
                PriceText = $"¥{Math.Round(62000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "ja",
                Name = "コムタムコンボ",
                Description = "焼き豚、豚皮、卵蒸しを添えたコムタム。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(62000m / 170m, 0),
                PriceText = $"¥{Math.Round(62000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "ko",
                Name = "껌땀 콤보",
                Description = "구운 돼지고기, 돼지껍질, 계란찜이 함께 나오는 껌땀.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(62000m / 18m, 0),
                PriceText = $"₩{Math.Round(62000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "fr",
                Name = "Riz brisé combo",
                Description = "Riz brisé avec porc grillé, couenne et pâté aux œufs.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(62000m / 27000m, 2),
                PriceText = $"€{Math.Round(62000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "es",
                Name = "Arroz quebrado combo",
                Description = "Arroz quebrado con cerdo a la parrilla, piel de cerdo y pastel de huevo.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(62000m / 27000m, 2),
                PriceText = $"€{Math.Round(62000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "it",
                Name = "Riso spezzato combo",
                Description = "Riso spezzato con maiale alla griglia, cotenna e polpettone all’uovo.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(62000m / 27000m, 2),
                PriceText = $"€{Math.Round(62000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "ru",
                Name = "Ком там комбо",
                Description = "Дроблёный рис с жареной свининой, свиной кожей и яичным рулетом.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(62000m / 300m, 2),
                PriceText = $"₽{Math.Round(62000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-02",
                LanguageCode = "vi",
                Name = "Cơm Tấm Sườn Nướng",
                Description = "Sườn nướng thơm ngon.",
                CurrencyCode = "VND",
                LocalizedPrice = 69000m,
                PriceText = "69.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-02",
                LanguageCode = "en",
                Name = "Broken Rice Grilled Pork",
                Description = "Broken rice with fragrant grilled pork chops.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(69000m / 25000m, 2),
                PriceText = $"${Math.Round(69000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-02",
                LanguageCode = "zh",
                Name = "烤排骨碎米饭",
                Description = "搭配香喷喷烤排骨的碎米饭。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(69000m / 3500m, 2),
                PriceText = $"¥{Math.Round(69000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-02",
                LanguageCode = "ja",
                Name = "焼き豚コムタム",
                Description = "香ばしい焼き豚をのせたコムタム。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(69000m / 170m, 0),
                PriceText = $"¥{Math.Round(69000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-02",
                LanguageCode = "ko",
                Name = "숯불 돼지고기 껌땀",
                Description = "향긋한 숯불 돼지고기를 곁들인 껌땀.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(69000m / 18m, 0),
                PriceText = $"₩{Math.Round(69000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-02",
                LanguageCode = "fr",
                Name = "Riz brisé aux côtes grillées",
                Description = "Riz brisé avec côtes de porc grillées parfumées.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(69000m / 27000m, 2),
                PriceText = $"€{Math.Round(69000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-02",
                LanguageCode = "es",
                Name = "Arroz quebrado con cerdo a la parrilla",
                Description = "Arroz quebrado con chuletas de cerdo a la parrilla.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(69000m / 27000m, 2),
                PriceText = $"€{Math.Round(69000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-02",
                LanguageCode = "it",
                Name = "Riso spezzato con maiale grigliato",
                Description = "Riso spezzato con costolette di maiale alla griglia.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(69000m / 27000m, 2),
                PriceText = $"€{Math.Round(69000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-02",
                LanguageCode = "ru",
                Name = "Ком там с жареной свининой",
                Description = "Дроблёный рис с ароматной жареной свининой.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(69000m / 300m, 2),
                PriceText = $"₽{Math.Round(69000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-03",
                LanguageCode = "vi",
                Name = "Cơm Tấm Đặc Biệt",
                Description = "Cơm tấm đặc biệt.",
                CurrencyCode = "VND",
                LocalizedPrice = 79000m,
                PriceText = "79.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-03",
                LanguageCode = "en",
                Name = "Special Broken Rice",
                Description = "Special broken rice with premium toppings.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(79000m / 25000m, 2),
                PriceText = $"${Math.Round(79000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-03",
                LanguageCode = "zh",
                Name = "特制碎米饭",
                Description = "搭配高级配料的特制碎米饭。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(79000m / 3500m, 2),
                PriceText = $"¥{Math.Round(79000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-03",
                LanguageCode = "ja",
                Name = "特製コムタム",
                Description = "具材たっぷりの特製コムタム。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(79000m / 170m, 0),
                PriceText = $"¥{Math.Round(79000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-03",
                LanguageCode = "ko",
                Name = "스페셜 껌땀",
                Description = "풍성한 토핑이 올라간 스페셜 껌땀.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(79000m / 18m, 0),
                PriceText = $"₩{Math.Round(79000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-03",
                LanguageCode = "fr",
                Name = "Riz brisé spécial",
                Description = "Riz brisé spécial avec garnitures premium.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(79000m / 27000m, 2),
                PriceText = $"€{Math.Round(79000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-03",
                LanguageCode = "es",
                Name = "Arroz quebrado especial",
                Description = "Arroz quebrado especial con toppings premium.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(79000m / 27000m, 2),
                PriceText = $"€{Math.Round(79000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-03",
                LanguageCode = "it",
                Name = "Riso spezzato speciale",
                Description = "Riso spezzato speciale con condimenti premium.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(79000m / 27000m, 2),
                PriceText = $"€{Math.Round(79000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-03",
                LanguageCode = "ru",
                Name = "Особый ком там",
                Description = "Особый дроблёный рис с премиальными добавками.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(79000m / 300m, 2),
                PriceText = $"₽{Math.Round(79000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "vi",
                Name = "Bánh Mì Thịt Nướng",
                Description = "Bánh mì kẹp thịt nướng.",
                CurrencyCode = "VND",
                LocalizedPrice = 35000m,
                PriceText = "35.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "en",
                Name = "Grilled Pork Banh Mi",
                Description = "Crispy baguette with grilled pork.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(35000m / 25000m, 2),
                PriceText = $"${Math.Round(35000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "zh",
                Name = "烤肉法棍",
                Description = "夹着烤肉的酥脆法棍。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(35000m / 3500m, 2),
                PriceText = $"¥{Math.Round(35000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "ja",
                Name = "焼き豚バインミー",
                Description = "焼き豚を挟んだカリッとしたバインミー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(35000m / 170m, 0),
                PriceText = $"¥{Math.Round(35000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "ko",
                Name = "돼지고기 반미",
                Description = "구운 돼지고기를 넣은 바삭한 반미.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(35000m / 18m, 0),
                PriceText = $"₩{Math.Round(35000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "fr",
                Name = "Bánh mì au porc grillé",
                Description = "Baguette croustillante garnie de porc grillé.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(35000m / 27000m, 2),
                PriceText = $"€{Math.Round(35000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "es",
                Name = "Bánh mì de cerdo a la parrilla",
                Description = "Baguette crujiente con cerdo a la parrilla.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(35000m / 27000m, 2),
                PriceText = $"€{Math.Round(35000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "it",
                Name = "Bánh mì con maiale grigliato",
                Description = "Baguette croccante con maiale alla griglia.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(35000m / 27000m, 2),
                PriceText = $"€{Math.Round(35000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "ru",
                Name = "Баньми с жареной свининой",
                Description = "Хрустящий багет с жареной свининой.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(35000m / 300m, 2),
                PriceText = $"₽{Math.Round(35000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-02",
                LanguageCode = "vi",
                Name = "Bánh Mì Gà",
                Description = "Bánh mì gà xé.",
                CurrencyCode = "VND",
                LocalizedPrice = 38000m,
                PriceText = "38.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-02",
                LanguageCode = "en",
                Name = "Chicken Banh Mi",
                Description = "Banh mi with shredded chicken.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(38000m / 25000m, 2),
                PriceText = $"${Math.Round(38000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-02",
                LanguageCode = "zh",
                Name = "鸡肉法棍",
                Description = "夹有鸡丝的越式法棍。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(38000m / 3500m, 2),
                PriceText = $"¥{Math.Round(38000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-02",
                LanguageCode = "ja",
                Name = "チキンバインミー",
                Description = "ほぐし鶏入りのバインミー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(38000m / 170m, 0),
                PriceText = $"¥{Math.Round(38000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-02",
                LanguageCode = "ko",
                Name = "치킨 반미",
                Description = "닭고기를 넣은 반미.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(38000m / 18m, 0),
                PriceText = $"₩{Math.Round(38000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-02",
                LanguageCode = "fr",
                Name = "Bánh mì au poulet",
                Description = "Bánh mì au poulet effiloché.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(38000m / 27000m, 2),
                PriceText = $"€{Math.Round(38000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-02",
                LanguageCode = "es",
                Name = "Bánh mì de pollo",
                Description = "Bánh mì con pollo desmenuzado.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(38000m / 27000m, 2),
                PriceText = $"€{Math.Round(38000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-02",
                LanguageCode = "it",
                Name = "Bánh mì al pollo",
                Description = "Bánh mì con pollo sfilacciato.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(38000m / 27000m, 2),
                PriceText = $"€{Math.Round(38000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-02",
                LanguageCode = "ru",
                Name = "Баньми с курицей",
                Description = "Баньми с куриным мясом.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(38000m / 300m, 2),
                PriceText = $"₽{Math.Round(38000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-03",
                LanguageCode = "vi",
                Name = "Bánh Mì Đặc Biệt",
                Description = "Bánh mì đặc biệt đầy đủ.",
                CurrencyCode = "VND",
                LocalizedPrice = 45000m,
                PriceText = "45.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-03",
                LanguageCode = "en",
                Name = "Special Banh Mi",
                Description = "Special banh mi with assorted fillings.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(45000m / 25000m, 2),
                PriceText = $"${Math.Round(45000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-03",
                LanguageCode = "zh",
                Name = "特制法棍",
                Description = "夹有多种馅料的特制越式法棍。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(45000m / 3500m, 2),
                PriceText = $"¥{Math.Round(45000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-03",
                LanguageCode = "ja",
                Name = "特製バインミー",
                Description = "具沢山の特製バインミー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(45000m / 170m, 0),
                PriceText = $"¥{Math.Round(45000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-03",
                LanguageCode = "ko",
                Name = "스페셜 반미",
                Description = "다양한 속재료가 들어간 스페셜 반미.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(45000m / 18m, 0),
                PriceText = $"₩{Math.Round(45000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-03",
                LanguageCode = "fr",
                Name = "Bánh mì spécial",
                Description = "Bánh mì spécial avec garnitures assorties.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(45000m / 27000m, 2),
                PriceText = $"€{Math.Round(45000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-03",
                LanguageCode = "es",
                Name = "Bánh mì especial",
                Description = "Bánh mì especial con rellenos variados.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(45000m / 27000m, 2),
                PriceText = $"€{Math.Round(45000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-03",
                LanguageCode = "it",
                Name = "Bánh mì speciale",
                Description = "Bánh mì speciale con ripieni assortiti.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(45000m / 27000m, 2),
                PriceText = $"€{Math.Round(45000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-03",
                LanguageCode = "ru",
                Name = "Специальный баньми",
                Description = "Особый баньми с различными начинками.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(45000m / 300m, 2),
                PriceText = $"₽{Math.Round(45000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "vi",
                Name = "Chè Đậu Xanh",
                Description = "Chè đậu xanh thanh mát.",
                CurrencyCode = "VND",
                LocalizedPrice = 28000m,
                PriceText = "28.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "en",
                Name = "Mung Bean Sweet Soup",
                Description = "Refreshing mung bean sweet soup.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(28000m / 25000m, 2),
                PriceText = $"${Math.Round(28000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "zh",
                Name = "绿豆甜汤",
                Description = "清爽的绿豆甜汤。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(28000m / 3500m, 2),
                PriceText = $"¥{Math.Round(28000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "ja",
                Name = "緑豆チェー",
                Description = "さっぱりした緑豆のチェー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(28000m / 170m, 0),
                PriceText = $"¥{Math.Round(28000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "ko",
                Name = "녹두 체",
                Description = "시원한 녹두 디저트.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(28000m / 18m, 0),
                PriceText = $"₩{Math.Round(28000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "fr",
                Name = "Dessert aux haricots mungo",
                Description = "Dessert rafraîchissant aux haricots mungo.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(28000m / 27000m, 2),
                PriceText = $"€{Math.Round(28000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "es",
                Name = "Postre de frijol mungo",
                Description = "Postre refrescante de frijol mungo.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(28000m / 27000m, 2),
                PriceText = $"€{Math.Round(28000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "it",
                Name = "Dessert di fagioli mung",
                Description = "Dessert rinfrescante ai fagioli mung.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(28000m / 27000m, 2),
                PriceText = $"€{Math.Round(28000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "ru",
                Name = "Сладкий суп из маша",
                Description = "Освежающий сладкий суп из бобов маш.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(28000m / 300m, 2),
                PriceText = $"₽{Math.Round(28000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-02",
                LanguageCode = "vi",
                Name = "Chè Thập Cẩm",
                Description = "Chè thập cẩm đủ loại.",
                CurrencyCode = "VND",
                LocalizedPrice = 32000m,
                PriceText = "32.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-02",
                LanguageCode = "en",
                Name = "Mixed Sweet Soup",
                Description = "Sweet soup with assorted ingredients.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(32000m / 25000m, 2),
                PriceText = $"${Math.Round(32000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-02",
                LanguageCode = "zh",
                Name = "什锦甜汤",
                Description = "含多种配料的甜汤。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(32000m / 3500m, 2),
                PriceText = $"¥{Math.Round(32000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-02",
                LanguageCode = "ja",
                Name = "ミックスチェー",
                Description = "さまざまな具材が入ったチェー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(32000m / 170m, 0),
                PriceText = $"¥{Math.Round(32000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-02",
                LanguageCode = "ko",
                Name = "모둠 체",
                Description = "여러 재료가 들어간 달콤한 디저트.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(32000m / 18m, 0),
                PriceText = $"₩{Math.Round(32000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-02",
                LanguageCode = "fr",
                Name = "Dessert sucré assorti",
                Description = "Soupe sucrée avec ingrédients variés.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(32000m / 27000m, 2),
                PriceText = $"€{Math.Round(32000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-02",
                LanguageCode = "es",
                Name = "Postre dulce mixto",
                Description = "Sopa dulce con ingredientes variados.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(32000m / 27000m, 2),
                PriceText = $"€{Math.Round(32000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-02",
                LanguageCode = "it",
                Name = "Dessert dolce misto",
                Description = "Zuppa dolce con ingredienti assortiti.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(32000m / 27000m, 2),
                PriceText = $"€{Math.Round(32000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-02",
                LanguageCode = "ru",
                Name = "Смешанный сладкий суп",
                Description = "Сладкий десерт с разнообразными ингредиентами.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(32000m / 300m, 2),
                PriceText = $"₽{Math.Round(32000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-03",
                LanguageCode = "vi",
                Name = "Chè Dừa Non",
                Description = "Chè dừa non béo ngậy.",
                CurrencyCode = "VND",
                LocalizedPrice = 36000m,
                PriceText = "36.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-03",
                LanguageCode = "en",
                Name = "Young Coconut Sweet Soup",
                Description = "Creamy young coconut sweet soup.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(36000m / 25000m, 2),
                PriceText = $"${Math.Round(36000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-03",
                LanguageCode = "zh",
                Name = "嫩椰甜汤",
                Description = "香浓顺滑的嫩椰甜汤。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(36000m / 3500m, 2),
                PriceText = $"¥{Math.Round(36000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-03",
                LanguageCode = "ja",
                Name = "若いココナッツチェー",
                Description = "コクのある若いココナッツのチェー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(36000m / 170m, 0),
                PriceText = $"¥{Math.Round(36000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-03",
                LanguageCode = "ko",
                Name = "어린 코코넛 체",
                Description = "고소한 어린 코코넛 디저트.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(36000m / 18m, 0),
                PriceText = $"₩{Math.Round(36000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-03",
                LanguageCode = "fr",
                Name = "Dessert à la jeune noix de coco",
                Description = "Dessert crémeux à la jeune noix de coco.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(36000m / 27000m, 2),
                PriceText = $"€{Math.Round(36000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-03",
                LanguageCode = "es",
                Name = "Postre de coco joven",
                Description = "Postre cremoso de coco joven.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(36000m / 27000m, 2),
                PriceText = $"€{Math.Round(36000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-03",
                LanguageCode = "it",
                Name = "Dessert al cocco giovane",
                Description = "Dessert cremoso al cocco giovane.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(36000m / 27000m, 2),
                PriceText = $"€{Math.Round(36000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-03",
                LanguageCode = "ru",
                Name = "Сладкий суп с молодым кокосом",
                Description = "Нежный сладкий десерт с молодым кокосом.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(36000m / 300m, 2),
                PriceText = $"₽{Math.Round(36000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "vi",
                Name = "Nem Nướng Phần",
                Description = "Nem nướng đặc sản Đà Lạt.",
                CurrencyCode = "VND",
                LocalizedPrice = 68000m,
                PriceText = "68.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "en",
                Name = "Grilled Pork Rolls Set",
                Description = "Dalat grilled pork rolls set.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(68000m / 25000m, 2),
                PriceText = $"${Math.Round(68000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "zh",
                Name = "大叻烤肉卷套餐",
                Description = "大叻风味烤肉卷套餐。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(68000m / 3500m, 2),
                PriceText = $"¥{Math.Round(68000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "ja",
                Name = "ダラット焼き豚ロールセット",
                Description = "ダラット名物焼き豚ロールのセット。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(68000m / 170m, 0),
                PriceText = $"¥{Math.Round(68000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "ko",
                Name = "달랏 넴느엉 세트",
                Description = "달랏식 넴느엉 세트 메뉴.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(68000m / 18m, 0),
                PriceText = $"₩{Math.Round(68000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "fr",
                Name = "Set de rouleaux de porc grillé",
                Description = "Set de rouleaux de porc grillé de Dalat.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(68000m / 27000m, 2),
                PriceText = $"€{Math.Round(68000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "es",
                Name = "Set de rollos de cerdo a la parrilla",
                Description = "Set de rollos de cerdo a la parrilla de Dalat.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(68000m / 27000m, 2),
                PriceText = $"€{Math.Round(68000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "it",
                Name = "Set di involtini di maiale grigliato",
                Description = "Set di involtini di maiale grigliato di Dalat.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(68000m / 27000m, 2),
                PriceText = $"€{Math.Round(68000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "ru",
                Name = "Сет роллов из жареной свинины",
                Description = "Сет далатских роллов из жареной свинины.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(68000m / 300m, 2),
                PriceText = $"₽{Math.Round(68000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-02",
                LanguageCode = "vi",
                Name = "Nem Nướng Combo",
                Description = "Combo nem nướng hấp dẫn.",
                CurrencyCode = "VND",
                LocalizedPrice = 79000m,
                PriceText = "79.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-02",
                LanguageCode = "en",
                Name = "Grilled Pork Combo",
                Description = "Combo with grilled pork rolls and sides.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(79000m / 25000m, 2),
                PriceText = $"${Math.Round(79000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-02",
                LanguageCode = "zh",
                Name = "烤肉卷组合",
                Description = "烤肉卷搭配配菜的组合。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(79000m / 3500m, 2),
                PriceText = $"¥{Math.Round(79000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-02",
                LanguageCode = "ja",
                Name = "焼き豚ロールコンボ",
                Description = "焼き豚ロールと付け合わせのコンボ。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(79000m / 170m, 0),
                PriceText = $"¥{Math.Round(79000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-02",
                LanguageCode = "ko",
                Name = "넴느엉 콤보",
                Description = "넴느엉과 곁들임 메뉴가 함께 나오는 콤보.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(79000m / 18m, 0),
                PriceText = $"₩{Math.Round(79000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-02",
                LanguageCode = "fr",
                Name = "Combo de porc grillé",
                Description = "Combo de rouleaux de porc grillé avec accompagnements.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(79000m / 27000m, 2),
                PriceText = $"€{Math.Round(79000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-02",
                LanguageCode = "es",
                Name = "Combo de cerdo a la parrilla",
                Description = "Combo de rollos de cerdo a la parrilla con acompañamientos.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(79000m / 27000m, 2),
                PriceText = $"€{Math.Round(79000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-02",
                LanguageCode = "it",
                Name = "Combo di maiale grigliato",
                Description = "Combo di involtini di maiale grigliato con contorni.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(79000m / 27000m, 2),
                PriceText = $"€{Math.Round(79000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-02",
                LanguageCode = "ru",
                Name = "Комбо из жареной свинины",
                Description = "Комбо из роллов из жареной свинины с гарнирами.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(79000m / 300m, 2),
                PriceText = $"₽{Math.Round(79000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-03",
                LanguageCode = "vi",
                Name = "Nem Nướng Đặc Biệt",
                Description = "Nem nướng đặc biệt.",
                CurrencyCode = "VND",
                LocalizedPrice = 89000m,
                PriceText = "89.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-03",
                LanguageCode = "en",
                Name = "Special Grilled Pork Rolls",
                Description = "Special Dalat grilled pork rolls.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(89000m / 25000m, 2),
                PriceText = $"${Math.Round(89000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-03",
                LanguageCode = "zh",
                Name = "特制烤肉卷",
                Description = "特制大叻风味烤肉卷。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(89000m / 3500m, 2),
                PriceText = $"¥{Math.Round(89000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-03",
                LanguageCode = "ja",
                Name = "特製焼き豚ロール",
                Description = "特製ダラット風焼き豚ロール。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(89000m / 170m, 0),
                PriceText = $"¥{Math.Round(89000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-03",
                LanguageCode = "ko",
                Name = "스페셜 넴느엉",
                Description = "특제 달랏식 넴느엉.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(89000m / 18m, 0),
                PriceText = $"₩{Math.Round(89000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-03",
                LanguageCode = "fr",
                Name = "Rouleaux de porc grillé spéciaux",
                Description = "Rouleaux de porc grillé spéciaux de Dalat.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(89000m / 27000m, 2),
                PriceText = $"€{Math.Round(89000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-03",
                LanguageCode = "es",
                Name = "Rollos de cerdo a la parrilla especiales",
                Description = "Rollos especiales de cerdo a la parrilla de Dalat.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(89000m / 27000m, 2),
                PriceText = $"€{Math.Round(89000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-03",
                LanguageCode = "it",
                Name = "Involtini di maiale grigliato speciali",
                Description = "Involtini speciali di maiale grigliato di Dalat.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(89000m / 27000m, 2),
                PriceText = $"€{Math.Round(89000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-03",
                LanguageCode = "ru",
                Name = "Особые роллы из жареной свинины",
                Description = "Особые далатские роллы из жареной свинины.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(89000m / 300m, 2),
                PriceText = $"₽{Math.Round(89000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "vi",
                Name = "Bánh Xèo Tôm Thịt",
                Description = "Bánh xèo nhân tôm thịt.",
                CurrencyCode = "VND",
                LocalizedPrice = 65000m,
                PriceText = "65.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "en",
                Name = "Shrimp Pork Pancake",
                Description = "Crispy pancake with shrimp and pork.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(65000m / 25000m, 2),
                PriceText = $"${Math.Round(65000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "zh",
                Name = "虾肉煎饼",
                Description = "酥脆的越南煎饼，内馅是虾和猪肉。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(65000m / 3500m, 2),
                PriceText = $"¥{Math.Round(65000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "ja",
                Name = "海老と豚肉のバインセオ",
                Description = "海老と豚肉入りのカリカリのバインセオ。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(65000m / 170m, 0),
                PriceText = $"¥{Math.Round(65000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "ko",
                Name = "새우 돼지고기 반쎄오",
                Description = "새우와 돼지고기 속이 들어간 바삭한 반쎄오.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(65000m / 18m, 0),
                PriceText = $"₩{Math.Round(65000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "fr",
                Name = "Crêpe crevettes et porc",
                Description = "Crêpe croustillante aux crevettes et au porc.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(65000m / 27000m, 2),
                PriceText = $"€{Math.Round(65000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "es",
                Name = "Panqueque de camarón y cerdo",
                Description = "Panqueque crujiente con camarón y cerdo.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(65000m / 27000m, 2),
                PriceText = $"€{Math.Round(65000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "it",
                Name = "Pancake con gamberi e maiale",
                Description = "Pancake croccante con gamberi e maiale.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(65000m / 27000m, 2),
                PriceText = $"€{Math.Round(65000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "ru",
                Name = "Бань сео с креветками и свининой",
                Description = "Хрустящий вьетнамский блин с креветками и свининой.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(65000m / 300m, 2),
                PriceText = $"₽{Math.Round(65000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-02",
                LanguageCode = "vi",
                Name = "Bánh Xèo Chay",
                Description = "Bánh xèo nhân đậu xanh.",
                CurrencyCode = "VND",
                LocalizedPrice = 58000m,
                PriceText = "58.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-02",
                LanguageCode = "en",
                Name = "Vegetarian Pancake",
                Description = "Vegetarian crispy pancake with mung bean filling.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(58000m / 25000m, 2),
                PriceText = $"${Math.Round(58000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-02",
                LanguageCode = "zh",
                Name = "素食煎饼",
                Description = "以绿豆为馅的素食越南煎饼。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(58000m / 3500m, 2),
                PriceText = $"¥{Math.Round(58000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-02",
                LanguageCode = "ja",
                Name = "ベジタリアンバインセオ",
                Description = "緑豆入りのベジタリアンバインセオ。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(58000m / 170m, 0),
                PriceText = $"¥{Math.Round(58000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-02",
                LanguageCode = "ko",
                Name = "채식 반쎄오",
                Description = "녹두 속이 들어간 채식 반쎄오.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(58000m / 18m, 0),
                PriceText = $"₩{Math.Round(58000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-02",
                LanguageCode = "fr",
                Name = "Crêpe végétarienne",
                Description = "Crêpe croustillante végétarienne aux haricots mungo.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(58000m / 27000m, 2),
                PriceText = $"€{Math.Round(58000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-02",
                LanguageCode = "es",
                Name = "Panqueque vegetariano",
                Description = "Panqueque crujiente vegetariano con relleno de frijol mungo.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(58000m / 27000m, 2),
                PriceText = $"€{Math.Round(58000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-02",
                LanguageCode = "it",
                Name = "Pancake vegetariano",
                Description = "Pancake croccante vegetariano con ripieno di fagioli mung.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(58000m / 27000m, 2),
                PriceText = $"€{Math.Round(58000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-02",
                LanguageCode = "ru",
                Name = "Вегетарианский бань сео",
                Description = "Хрустящий вегетарианский блин с начинкой из маша.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(58000m / 300m, 2),
                PriceText = $"₽{Math.Round(58000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-03",
                LanguageCode = "vi",
                Name = "Bánh Xèo Đặc Biệt",
                Description = "Bánh xèo đặc biệt siêu to.",
                CurrencyCode = "VND",
                LocalizedPrice = 76000m,
                PriceText = "76.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-03",
                LanguageCode = "en",
                Name = "Special Pancake",
                Description = "Extra-large special crispy pancake.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(76000m / 25000m, 2),
                PriceText = $"${Math.Round(76000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-03",
                LanguageCode = "zh",
                Name = "特大特制煎饼",
                Description = "超大份特制越南煎饼。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(76000m / 3500m, 2),
                PriceText = $"¥{Math.Round(76000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-03",
                LanguageCode = "ja",
                Name = "特製バインセオ",
                Description = "特大サイズの特製バインセオ。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(76000m / 170m, 0),
                PriceText = $"¥{Math.Round(76000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-03",
                LanguageCode = "ko",
                Name = "스페셜 반쎄오",
                Description = "아주 큰 스페셜 반쎄오.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(76000m / 18m, 0),
                PriceText = $"₩{Math.Round(76000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-03",
                LanguageCode = "fr",
                Name = "Crêpe spéciale",
                Description = "Grande crêpe croustillante spéciale.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(76000m / 27000m, 2),
                PriceText = $"€{Math.Round(76000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-03",
                LanguageCode = "es",
                Name = "Panqueque especial",
                Description = "Panqueque crujiente especial de gran tamaño.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(76000m / 27000m, 2),
                PriceText = $"€{Math.Round(76000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-03",
                LanguageCode = "it",
                Name = "Pancake speciale",
                Description = "Grande pancake croccante speciale.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(76000m / 27000m, 2),
                PriceText = $"€{Math.Round(76000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-03",
                LanguageCode = "ru",
                Name = "Особый бань сео",
                Description = "Большой особый хрустящий блин.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(76000m / 300m, 2),
                PriceText = $"₽{Math.Round(76000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "vi",
                Name = "Gỏi Cuốn Tôm Thịt",
                Description = "Gỏi cuốn tôm thịt tươi ngon.",
                CurrencyCode = "VND",
                LocalizedPrice = 42000m,
                PriceText = "42.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "en",
                Name = "Shrimp Pork Spring Rolls",
                Description = "Fresh spring rolls with shrimp and pork.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(42000m / 25000m, 2),
                PriceText = $"${Math.Round(42000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "zh",
                Name = "鲜虾猪肉春卷",
                Description = "新鲜的虾肉猪肉春卷。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(42000m / 3500m, 2),
                PriceText = $"¥{Math.Round(42000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "ja",
                Name = "海老と豚肉の生春巻き",
                Description = "海老と豚肉入りの新鮮な生春巻き。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(42000m / 170m, 0),
                PriceText = $"¥{Math.Round(42000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "ko",
                Name = "새우 돼지고기 생춘권",
                Description = "새우와 돼지고기가 들어간 신선한 생춘권.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(42000m / 18m, 0),
                PriceText = $"₩{Math.Round(42000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "fr",
                Name = "Rouleaux frais crevettes et porc",
                Description = "Rouleaux frais avec crevettes et porc.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(42000m / 27000m, 2),
                PriceText = $"€{Math.Round(42000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "es",
                Name = "Rollitos frescos de camarón y cerdo",
                Description = "Rollitos frescos con camarón y cerdo.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(42000m / 27000m, 2),
                PriceText = $"€{Math.Round(42000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "it",
                Name = "Involtini freschi con gamberi e maiale",
                Description = "Involtini freschi con gamberi e maiale.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(42000m / 27000m, 2),
                PriceText = $"€{Math.Round(42000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "ru",
                Name = "Свежие роллы с креветками и свининой",
                Description = "Свежие роллы с креветками и свининой.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(42000m / 300m, 2),
                PriceText = $"₽{Math.Round(42000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-02",
                LanguageCode = "vi",
                Name = "Gỏi Cuốn Bò Nướng",
                Description = "Gỏi cuốn nhân bò nướng.",
                CurrencyCode = "VND",
                LocalizedPrice = 48000m,
                PriceText = "48.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-02",
                LanguageCode = "en",
                Name = "Beef Spring Rolls",
                Description = "Fresh spring rolls with grilled beef.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(48000m / 25000m, 2),
                PriceText = $"${Math.Round(48000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-02",
                LanguageCode = "zh",
                Name = "烤牛肉春卷",
                Description = "夹有烤牛肉的新鲜春卷。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(48000m / 3500m, 2),
                PriceText = $"¥{Math.Round(48000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-02",
                LanguageCode = "ja",
                Name = "牛肉の生春巻き",
                Description = "焼き牛肉入りの生春巻き。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(48000m / 170m, 0),
                PriceText = $"¥{Math.Round(48000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-02",
                LanguageCode = "ko",
                Name = "소고기 생춘권",
                Description = "구운 소고기가 들어간 생춘권.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(48000m / 18m, 0),
                PriceText = $"₩{Math.Round(48000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-02",
                LanguageCode = "fr",
                Name = "Rouleaux frais au bœuf",
                Description = "Rouleaux frais avec bœuf grillé.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(48000m / 27000m, 2),
                PriceText = $"€{Math.Round(48000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-02",
                LanguageCode = "es",
                Name = "Rollitos frescos de ternera",
                Description = "Rollitos frescos con carne de res a la parrilla.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(48000m / 27000m, 2),
                PriceText = $"€{Math.Round(48000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-02",
                LanguageCode = "it",
                Name = "Involtini freschi al manzo",
                Description = "Involtini freschi con manzo grigliato.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(48000m / 27000m, 2),
                PriceText = $"€{Math.Round(48000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-02",
                LanguageCode = "ru",
                Name = "Свежие роллы с говядиной",
                Description = "Свежие роллы с жареной говядиной.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(48000m / 300m, 2),
                PriceText = $"₽{Math.Round(48000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-03",
                LanguageCode = "vi",
                Name = "Combo 6 Cuốn",
                Description = "Combo 6 cuốn đầy đủ.",
                CurrencyCode = "VND",
                LocalizedPrice = 75000m,
                PriceText = "75.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-03",
                LanguageCode = "en",
                Name = "6-roll Combo",
                Description = "Combo of six assorted spring rolls.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(75000m / 25000m, 2),
                PriceText = $"${Math.Round(75000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-03",
                LanguageCode = "zh",
                Name = "六卷组合",
                Description = "六个什锦春卷组合。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(75000m / 3500m, 2),
                PriceText = $"¥{Math.Round(75000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-03",
                LanguageCode = "ja",
                Name = "6本ロールセット",
                Description = "6本入りの生春巻きコンボ。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(75000m / 170m, 0),
                PriceText = $"¥{Math.Round(75000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-03",
                LanguageCode = "ko",
                Name = "6개 롤 콤보",
                Description = "6개의 다양한 롤이 들어간 콤보.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(75000m / 18m, 0),
                PriceText = $"₩{Math.Round(75000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-03",
                LanguageCode = "fr",
                Name = "Combo 6 rouleaux",
                Description = "Combo de six rouleaux assortis.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(75000m / 27000m, 2),
                PriceText = $"€{Math.Round(75000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-03",
                LanguageCode = "es",
                Name = "Combo de 6 rollitos",
                Description = "Combo de seis rollitos variados.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(75000m / 27000m, 2),
                PriceText = $"€{Math.Round(75000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-03",
                LanguageCode = "it",
                Name = "Combo da 6 involtini",
                Description = "Combo di sei involtini assortiti.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(75000m / 27000m, 2),
                PriceText = $"€{Math.Round(75000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-03",
                LanguageCode = "ru",
                Name = "Комбо из 6 роллов",
                Description = "Комбо из шести ассорти-роллов.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(75000m / 300m, 2),
                PriceText = $"₽{Math.Round(75000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "vi",
                Name = "Mực Nướng Sa Tế",
                Description = "Mực nướng sa tế cay nồng.",
                CurrencyCode = "VND",
                LocalizedPrice = 98000m,
                PriceText = "98.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "en",
                Name = "Grilled Squid",
                Description = "Grilled squid with spicy sate sauce.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(98000m / 25000m, 2),
                PriceText = $"${Math.Round(98000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "zh",
                Name = "沙爹烤鱿鱼",
                Description = "配辣味沙爹酱的烤鱿鱼。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(98000m / 3500m, 2),
                PriceText = $"¥{Math.Round(98000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "ja",
                Name = "サテソース焼きイカ",
                Description = "ピリ辛サテソースの焼きイカ。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(98000m / 170m, 0),
                PriceText = $"¥{Math.Round(98000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "ko",
                Name = "사테 오징어 구이",
                Description = "매콤한 사테 소스를 곁들인 오징어 구이.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(98000m / 18m, 0),
                PriceText = $"₩{Math.Round(98000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "fr",
                Name = "Calmar grillé au saté",
                Description = "Calmar grillé avec sauce saté épicée.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(98000m / 27000m, 2),
                PriceText = $"€{Math.Round(98000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "es",
                Name = "Calamar a la parrilla con saté",
                Description = "Calamar a la parrilla con salsa saté picante.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(98000m / 27000m, 2),
                PriceText = $"€{Math.Round(98000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "it",
                Name = "Calamaro grigliato al saté",
                Description = "Calamaro grigliato con salsa saté piccante.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(98000m / 27000m, 2),
                PriceText = $"€{Math.Round(98000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "ru",
                Name = "Кальмар на гриле с соусом сате",
                Description = "Кальмар на гриле с острым соусом сате.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(98000m / 300m, 2),
                PriceText = $"₽{Math.Round(98000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-02",
                LanguageCode = "vi",
                Name = "Tôm Nướng Muối Ớt",
                Description = "Tôm nướng muối ớt đậm đà.",
                CurrencyCode = "VND",
                LocalizedPrice = 115000m,
                PriceText = "115.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-02",
                LanguageCode = "en",
                Name = "Grilled Shrimp",
                Description = "Grilled shrimp with chili salt.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(115000m / 25000m, 2),
                PriceText = $"${Math.Round(115000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-02",
                LanguageCode = "zh",
                Name = "椒盐烤虾",
                Description = "配辣椒盐的烤虾。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(115000m / 3500m, 2),
                PriceText = $"¥{Math.Round(115000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-02",
                LanguageCode = "ja",
                Name = "海老の塩辛焼き",
                Description = "唐辛子塩で焼いた海老。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(115000m / 170m, 0),
                PriceText = $"¥{Math.Round(115000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-02",
                LanguageCode = "ko",
                Name = "새우 소금고추 구이",
                Description = "고추소금을 곁들인 새우 구이.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(115000m / 18m, 0),
                PriceText = $"₩{Math.Round(115000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-02",
                LanguageCode = "fr",
                Name = "Crevettes grillées au sel pimenté",
                Description = "Crevettes grillées au sel et au piment.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(115000m / 27000m, 2),
                PriceText = $"€{Math.Round(115000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-02",
                LanguageCode = "es",
                Name = "Camarones a la parrilla con sal y chile",
                Description = "Camarones a la parrilla con sal picante.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(115000m / 27000m, 2),
                PriceText = $"€{Math.Round(115000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-02",
                LanguageCode = "it",
                Name = "Gamberi grigliati con sale e peperoncino",
                Description = "Gamberi grigliati con sale al peperoncino.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(115000m / 27000m, 2),
                PriceText = $"€{Math.Round(115000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-02",
                LanguageCode = "ru",
                Name = "Креветки на гриле с солью и перцем",
                Description = "Креветки на гриле с острым соляным соусом.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(115000m / 300m, 2),
                PriceText = $"₽{Math.Round(115000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-03",
                LanguageCode = "vi",
                Name = "Combo Hải Sản",
                Description = "Combo hải sản nướng thập cẩm.",
                CurrencyCode = "VND",
                LocalizedPrice = 149000m,
                PriceText = "149.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-03",
                LanguageCode = "en",
                Name = "Seafood Combo",
                Description = "Mixed grilled seafood combo.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(149000m / 25000m, 2),
                PriceText = $"${Math.Round(149000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-03",
                LanguageCode = "zh",
                Name = "海鲜组合",
                Description = "什锦烤海鲜组合。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(149000m / 3500m, 2),
                PriceText = $"¥{Math.Round(149000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-03",
                LanguageCode = "ja",
                Name = "シーフードコンボ",
                Description = "焼きシーフードの盛り合わせコンボ。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(149000m / 170m, 0),
                PriceText = $"¥{Math.Round(149000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-03",
                LanguageCode = "ko",
                Name = "해산물 콤보",
                Description = "모둠 해산물 구이 콤보.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(149000m / 18m, 0),
                PriceText = $"₩{Math.Round(149000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-03",
                LanguageCode = "fr",
                Name = "Combo fruits de mer",
                Description = "Combo de fruits de mer grillés assortis.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(149000m / 27000m, 2),
                PriceText = $"€{Math.Round(149000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-03",
                LanguageCode = "es",
                Name = "Combo de mariscos",
                Description = "Combo de mariscos a la parrilla surtidos.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(149000m / 27000m, 2),
                PriceText = $"€{Math.Round(149000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-03",
                LanguageCode = "it",
                Name = "Combo di frutti di mare",
                Description = "Combo misto di frutti di mare alla griglia.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(149000m / 27000m, 2),
                PriceText = $"€{Math.Round(149000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-03",
                LanguageCode = "ru",
                Name = "Комбо из морепродуктов",
                Description = "Ассорти-комбо из жареных морепродуктов.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(149000m / 300m, 2),
                PriceText = $"₽{Math.Round(149000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "vi",
                Name = "Cà Phê Sữa Đá",
                Description = "Cà phê sữa đá truyền thống.",
                CurrencyCode = "VND",
                LocalizedPrice = 30000m,
                PriceText = "30.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "en",
                Name = "Iced Milk Coffee",
                Description = "Traditional Vietnamese iced milk coffee.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(30000m / 25000m, 2),
                PriceText = $"${Math.Round(30000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "zh",
                Name = "越式冰奶咖啡",
                Description = "传统越式冰奶咖啡。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(30000m / 3500m, 2),
                PriceText = $"¥{Math.Round(30000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "ja",
                Name = "ベトナムアイスミルクコーヒー",
                Description = "伝統的なベトナム風アイスミルクコーヒー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(30000m / 170m, 0),
                PriceText = $"¥{Math.Round(30000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "ko",
                Name = "베트남 연유 아이스커피",
                Description = "전통적인 베트남식 연유 아이스커피.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(30000m / 18m, 0),
                PriceText = $"₩{Math.Round(30000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "fr",
                Name = "Café glacé au lait vietnamien",
                Description = "Café vietnamien glacé au lait condensé.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(30000m / 27000m, 2),
                PriceText = $"€{Math.Round(30000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "es",
                Name = "Café helado vietnamita con leche",
                Description = "Café vietnamita helado con leche condensada.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(30000m / 27000m, 2),
                PriceText = $"€{Math.Round(30000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "it",
                Name = "Caffè vietnamita freddo al latte",
                Description = "Caffè vietnamita freddo con latte condensato.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(30000m / 27000m, 2),
                PriceText = $"€{Math.Round(30000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "ru",
                Name = "Вьетнамский кофе со льдом и молоком",
                Description = "Традиционный вьетнамский кофе со льдом и молоком.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(30000m / 300m, 2),
                PriceText = $"₽{Math.Round(30000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-02",
                LanguageCode = "vi",
                Name = "Trà Sữa Trân Châu",
                Description = "Trà sữa trân châu đường đen.",
                CurrencyCode = "VND",
                LocalizedPrice = 42000m,
                PriceText = "42.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-02",
                LanguageCode = "en",
                Name = "Bubble Milk Tea",
                Description = "Milk tea with black sugar pearls.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(42000m / 25000m, 2),
                PriceText = $"${Math.Round(42000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-02",
                LanguageCode = "zh",
                Name = "黑糖珍珠奶茶",
                Description = "配黑糖珍珠的奶茶。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(42000m / 3500m, 2),
                PriceText = $"¥{Math.Round(42000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-02",
                LanguageCode = "ja",
                Name = "黒糖タピオカミルクティー",
                Description = "黒糖タピオカ入りミルクティー。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(42000m / 170m, 0),
                PriceText = $"¥{Math.Round(42000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-02",
                LanguageCode = "ko",
                Name = "흑당 버블 밀크티",
                Description = "흑당 펄이 들어간 밀크티.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(42000m / 18m, 0),
                PriceText = $"₩{Math.Round(42000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-02",
                LanguageCode = "fr",
                Name = "Thé au lait perlé",
                Description = "Thé au lait avec perles au sucre noir.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(42000m / 27000m, 2),
                PriceText = $"€{Math.Round(42000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-02",
                LanguageCode = "es",
                Name = "Té con leche y perlas",
                Description = "Té con leche con perlas de azúcar negro.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(42000m / 27000m, 2),
                PriceText = $"€{Math.Round(42000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-02",
                LanguageCode = "it",
                Name = "Tè al latte con perle",
                Description = "Tè al latte con perle di zucchero nero.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(42000m / 27000m, 2),
                PriceText = $"€{Math.Round(42000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-02",
                LanguageCode = "ru",
                Name = "Молочный чай с тапиокой",
                Description = "Молочный чай с шариками тапиоки и чёрным сахаром.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(42000m / 300m, 2),
                PriceText = $"₽{Math.Round(42000m / 300m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-03",
                LanguageCode = "vi",
                Name = "Combo Đồ Uống",
                Description = "Combo cà phê và trà sữa.",
                CurrencyCode = "VND",
                LocalizedPrice = 70000m,
                PriceText = "70.000 đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-03",
                LanguageCode = "en",
                Name = "Drink Combo",
                Description = "Coffee and milk tea combo.",
                CurrencyCode = "USD",
                LocalizedPrice = Math.Round(70000m / 25000m, 2),
                PriceText = $"${Math.Round(70000m / 25000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-03",
                LanguageCode = "zh",
                Name = "饮品组合",
                Description = "咖啡与奶茶组合。",
                CurrencyCode = "CNY",
                LocalizedPrice = Math.Round(70000m / 3500m, 2),
                PriceText = $"¥{Math.Round(70000m / 3500m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-03",
                LanguageCode = "ja",
                Name = "ドリンクコンボ",
                Description = "コーヒーとミルクティーのコンボ。",
                CurrencyCode = "JPY",
                LocalizedPrice = Math.Round(70000m / 170m, 0),
                PriceText = $"¥{Math.Round(70000m / 170m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-03",
                LanguageCode = "ko",
                Name = "음료 콤보",
                Description = "커피와 밀크티 콤보.",
                CurrencyCode = "KRW",
                LocalizedPrice = Math.Round(70000m / 18m, 0),
                PriceText = $"₩{Math.Round(70000m / 18m, 0):0}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-03",
                LanguageCode = "fr",
                Name = "Combo boissons",
                Description = "Combo café et thé au lait.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(70000m / 27000m, 2),
                PriceText = $"€{Math.Round(70000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-03",
                LanguageCode = "es",
                Name = "Combo de bebidas",
                Description = "Combo de café y té con leche.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(70000m / 27000m, 2),
                PriceText = $"€{Math.Round(70000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-03",
                LanguageCode = "it",
                Name = "Combo bevande",
                Description = "Combo di caffè e tè al latte.",
                CurrencyCode = "EUR",
                LocalizedPrice = Math.Round(70000m / 27000m, 2),
                PriceText = $"€{Math.Round(70000m / 27000m, 2):0.##}"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-03",
                LanguageCode = "ru",
                Name = "Комбо напитков",
                Description = "Комбо из кофе и молочного чая.",
                CurrencyCode = "RUB",
                LocalizedPrice = Math.Round(70000m / 300m, 2),
                PriceText = $"₽{Math.Round(70000m / 300m, 2):0.##}"
            });

            db.BoothMenuItemTranslations.AddRange(itemTranslations);
            await db.SaveChangesAsync();
        }
        if (!await db.BoothVisitLogs.AnyAsync())
        {
            db.BoothVisitLogs.AddRange(new List<BoothVisitLog>
    {
        new BoothVisitLog
        {
            VisitorUserId = "visitor-001",
            BoothId = "booth-01",
            TriggerType = "QR",
            Language = "vi",
            VisitedAtUtc = DateTime.Parse("2026-04-08T07:55:00Z"),
            SessionId = "session-001",
            Lat = 10.7768,
            Lng = 106.7008,
            IsSynced = true
        },
        new BoothVisitLog
        {
            VisitorUserId = "visitor-002",
            BoothId = "booth-02",
            TriggerType = "GPS",
            Language = "en",
            VisitedAtUtc = DateTime.Parse("2026-04-08T08:08:00Z"),
            SessionId = "session-002",
            Lat = 10.77698,
            Lng = 106.7008,
            IsSynced = true
        },
        new BoothVisitLog
        {
            VisitorUserId = "visitor-003",
            BoothId = "booth-03",
            TriggerType = "ManualOpen",
            Language = "ja",
            VisitedAtUtc = DateTime.Parse("2026-04-08T08:18:00Z"),
            SessionId = "session-003",
            Lat = 10.77716,
            Lng = 106.7008,
            IsSynced = true
        }
    });

            await db.SaveChangesAsync();
        }

        if (!await db.PlaybackLogs.AnyAsync())
        {
            db.PlaybackLogs.AddRange(new List<PlaybackLog>
    {
        new PlaybackLog
        {
            VisitorUserId = "visitor-001",
            BoothId = "booth-01",
            TriggerType = "QR",
            Language = "vi",
            PlayedAtUtc = DateTime.Parse("2026-04-08T08:00:00Z"),
            DurationSeconds = 12,
            Lat = 10.7768,
            Lng = 106.7008,
            IsCompleted = true,
            SessionId = "session-001",
            IsSynced = true
        },
        new PlaybackLog
        {
            VisitorUserId = "visitor-002",
            BoothId = "booth-02",
            TriggerType = "GPS",
            Language = "en",
            PlayedAtUtc = DateTime.Parse("2026-04-08T08:10:00Z"),
            DurationSeconds = 10,
            Lat = 10.77698,
            Lng = 106.7008,
            IsCompleted = true,
            SessionId = "session-002",
            IsSynced = true
        },
        new PlaybackLog
        {
            VisitorUserId = "visitor-003",
            BoothId = "booth-03",
            TriggerType = "Manual",
            Language = "ja",
            PlayedAtUtc = DateTime.Parse("2026-04-08T08:20:00Z"),
            DurationSeconds = 14,
            Lat = 10.77716,
            Lng = 106.7008,
            IsCompleted = true,
            SessionId = "session-003",
            IsSynced = true
        }
    });

            await db.SaveChangesAsync();
        }
    }
}