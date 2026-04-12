using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.Models.Entities;
using PoiNarration.Core.Models;

namespace PoiNarration.Api.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // Đảm bảo database được tạo trước khi nạp dữ liệu
        await db.Database.EnsureCreatedAsync();

        // 1. Bảng AppUsers (Người dùng) - Đầy đủ 3 tài khoản
        // 1. Bảng AppUsers (1 Admin + 10 Owners)
        if (!await db.AppUsers.AnyAsync())
        {
            var users = new List<AppUser>
    {
        new AppUser { Id = "admin", Username = "admin", Password = "123456", PasswordHash = "123456", FullName = "Administrator", Role = "Admin" }
    };

            // Vòng lặp tạo nhanh từ owner1 đến owner10
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

        // 2. Bảng Booths (Gian hàng) - Đầy đủ 10 gian hàng từ file poi.db
        // 2. Bảng Booths (Mỗi Booth 1 Owner riêng)
        if (!await db.Booths.AnyAsync())
        {
            db.Booths.AddRange(new List<Booth>
    {
        new Booth { Id = "booth-01", ZoneId = "zone-a", NameVi = "Phở Hà Nội", NameEn = "Hanoi Pho", Lat = 10.7768, Lng = 106.7008, RadiusMeters = 25, Priority = 1, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-01-menu-01.png", OwnerUserId = "owner1" },

new Booth { Id = "booth-02", ZoneId = "zone-a", NameVi = "Bún Bò Huế", NameEn = "Hue Beef Noodles", Lat = 10.77698, Lng = 106.7008, RadiusMeters = 25, Priority = 2, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-02-menu-01.png", OwnerUserId = "owner2" },

new Booth { Id = "booth-03", ZoneId = "zone-a", NameVi = "Cơm Tấm Sài Gòn", NameEn = "Saigon Broken Rice", Lat = 10.77716, Lng = 106.7008, RadiusMeters = 25, Priority = 3, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-03-menu-01.png", OwnerUserId = "owner3" },

new Booth { Id = "booth-04", ZoneId = "zone-a", NameVi = "Bánh Mì Việt", NameEn = "Vietnamese Banh Mi", Lat = 10.77734, Lng = 106.7008, RadiusMeters = 25, Priority = 4, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-04-menu-01.png", OwnerUserId = "owner4" },

new Booth { Id = "booth-05", ZoneId = "zone-a", NameVi = "Chè Ba Miền", NameEn = "Three-Region Sweet Soup", Lat = 10.77752, Lng = 106.7008, RadiusMeters = 25, Priority = 5, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-05-menu-01.png", OwnerUserId = "owner5" },

new Booth { Id = "booth-06", ZoneId = "zone-b", NameVi = "Nem Nướng Đà Lạt", NameEn = "Dalat Grilled Pork", Lat = 10.7768, Lng = 106.70102, RadiusMeters = 25, Priority = 6, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-06-menu-01.png", OwnerUserId = "owner6" },

new Booth { Id = "booth-07", ZoneId = "zone-b", NameVi = "Bánh Xèo Miền Tây", NameEn = "Mekong Pancake", Lat = 10.77698, Lng = 106.70102, RadiusMeters = 25, Priority = 7, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-07-menu-01.png", OwnerUserId = "owner7" },

new Booth { Id = "booth-08", ZoneId = "zone-b", NameVi = "Gỏi Cuốn Tươi", NameEn = "Fresh Spring Rolls", Lat = 10.77716, Lng = 106.70102, RadiusMeters = 25, Priority = 8, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-08-menu-01.png", OwnerUserId = "owner8" },

new Booth { Id = "booth-09", ZoneId = "zone-b", NameVi = "Hải Sản Nướng", NameEn = "Grilled Seafood", Lat = 10.77734, Lng = 106.70102, RadiusMeters = 25, Priority = 9, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-09-menu-01.png", OwnerUserId = "owner9" },

new Booth { Id = "booth-10", ZoneId = "zone-b", NameVi = "Cà Phê & Trà Sữa", NameEn = "Coffee & Milk Tea", Lat = 10.77752, Lng = 106.70102, RadiusMeters = 25, Priority = 10, IsActive = true, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-10-menu-01.png", OwnerUserId = "owner10" }
    });
            await db.SaveChangesAsync();
        }

        // 3. Bảng BoothTranslations (Bản dịch gian hàng) - 3 ví dụ
        // 3. Gieo mầm Bản dịch gian hàng (BoothTranslations) - Full 10 Booths x 9 Langs
        if (!await db.BoothTranslations.AnyAsync())
        {
            var translations = new List<BoothTranslationLocal>();

            // ==========================================
            // BOOTH 01 - Phở Hà Nội
            // ==========================================
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "vi",
                Name = "Phở Hà Nội",
                Description = "Phở Hà Nội - gian hàng nổi bật của hội chợ ẩm thực...",
                TtsScript = "Xin chào, bạn đang đến với gian hàng Phở Hà Nội.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-01-vi.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "en",
                Name = "Hanoi Pho",
                Description = "Hanoi Pho booth at the food fair with image menu...",
                TtsScript = "Welcome to Hanoi Pho.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-01-en.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "zh",
                Name = "河内牛肉粉",
                Description = "河内牛肉粉是本美食展的特色摊位之一，欢迎您前来体验地道风味。",
                TtsScript = "欢迎来到河内牛肉粉展位，这里为您提供正宗的越南河内风味牛肉粉。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "ja",
                Name = "ハノイフォー",
                Description = "ハノイフォーは本フードフェアの人気ブースの一つです。",
                TtsScript = "ハノイフォーのブースへようこそ。本場ベトナムの味をお楽しみください。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "ko",
                Name = "하노이 퍼",
                Description = "하노이 퍼는 본 음식 축제의 대표적인 부스 중 하나입니다.",
                TtsScript = "하노이 퍼 부스에 오신 것을 환영합니다. 베트남 정통 하노이 쌀국수를 즐겨보세요."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "fr",
                Name = "Pho de Hanoï",
                Description = "Le stand Pho de Hanoï est l'un des stands phares de ce festival gastronomique.",
                TtsScript = "Bienvenue au stand Pho de Hanoï. Profitez de saveurs authentiques."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "es",
                Name = "Pho de Hanói",
                Description = "El stand de Pho de Hanói es uno de los más destacados de esta feria gastronómica.",
                TtsScript = "Bienvenido al stand de Pho de Hanói. Disfrute de sabores auténticos."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "it",
                Name = "Pho di Hanoi",
                Description = "Lo stand di Pho di Hanoi è uno dei più importanti di questo festival gastronomico.",
                TtsScript = "Benvenuto allo stand di Pho di Hanoi. Goditi i sapori autentici."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-01",
                LanguageCode = "ru",
                Name = "Ханойский фо",
                Description = "Стенд Ханойский фо — один из самых популярных на этом гастрономическом фестивале.",
                TtsScript = "Добро пожаловать на стенд Ханойский фо. Наслаждайтесь аутентичными вкусами."
            });

            // ==========================================
            // BOOTH 02 - Bún Bò Huế
            // ==========================================
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "vi",
                Name = "Bún Bò Huế",
                Description = "Bún Bò Huế - gian hàng nổi bật của hội chợ ẩm thực...",
                TtsScript = "Xin chào, bạn đang đến với gian hàng Bún Bò Huế.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-02-vi.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "en",
                Name = "Hue Beef Noodles",
                Description = "Hue Beef Noodles booth at the food fair...",
                TtsScript = "Welcome to Hue Beef Noodles.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-02-en.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "zh",
                Name = "顺化牛肉粉",
                Description = "顺化牛肉粉是本美食展的特色摊位之一，欢迎您前来体验地道风味。",
                TtsScript = "欢迎来到顺化牛肉粉展位，这里为您提供正宗的美食。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "ja",
                Name = "フエ牛肉麺",
                Description = "フエ牛肉麺は本フードフェアの人気ブースの一つです。",
                TtsScript = "フエ牛肉麺のブースへようこそ。本場の味をお楽しみください。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "ko",
                Name = "후에 소고기 국수",
                Description = "후에 소고기 국수은(는) 본 음식 축제의 대표적인 부스 중 하나입니다.",
                TtsScript = "후에 소고기 국수 부스에 오신 것을 환영합니다. 정통의 맛을 즐겨보세요."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "fr",
                Name = "Bún Bò Huế",
                Description = "Le stand Bún Bò Huế est l'un des stands phares de ce festival gastronomique.",
                TtsScript = "Bienvenue au stand Bún Bò Huế. Profitez de saveurs authentiques."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "es",
                Name = "Bún Bò Huế",
                Description = "El stand de Bún Bò Huế es uno de los más destacados de esta feria gastronómica.",
                TtsScript = "Bienvenido al stand de Bún Bò Huế. Disfrute de sabores auténticos."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "it",
                Name = "Bún Bò Huế",
                Description = "Lo stand di Bún Bò Huế è uno dei più importanti di questo festival gastronomico.",
                TtsScript = "Benvenuto allo stand di Bún Bò Huế. Goditi i sapori autentici."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-02",
                LanguageCode = "ru",
                Name = "Хюэский суп с говядиной",
                Description = "Стенд Хюэский суп с говядиной — один из самых популярных на этом гастрономическом фестивале.",
                TtsScript = "Добро пожаловать на стенд Хюэский суп с говядиной. Наслаждайтесь аутентичными вкусами."
            });

            // ==========================================
            // BOOTH 03 - Cơm Tấm Sài Gòn
            // ==========================================
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "vi",
                Name = "Cơm Tấm Sài Gòn",
                Description = "Cơm Tấm Sài Gòn - gian hàng ẩm thực nổi bật...",
                TtsScript = "Xin chào, bạn đang đến với gian hàng Cơm Tấm Sài Gòn.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-03-vi.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "en",
                Name = "Saigon Broken Rice",
                Description = "Saigon Broken Rice booth at the food fair...",
                TtsScript = "Welcome to Saigon Broken Rice.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-03-en.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "zh",
                Name = "西贡碎米饭",
                Description = "西贡碎米饭是本美食展的特色摊位之一，欢迎您前来体验地道风味。",
                TtsScript = "欢迎来到西贡碎米饭展位，这里为您提供正宗的美食。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "ja",
                Name = "サイゴン焼き豚のせご飯",
                Description = "サイゴン焼き豚のせご飯は本フードフェアの人気ブースの一つです。",
                TtsScript = "サイゴン焼き豚のせご飯のブースへようこそ。本場の味をお楽しみください。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "ko",
                Name = "사이공 껌땀",
                Description = "사이공 껌땀은(는) 본 음식 축제의 대표적인 부스 중 하나입니다.",
                TtsScript = "사이공 껌땀 부스에 오신 것을 환영합니다. 정통의 맛을 즐겨보세요."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "fr",
                Name = "Riz brisé de Saïgon",
                Description = "Le stand Riz brisé de Saïgon est l'un des stands phares de ce festival gastronomique.",
                TtsScript = "Bienvenue au stand Riz brisé de Saïgon. Profitez de saveurs authentiques."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "es",
                Name = "Arroz quebrado de Saigón",
                Description = "El stand de Arroz quebrado de Saigón es uno de los más destacados de esta feria gastronómica.",
                TtsScript = "Bienvenido al stand de Arroz quebrado de Saigón. Disfrute de sabores auténticos."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "it",
                Name = "Riso spezzato di Saigon",
                Description = "Lo stand di Riso spezzato di Saigon è uno dei più importanti di questo festival gastronomico.",
                TtsScript = "Benvenuto allo stand di Riso spezzato di Saigon. Goditi i sapori autentici."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-03",
                LanguageCode = "ru",
                Name = "Сайгонский дроблёный рис",
                Description = "Стенд Сайгонский дроблёный рис — один из самых популярных на этом гастрономическом фестивале.",
                TtsScript = "Добро пожаловать на стенд Сайгонский дроблёный рис. Наслаждайтесь аутентичными вкусами."
            });

            // ==========================================
            // BOOTH 04 - Bánh Mì Việt
            // ==========================================
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "vi",
                Name = "Bánh Mì Việt",
                Description = "Bánh Mì Việt - gian hàng ẩm thực nổi bật...",
                TtsScript = "Xin chào, bạn đang đến với gian hàng Bánh Mì Việt.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-04-vi.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "en",
                Name = "Vietnamese Banh Mi",
                Description = "Vietnamese Banh Mi booth at the food fair...",
                TtsScript = "Welcome to Vietnamese Banh Mi.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-04-en.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "zh",
                Name = "越式法棍",
                Description = "越式法棍是本美食展的特色摊位之一，欢迎您前来体验地道风味。",
                TtsScript = "欢迎来到越式法棍展位，这里为您提供正宗的美食。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "ja",
                Name = "ベトナムバインミー",
                Description = "ベトナムバインミーは本フードフェアの人気ブースの一つです。",
                TtsScript = "ベトナムバインミーのブースへようこそ。本場の味をお楽しみください。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "ko",
                Name = "베트남 반미",
                Description = "베트남 반미은(는) 본 음식 축제의 대표적인 부스 중 하나입니다.",
                TtsScript = "베트남 반미 부스에 오신 것을 환영합니다. 정통의 맛을 즐겨보세요."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "fr",
                Name = "Bánh Mì vietnamien",
                Description = "Le stand Bánh Mì vietnamien est l'un des stands phares de ce festival gastronomique.",
                TtsScript = "Bienvenue au stand Bánh Mì vietnamien. Profitez de saveurs authentiques."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "es",
                Name = "Bánh Mì vietnamita",
                Description = "El stand de Bánh Mì vietnamita es uno de los más destacados de esta feria gastronómica.",
                TtsScript = "Bienvenido al stand de Bánh Mì vietnamita. Disfrute de sabores auténticos."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "it",
                Name = "Bánh Mì vietnamita",
                Description = "Lo stand di Bánh Mì vietnamita è uno dei più importanti di questo festival gastronomico.",
                TtsScript = "Benvenuto allo stand di Bánh Mì vietnamita. Goditi i sapori autentici."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-04",
                LanguageCode = "ru",
                Name = "Вьетнамский баньми",
                Description = "Стенд Вьетнамский баньми — один из самых популярных на этом гастрономическом фестивале.",
                TtsScript = "Добро пожаловать на стенд Вьетнамский баньми. Наслаждайтесь аутентичными вкусами."
            });

            // ==========================================
            // BOOTH 05 - Chè Ba Miền
            // ==========================================
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "vi",
                Name = "Chè Ba Miền",
                Description = "Chè Ba Miền - gian hàng ẩm thực nổi bật...",
                TtsScript = "Xin chào, bạn đang đến với gian hàng Chè Ba Miền.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-05-vi.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "en",
                Name = "Three-Region Sweet Soup",
                Description = "Three-Region Sweet Soup booth...",
                TtsScript = "Welcome to Three-Region Sweet Soup.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-05-en.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "zh",
                Name = "三地甜汤",
                Description = "三地甜汤是本美食展的特色摊位之一，欢迎您前来体验地道风味。",
                TtsScript = "欢迎来到三地甜汤展位，这里为您提供正宗的美食。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "ja",
                Name = "三地域のチェー",
                Description = "三地域のチェーは本フードフェアの人気ブースの一つです。",
                TtsScript = "三地域のチェーのブースへようこそ。本場の味をお楽しみください。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "ko",
                Name = "삼지역 체",
                Description = "삼지역 체은(는) 본 음식 축제의 대표적인 부스 중 하나입니다.",
                TtsScript = "삼지역 체 부스에 오신 것을 환영합니다. 정통의 맛을 즐겨보세요."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "fr",
                Name = "Soupe sucrée des trois régions",
                Description = "Le stand Soupe sucrée des trois régions est l'un des stands phares de ce festival gastronomique.",
                TtsScript = "Bienvenue au stand Soupe sucrée des trois régions. Profitez de saveurs authentiques."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "es",
                Name = "Postre de las tres regiones",
                Description = "El stand de Postre de las tres regiones es uno de los más destacados de esta feria gastronómica.",
                TtsScript = "Bienvenido al stand de Postre de las tres regiones. Disfrute de sabores auténticos."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "it",
                Name = "Dolce delle tre regioni",
                Description = "Lo stand di Dolce delle tre regioni è uno dei più importanti di questo festival gastronomico.",
                TtsScript = "Benvenuto allo stand di Dolce delle tre regioni. Goditi i sapori autentici."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-05",
                LanguageCode = "ru",
                Name = "Сладкий суп трёх регионов",
                Description = "Стенд Сладкий суп трёх регионов — один из самых популярных на этом гастрономическом фестивале.",
                TtsScript = "Добро пожаловать на стенд Сладкий суп трёх регионов. Наслаждайтесь аутентичными вкусами."
            });

            // ==========================================
            // BOOTH 06 - Nem Nướng Đà Lạt
            // ==========================================
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "vi",
                Name = "Nem Nướng Đà Lạt",
                Description = "Nem Nướng Đà Lạt - gian hàng nổi bật của hội chợ ẩm thực...",
                TtsScript = "Xin chào, bạn đang đến với gian hàng Nem Nướng Đà Lạt.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-06-vi.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "en",
                Name = "Dalat Grilled Pork Rolls",
                Description = "Dalat Grilled Pork Rolls booth at the food fair...",
                TtsScript = "Welcome to Dalat Grilled Pork Rolls.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-06-en.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "zh",
                Name = "大叻烤肉卷",
                Description = "大叻烤肉卷是本美食展的特色摊位之一，欢迎您前来体验地道风味。",
                TtsScript = "欢迎来到大叻烤肉卷展位，这里为您提供正宗的美食。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "ja",
                Name = "ダラット焼き豚ロール",
                Description = "ダラット焼き豚ロールは本フードフェアの人気ブースの一つです。",
                TtsScript = "ダラット焼き豚ロールのブースへようこそ。本場の味をお楽しみください。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "ko",
                Name = "달랏 넴느엉",
                Description = "달랏 넴느엉은(는) 본 음식 축제의 대표적인 부스 중 하나입니다.",
                TtsScript = "달랏 넴느엉 부스에 오신 것을 환영합니다. 정통의 맛을 즐겨보세요."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "fr",
                Name = "Rouleaux de porc grillé de Dalat",
                Description = "Le stand Rouleaux de porc grillé de Dalat est l'un des stands phares de ce festival gastronomique.",
                TtsScript = "Bienvenue au stand Rouleaux de porc grillé de Dalat. Profitez de saveurs authentiques."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "es",
                Name = "Rollos de cerdo a la parrilla de Dalat",
                Description = "El stand de Rollos de cerdo a la parrilla de Dalat es uno de los más destacados de esta feria gastronómica.",
                TtsScript = "Bienvenido al stand de Rollos de cerdo a la parrilla de Dalat. Disfrute de sabores auténticos."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "it",
                Name = "Involtini di maiale grigliato di Dalat",
                Description = "Lo stand di Involtini di maiale grigliato di Dalat è uno dei più importanti di questo festival gastronomico.",
                TtsScript = "Benvenuto allo stand di Involtini di maiale grigliato di Dalat. Goditi i sapori autentici."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-06",
                LanguageCode = "ru",
                Name = "Далатские роллы из жареной свинины",
                Description = "Стенд Далатские роллы из жареной свинины — один из самых популярных на этом гастрономическом фестивале.",
                TtsScript = "Добро пожаловать на стенд Далатские роллы из жареной свинины. Наслаждайтесь аутентичными вкусами."
            });

            // ==========================================
            // BOOTH 07 - Bánh Xèo Miền Tây
            // ==========================================
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "vi",
                Name = "Bánh Xèo Miền Tây",
                Description = "Bánh Xèo Miền Tây - gian hàng nổi bật của hội chợ ẩm thực...",
                TtsScript = "Xin chào, bạn đang đến với gian hàng Bánh Xèo Miền Tây.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-07-vi.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "en",
                Name = "Mekong Crispy Pancake",
                Description = "Mekong Crispy Pancake booth at the food fair...",
                TtsScript = "Welcome to Mekong Crispy Pancake.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-07-en.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "zh",
                Name = "湄公河煎饼",
                Description = "湄公河煎饼是本美食展的特色摊位之一，欢迎您前来体验地道风味。",
                TtsScript = "欢迎来到湄公河煎饼展位，这里为您提供正宗的美食。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "ja",
                Name = "メコン風バインセオ",
                Description = "メコン風バインセオは本フードフェアの人気ブースの一つです。",
                TtsScript = "メコン風バインセオのブースへようこそ。本場の味をお楽しみください。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "ko",
                Name = "메콩식 반쎄오",
                Description = "메콩식 반쎄오은(는) 본 음식 축제의 대표적인 부스 중 하나입니다.",
                TtsScript = "메콩식 반쎄오 부스에 오신 것을 환영합니다. 정통의 맛을 즐겨보세요."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "fr",
                Name = "Crêpe croustillante du Mékong",
                Description = "Le stand Crêpe croustillante du Mékong est l'un des stands phares de ce festival gastronomique.",
                TtsScript = "Bienvenue au stand Crêpe croustillante du Mékong. Profitez de saveurs authentiques."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "es",
                Name = "Panqueque crujiente del Mekong",
                Description = "El stand de Panqueque crujiente del Mekong es uno de los más destacados de esta feria gastronómica.",
                TtsScript = "Bienvenido al stand de Panqueque crujiente del Mekong. Disfrute de sabores auténticos."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "it",
                Name = "Pancake croccante del Mekong",
                Description = "Lo stand di Pancake croccante del Mekong è uno dei più importanti di questo festival gastronomico.",
                TtsScript = "Benvenuto allo stand di Pancake croccante del Mekong. Goditi i sapori autentici."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-07",
                LanguageCode = "ru",
                Name = "Хрустящий блин Меконга",
                Description = "Стенд Хрустящий блин Меконга — один из самых популярных на этом гастрономическом фестивале.",
                TtsScript = "Добро пожаловать на стенд Хрустящий блин Меконга. Наслаждайтесь аутентичными вкусами."
            });

            // ==========================================
            // BOOTH 08 - Gỏi Cuốn Tươi
            // ==========================================
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "vi",
                Name = "Gỏi Cuốn Tươi",
                Description = "Gỏi Cuốn Tươi - gian hàng nổi bật của hội chợ ẩm thực...",
                TtsScript = "Xin chào, bạn đang đến với gian hàng Gỏi Cuốn Tươi.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-08-vi.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "en",
                Name = "Fresh Spring Rolls",
                Description = "Fresh Spring Rolls booth at the food fair...",
                TtsScript = "Welcome to Fresh Spring Rolls.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-08-en.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "zh",
                Name = "鲜春卷",
                Description = "鲜春卷是本美食展的特色摊位之一，欢迎您前来体验地道风味。",
                TtsScript = "欢迎来到鲜春卷展位，这里为您提供正宗的美食。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "ja",
                Name = "生春巻き",
                Description = "生春巻きは本フードフェアの人気ブースの一つです。",
                TtsScript = "生春巻きのブースへようこそ。本場の味をお楽しみください。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "ko",
                Name = "생춘권",
                Description = "생춘권은(는) 본 음식 축제의 대표적인 부스 중 하나입니다.",
                TtsScript = "생춘권 부스에 오신 것을 환영합니다. 정통의 맛을 즐겨보세요."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "fr",
                Name = "Rouleaux de printemps frais",
                Description = "Le stand Rouleaux de printemps frais est l'un des stands phares de ce festival gastronomique.",
                TtsScript = "Bienvenue au stand Rouleaux de printemps frais. Profitez de saveurs authentiques."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "es",
                Name = "Rollitos frescos",
                Description = "El stand de Rollitos frescos es uno de los más destacados de esta feria gastronómica.",
                TtsScript = "Bienvenido al stand de Rollitos frescos. Disfrute de sabores auténticos."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "it",
                Name = "Involtini freschi",
                Description = "Lo stand di Involtini freschi è uno dei più importanti di questo festival gastronomico.",
                TtsScript = "Benvenuto allo stand di Involtini freschi. Goditi i sapori autentici."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-08",
                LanguageCode = "ru",
                Name = "Свежие спринг-роллы",
                Description = "Стенд Свежие спринг-роллы — один из самых популярных на этом гастрономическом фестивале.",
                TtsScript = "Добро пожаловать на стенд Свежие спринг-роллы. Наслаждайтесь аутентичными вкусами."
            });

            // ==========================================
            // BOOTH 09 - Hải Sản Nướng
            // ==========================================
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "vi",
                Name = "Hải Sản Nướng",
                Description = "Hải Sản Nướng - gian hàng nổi bật của hội chợ ẩm thực...",
                TtsScript = "Xin chào, bạn đang đến với gian hàng Hải Sản Nướng.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-09-vi.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "en",
                Name = "Grilled Seafood",
                Description = "Grilled Seafood booth at the food fair...",
                TtsScript = "Welcome to Grilled Seafood.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-09-en.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "zh",
                Name = "烤海鲜",
                Description = "烤海鲜是本美食展的特色摊位之一，欢迎您前来体验地道风味。",
                TtsScript = "欢迎来到烤海鲜展位，这里为您提供正宗的美食。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "ja",
                Name = "焼きシーフード",
                Description = "焼きシーフードは本フードフェアの人気ブースの一つです。",
                TtsScript = "焼きシーフードのブースへようこそ。本場の味をお楽しみください。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "ko",
                Name = "구운 해산물",
                Description = "구운 해산물은(는) 본 음식 축제의 대표적인 부스 중 하나입니다.",
                TtsScript = "구운 해산물 부스에 오신 것을 환영합니다. 정통의 맛을 즐겨보세요."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "fr",
                Name = "Fruits de mer grillés",
                Description = "Le stand Fruits de mer grillés est l'un des stands phares de ce festival gastronomique.",
                TtsScript = "Bienvenue au stand Fruits de mer grillés. Profitez de saveurs authentiques."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "es",
                Name = "Mariscos a la parrilla",
                Description = "El stand de Mariscos a la parrilla es uno de los más destacados de esta feria gastronómica.",
                TtsScript = "Bienvenido al stand de Mariscos a la parrilla. Disfrute de sabores auténticos."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "it",
                Name = "Frutti di mare alla griglia",
                Description = "Lo stand di Frutti di mare alla griglia è uno dei più importanti di questo festival gastronomico.",
                TtsScript = "Benvenuto allo stand di Frutti di mare alla griglia. Goditi i sapori autentici."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-09",
                LanguageCode = "ru",
                Name = "Жареные морепродукты",
                Description = "Стенд Жареные морепродукты — один из самых популярных на этом гастрономическом фестивале.",
                TtsScript = "Добро пожаловать на стенд Жареные морепродукты. Наслаждайтесь аутентичными вкусами."
            });

            // ==========================================
            // BOOTH 10 - Cà Phê & Trà Sữa
            // ==========================================
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "vi",
                Name = "Cà Phê & Trà Sữa",
                Description = "Cà Phê & Trà Sữa - gian hàng nổi bật của hội chợ ẩm thực...",
                TtsScript = "Xin chào, bạn đang đến với gian hàng Cà Phê & Trà Sữa.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-10-vi.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "en",
                Name = "Coffee & Milk Tea",
                Description = "Coffee & Milk Tea booth at the food fair...",
                TtsScript = "Welcome to Coffee & Milk Tea.",
                AudioUrl = "http://192.168.1.237:5151/uploads/audio/booth-10-en.mp3"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "zh",
                Name = "咖啡与奶茶",
                Description = "咖啡与奶茶是本美食展的特色摊位之一，欢迎您前来体验地道风味。",
                TtsScript = "欢迎来到咖啡与奶茶展位，这里为您提供正宗的美食。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "ja",
                Name = "コーヒー＆ミルクティー",
                Description = "コーヒー＆ミルクティーは本フードフェアの人気ブースの一つです。",
                TtsScript = "コーヒー＆ミルクティーのブースへようこそ。本場の味をお楽しみください。"
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "ko",
                Name = "커피 & 밀크티",
                Description = "커피 & 밀크티은(는) 본 음식 축제의 대표적인 부스 중 하나입니다.",
                TtsScript = "커피 & 밀크티 부스에 오신 것을 환영합니다. 정통의 맛을 즐겨보세요."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "fr",
                Name = "Café & thé au lait",
                Description = "Le stand Café & thé au lait est l'un des stands phares de ce festival gastronomique.",
                TtsScript = "Bienvenue au stand Café & thé au lait. Profitez de saveurs authentiques."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "es",
                Name = "Café y té con leche",
                Description = "El stand de Café y té con leche es uno de los más destacados de esta feria gastronómica.",
                TtsScript = "Bienvenido al stand de Café y té con leche. Disfrute de sabores auténticos."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "it",
                Name = "Caffè e tè al latte",
                Description = "Lo stand di Caffè e tè al latte è uno dei più importanti di questo festival gastronomico.",
                TtsScript = "Benvenuto allo stand di Caffè e tè al latte. Goditi i sapori autentici."
            });
            translations.Add(new BoothTranslationLocal
            {
                BoothId = "booth-10",
                LanguageCode = "ru",
                Name = "Кофе и молочный чай",
                Description = "Стенд Кофе и молочный чай — один из самых популярных на этом гастрономическом фестивале.",
                TtsScript = "Добро пожаловать на стенд Кофе и молочный чай. Наслаждайтесь аутентичными вкусами."
            }); 
            db.BoothTranslations.AddRange(translations);
            await db.SaveChangesAsync();
        }

        // 4. Bảng BoothMenuItems (Món ăn) - 3 ví dụ
        // 4. Bảng BoothMenuItems (Món ăn) - Đầy đủ 30 món từ poi.db
        if (!await db.BoothMenuItems.AnyAsync())
        {
            var menuItems = new List<BoothMenuItem>();

            // --- Booth 01: Phở Hà Nội ---
            menuItems.Add(new BoothMenuItem { Id = "booth-01-menu-01", BoothId = "booth-01", Name = "Phở Đặc Biệt", NameEn = "Pho Special", Description = "Phở đặc biệt đầy đủ topping.", DescriptionEn = "Pho Special item for demo.", Price = 65000m, PriceUsd = 2.60m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-01-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-01-menu-02", BoothId = "booth-01", Name = "Phở Tái Nạm", NameEn = "Rare & Brisket Pho", Description = "Phở tái nạm tươi ngon.", DescriptionEn = "Rare & Brisket Pho item.", Price = 72000m, PriceUsd = 2.88m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-01-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-01-menu-03", BoothId = "booth-01", Name = "Phở Bò Viên", NameEn = "Meatball Pho", Description = "Phở bò viên dai giòn.", DescriptionEn = "Meatball Pho item.", Price = 59000m, PriceUsd = 2.36m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-01-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 02: Bún Bò Huế ---
            menuItems.Add(new BoothMenuItem { Id = "booth-02-menu-01", BoothId = "booth-02", Name = "Bún Bò Đặc Biệt", NameEn = "Special Hue Noodles", Description = "Bún bò Huế đặc biệt.", DescriptionEn = "Special Hue Noodles item.", Price = 68000m, PriceUsd = 2.72m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-02-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-02-menu-02", BoothId = "booth-02", Name = "Bún Bò Giò Heo", NameEn = "Hue Noodles with Pork Hock", Description = "Bún bò giò heo đặc trưng.", DescriptionEn = "Hue Noodles with Pork Hock item.", Price = 75000m, PriceUsd = 3.00m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-02-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-02-menu-03", BoothId = "booth-02", Name = "Bún Bò Thập Cẩm", NameEn = "Mixed Hue Noodles", Description = "Bún bò thập cẩm đầy đủ.", DescriptionEn = "Mixed Hue Noodles item.", Price = 79000m, PriceUsd = 3.16m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-02-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 03: Cơm Tấm Sài Gòn ---
            menuItems.Add(new BoothMenuItem { Id = "booth-03-menu-01", BoothId = "booth-03", Name = "Cơm Tấm Sườn Bì Chả", NameEn = "Broken Rice Combo", Description = "Cơm tấm sườn bì chả truyền thống.", DescriptionEn = "Broken Rice Combo item.", Price = 62000m, PriceUsd = 2.48m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-03-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-03-menu-02", BoothId = "booth-03", Name = "Cơm Tấm Sườn Nướng", NameEn = "Broken Rice Grilled Pork", Description = "Sườn nướng thơm ngon.", DescriptionEn = "Broken Rice Grilled Pork item.", Price = 69000m, PriceUsd = 2.76m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-03-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-03-menu-03", BoothId = "booth-03", Name = "Cơm Tấm Đặc Biệt", NameEn = "Special Broken Rice", Description = "Cơm tấm đặc biệt.", DescriptionEn = "Special Broken Rice item.", Price = 79000m, PriceUsd = 3.16m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-03-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 04: Bánh Mì Việt ---
            menuItems.Add(new BoothMenuItem { Id = "booth-04-menu-01", BoothId = "booth-04", Name = "Bánh Mì Thịt Nướng", NameEn = "Grilled Pork Banh Mi", Description = "Bánh mì kẹp thịt nướng.", DescriptionEn = "Grilled Pork Banh Mi item.", Price = 35000m, PriceUsd = 1.40m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-04-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-04-menu-02", BoothId = "booth-04", Name = "Bánh Mì Gà", NameEn = "Chicken Banh Mi", Description = "Bánh mì gà xé.", DescriptionEn = "Chicken Banh Mi item.", Price = 38000m, PriceUsd = 1.52m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-04-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-04-menu-03", BoothId = "booth-04", Name = "Bánh Mì Đặc Biệt", NameEn = "Special Banh Mi", Description = "Bánh mì đặc biệt đầy đủ.", DescriptionEn = "Special Banh Mi item.", Price = 45000m, PriceUsd = 1.80m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-04-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 05: Chè Ba Miền ---
            menuItems.Add(new BoothMenuItem { Id = "booth-05-menu-01", BoothId = "booth-05", Name = "Chè Đậu Xanh", NameEn = "Mung Bean Sweet Soup", Description = "Chè đậu xanh thanh mát.", DescriptionEn = "Mung Bean Sweet Soup item.", Price = 28000m, PriceUsd = 1.12m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-05-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-05-menu-02", BoothId = "booth-05", Name = "Chè Thập Cẩm", NameEn = "Mixed Sweet Soup", Description = "Chè thập cẩm đủ loại.", DescriptionEn = "Mixed Sweet Soup item.", Price = 32000m, PriceUsd = 1.28m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-05-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-05-menu-03", BoothId = "booth-05", Name = "Chè Dừa Non", NameEn = "Young Coconut Sweet Soup", Description = "Chè dừa non béo ngậy.", DescriptionEn = "Young Coconut Sweet Soup item.", Price = 36000m, PriceUsd = 1.44m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-05-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 06: Nem Nướng Đà Lạt ---
            menuItems.Add(new BoothMenuItem { Id = "booth-06-menu-01", BoothId = "booth-06", Name = "Nem Nướng Phần", NameEn = "Grilled Pork Rolls Set", Description = "Nem nướng đặc sản Đà Lạt.", DescriptionEn = "Grilled Pork Rolls Set item.", Price = 68000m, PriceUsd = 2.72m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-06-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-06-menu-02", BoothId = "booth-06", Name = "Nem Nướng Combo", NameEn = "Grilled Pork Combo", Description = "Combo nem nướng hấp dẫn.", DescriptionEn = "Grilled Pork Combo item.", Price = 79000m, PriceUsd = 3.16m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-06-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-06-menu-03", BoothId = "booth-06", Name = "Nem Nướng Đặc Biệt", NameEn = "Special Grilled Pork Rolls", Description = "Nem nướng đặc biệt.", DescriptionEn = "Special Grilled Pork Rolls item.", Price = 89000m, PriceUsd = 3.56m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-06-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 07: Bánh Xèo Miền Tây ---
            menuItems.Add(new BoothMenuItem { Id = "booth-07-menu-01", BoothId = "booth-07", Name = "Bánh Xèo Tôm Thịt", NameEn = "Shrimp Pork Pancake", Description = "Bánh xèo nhân tôm thịt.", DescriptionEn = "Shrimp Pork Pancake item.", Price = 65000m, PriceUsd = 2.60m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-07-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-07-menu-02", BoothId = "booth-07", Name = "Bánh Xèo Chay", NameEn = "Vegetarian Pancake", Description = "Bánh xèo nhân đậu xanh.", DescriptionEn = "Vegetarian Pancake item.", Price = 58000m, PriceUsd = 2.32m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-07-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-07-menu-03", BoothId = "booth-07", Name = "Bánh Xèo Đặc Biệt", NameEn = "Special Pancake", Description = "Bánh xèo đặc biệt siêu to.", DescriptionEn = "Special Pancake item.", Price = 76000m, PriceUsd = 3.04m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-07-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 08: Gỏi Cuốn Tươi ---
            menuItems.Add(new BoothMenuItem { Id = "booth-08-menu-01", BoothId = "booth-08", Name = "Gỏi Cuốn Tôm Thịt", NameEn = "Shrimp Pork Spring Rolls", Description = "Gỏi cuốn tôm thịt tươi ngon.", DescriptionEn = "Shrimp Pork Spring Rolls item.", Price = 42000m, PriceUsd = 1.68m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-08-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-08-menu-02", BoothId = "booth-08", Name = "Gỏi Cuốn Bò Nướng", NameEn = "Beef Spring Rolls", Description = "Gỏi cuốn nhân bò nướng.", DescriptionEn = "Beef Spring Rolls item.", Price = 48000m, PriceUsd = 1.92m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-08-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-08-menu-03", BoothId = "booth-08", Name = "Combo 6 Cuốn", NameEn = "6-roll Combo", Description = "Combo 6 cuốn đầy đủ.", DescriptionEn = "6-roll Combo item.", Price = 75000m, PriceUsd = 3.00m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-08-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 09: Hải Sản Nướng ---
            menuItems.Add(new BoothMenuItem { Id = "booth-09-menu-01", BoothId = "booth-09", Name = "Mực Nướng Sa Tế", NameEn = "Grilled Squid", Description = "Mực nướng sa tế cay nồng.", DescriptionEn = "Grilled Squid item.", Price = 98000m, PriceUsd = 3.92m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-09-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-09-menu-02", BoothId = "booth-09", Name = "Tôm Nướng Muối Ớt", NameEn = "Grilled Shrimp", Description = "Tôm nướng muối ớt đậm đà.", DescriptionEn = "Grilled Shrimp item.", Price = 115000m, PriceUsd = 4.60m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-09-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-09-menu-03", BoothId = "booth-09", Name = "Combo Hải Sản", NameEn = "Seafood Combo", Description = "Combo hải sản nướng thập cẩm.", DescriptionEn = "Seafood Combo item.", Price = 149000m, PriceUsd = 5.96m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-09-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 10: Cà Phê & Trà Sữa ---
            menuItems.Add(new BoothMenuItem { Id = "booth-10-menu-01", BoothId = "booth-10", Name = "Cà Phê Sữa Đá", NameEn = "Iced Milk Coffee", Description = "Cà phê sữa đá truyền thống.", DescriptionEn = "Iced Milk Coffee item.", Price = 30000m, PriceUsd = 1.20m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-10-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-10-menu-02", BoothId = "booth-10", Name = "Trà Sữa Trân Châu", NameEn = "Bubble Milk Tea", Description = "Trà sữa trân châu đường đen.", DescriptionEn = "Bubble Milk Tea item.", Price = 42000m, PriceUsd = 1.68m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-10-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-10-menu-03", BoothId = "booth-10", Name = "Combo Đồ Uống", NameEn = "Drink Combo", Description = "Combo cà phê và trà sữa.", DescriptionEn = "Drink Combo item.", Price = 70000m, PriceUsd = 2.80m, ImageUrl = "http://192.168.1.237:5151/uploads/menu/booth-10-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            db.BoothMenuItems.AddRange(menuItems);
            await db.SaveChangesAsync();
        }

        // 5. Bảng BoothMenuItemTranslations (Bản dịch món ăn) - 3 ví dụ
        // 5. Bảng BoothMenuItemTranslations (Bản dịch món ăn) - Full 90 bản ghi từ poi.db
        if (!await db.BoothMenuItemTranslations.AnyAsync())
        {
            var itemTranslations = new List<BoothMenuItemTranslationLocal>();

            // Booth 01 - Phở Hà Nội
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "vi",
                Name = "Phở Hà Nội Phở đặc biệt",
                Description = "Phở đặc biệt của Phở Hà Nội.",
                CurrencyCode = "VND",
                LocalizedPrice = 65000m,
                PriceText = "65.000đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "en",
                Name = "Hanoi Pho Special",
                Description = "Hanoi Special Pho with premium beef cuts.",
                CurrencyCode = "USD",
                LocalizedPrice = 2.99m,
                PriceText = "$2.99"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "zh",
                Name = "河内特制牛肉粉",
                Description = "河内正宗特制牛肉粉，选用上等牛肉。",
                CurrencyCode = "CNY",
                LocalizedPrice = 18.0m,
                PriceText = "¥18.0"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "ja",
                Name = "ハノイ特製フォー",
                Description = "ハノイの特製牛肉フォー、最高級の牛肉を使用。",
                CurrencyCode = "JPY",
                LocalizedPrice = 390m,
                PriceText = "¥390"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-01-menu-01",
                LanguageCode = "ko",
                Name = "하노이 스페셜 쌀국수",
                Description = "최상급 소고기를 곁들인 하노이 전통 스페셜 쌀국수.",
                CurrencyCode = "KRW",
                LocalizedPrice = 3500m,
                PriceText = "₩3,500"
            });

            // Booth 02 - Bún Bò Huế
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "vi",
                Name = "Bún Bò Huế Đặc Biệt",
                Description = "Bún bò đặc biệt của Bún Bò Huế, hương vị đậm đà.",
                CurrencyCode = "VND",
                LocalizedPrice = 75000m,
                PriceText = "75.000đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "en",
                Name = "Special Hue Beef Noodles",
                Description = "Rich and spicy Hue style beef noodle soup.",
                CurrencyCode = "USD",
                LocalizedPrice = 3.20m,
                PriceText = "$3.20"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "zh",
                Name = "顺化牛肉粉特制版",
                Description = "顺化风味特制牛肉米粉，配料丰富。",
                CurrencyCode = "CNY",
                LocalizedPrice = 19.8m,
                PriceText = "¥19.8"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "ja",
                Name = "フエ名物牛肉麺スペシャル",
                Description = "フエ風の特製牛肉麺、具だくさんです。",
                CurrencyCode = "JPY",
                LocalizedPrice = 420m,
                PriceText = "¥420"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-02-menu-01",
                LanguageCode = "ko",
                Name = "후에식 소고기 쌀국수 스페셜",
                Description = "후에 스타일의 특별 소고기 국수로 다양한 토핑이 들어갑니다.",
                CurrencyCode = "KRW",
                LocalizedPrice = 3600m,
                PriceText = "₩3,600"
            });

            // Booth 03 - Cơm Tấm Sài Gòn
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "vi",
                Name = "Cơm Tấm Sườn Bì Chả",
                Description = "Cơm tấm đặc sản Sài Gòn với đầy đủ topping.",
                CurrencyCode = "VND",
                LocalizedPrice = 55000m,
                PriceText = "55.000đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "en",
                Name = "Saigon Broken Rice Combo",
                Description = "Famous broken rice with grilled pork chop, skin and egg meatloaf.",
                CurrencyCode = "USD",
                LocalizedPrice = 2.50m,
                PriceText = "$2.50"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "zh",
                Name = "西贡碎米饭全套餐",
                Description = "著名的碎米饭，搭配烤排骨、肉丝和肉饼。",
                CurrencyCode = "CNY",
                LocalizedPrice = 16.5m,
                PriceText = "¥16.5"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "ja",
                Name = "サイゴン名物コムタム",
                Description = "豚肉のグリルと卵のミートローフを添えたサイゴン風砕き米ご飯。",
                CurrencyCode = "JPY",
                LocalizedPrice = 350m,
                PriceText = "¥350"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-03-menu-01",
                LanguageCode = "ko",
                Name = "사이공 껌땀 콤보",
                Description = "그릴 돼지갈비와 계란 찜이 포함된 사이공 스타일 깨진 쌀밥.",
                CurrencyCode = "KRW",
                LocalizedPrice = 3000m,
                PriceText = "₩3,000"
            });

            // Booth 04 - Bánh Mì Việt
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "vi",
                Name = "Bánh Mì Thịt Nướng",
                Description = "Bánh mì giòn kẹp thịt nướng thơm ngon.",
                CurrencyCode = "VND",
                LocalizedPrice = 30000m,
                PriceText = "30.000đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "en",
                Name = "Grilled Pork Banh Mi",
                Description = "Crunchy baguette filled with flavorful grilled pork.",
                CurrencyCode = "USD",
                LocalizedPrice = 1.30m,
                PriceText = "$1.30"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "zh",
                Name = "越式烤肉夹心面包",
                Description = "酥脆的面包夹着香脆的烤肉。",
                CurrencyCode = "CNY",
                LocalizedPrice = 9.0m,
                PriceText = "¥9.0"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "ja",
                Name = "豚焼き肉バインミー",
                Description = "香ばしい焼き肉を挟んだカリカリのフランスパン。",
                CurrencyCode = "JPY",
                LocalizedPrice = 180m,
                PriceText = "¥180"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-04-menu-01",
                LanguageCode = "ko",
                Name = "돼지구이 반미",
                Description = "고소한 돼지구이가 듬뿍 들어간 바삭한 바게트.",
                CurrencyCode = "KRW",
                LocalizedPrice = 1600m,
                PriceText = "₩1,600"
            });

            // Booth 05 - Chè Ba Miền
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "vi",
                Name = "Chè Đậu Xanh",
                Description = "Chè đậu xanh ngọt mát, thanh nhiệt.",
                CurrencyCode = "VND",
                LocalizedPrice = 20000m,
                PriceText = "20.000đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "en",
                Name = "Mung Bean Sweet Soup",
                Description = "Refreshing and cooling mung bean dessert.",
                CurrencyCode = "USD",
                LocalizedPrice = 0.90m,
                PriceText = "$0.90"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-05-menu-01",
                LanguageCode = "zh",
                Name = "绿豆甜羹",
                Description = "清凉解暑的甜绿豆汤。",
                CurrencyCode = "CNY",
                LocalizedPrice = 6.0m,
                PriceText = "¥6.0"
            });

            // Booth 06 - Nem Nướng Đà Lạt
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "vi",
                Name = "Nem Nướng Đà Lạt Phần",
                Description = "Suất nem nướng đặc sản Đà Lạt.",
                CurrencyCode = "VND",
                LocalizedPrice = 60000m,
                PriceText = "60.000đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-06-menu-01",
                LanguageCode = "en",
                Name = "Dalat Grilled Pork Set",
                Description = "Authentic Dalat grilled pork rolls served with herbs.",
                CurrencyCode = "USD",
                LocalizedPrice = 2.60m,
                PriceText = "$2.60"
            });

            // Booth 07 - Bánh Xèo Miền Tây
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "vi",
                Name = "Bánh Xèo Tôm Thịt",
                Description = "Bánh xèo giòn tan nhân tôm thịt.",
                CurrencyCode = "VND",
                LocalizedPrice = 45000m,
                PriceText = "45.000đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-07-menu-01",
                LanguageCode = "en",
                Name = "Shrimp & Pork Pancake",
                Description = "Crispy Vietnamese pancake with shrimp and pork.",
                CurrencyCode = "USD",
                LocalizedPrice = 2.00m,
                PriceText = "$2.00"
            });

            // Booth 08 - Gỏi Cuốn Tươi
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "vi",
                Name = "Gỏi Cuốn Tôm Thịt",
                Description = "Cuốn tôm thịt tươi ngon mỗi ngày.",
                CurrencyCode = "VND",
                LocalizedPrice = 10000m,
                PriceText = "10.000đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-08-menu-01",
                LanguageCode = "en",
                Name = "Shrimp & Pork Spring Rolls",
                Description = "Fresh rice paper rolls with shrimp and pork.",
                CurrencyCode = "USD",
                LocalizedPrice = 0.45m,
                PriceText = "$0.45"
            });

            // Booth 09 - Hải Sản Nướng
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "vi",
                Name = "Mực Nướng Sa Tế",
                Description = "Mực nướng cay nồng vị sa tế.",
                CurrencyCode = "VND",
                LocalizedPrice = 120000m,
                PriceText = "120.000đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-09-menu-01",
                LanguageCode = "en",
                Name = "Sate Grilled Squid",
                Description = "Grilled squid with spicy sate sauce.",
                CurrencyCode = "USD",
                LocalizedPrice = 5.20m,
                PriceText = "$5.20"
            });

            // Booth 10 - Cà Phê & Trà Sữa
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "vi",
                Name = "Cà Phê Sữa Đá",
                Description = "Cà phê sữa đá pha phin đậm chất Việt.",
                CurrencyCode = "VND",
                LocalizedPrice = 25000m,
                PriceText = "25.000đ"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "en",
                Name = "Vietnamese Iced Milk Coffee",
                Description = "Traditional drip coffee with condensed milk.",
                CurrencyCode = "USD",
                LocalizedPrice = 1.10m,
                PriceText = "$1.10"
            });
            itemTranslations.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = "booth-10-menu-01",
                LanguageCode = "zh",
                Name = "越式冰奶咖啡",
                Description = "传统滴漏咖啡，搭配炼乳。",
                CurrencyCode = "CNY",
                LocalizedPrice = 7.5m,
                PriceText = "¥7.5"
            });
            db.BoothMenuItemTranslations.AddRange(itemTranslations);
            await db.SaveChangesAsync();
        }

        // 6. Bảng PlaybackLogs (Nhật ký) - 3 ví dụ (Đã có IsSynced)
        // 6. Bảng PlaybackLogs (Nhật ký) - Đầy đủ 15 bản ghi từ poi.db
        if (!await db.PlaybackLogs.AnyAsync())
        {
            db.PlaybackLogs.AddRange(new List<PlaybackLog>
    {
        new PlaybackLog { BoothId = "booth-01", TriggerType = "QR", Language = "vi", PlayedAtUtc = DateTime.Parse("2026-04-08T08:00:00Z"), DurationSeconds = 12, Lat = 10.7768, Lng = 106.7008, IsCompleted = true, SessionId = "session-001", IsSynced = true },
        new PlaybackLog { BoothId = "booth-01", TriggerType = "GPS", Language = "vi", PlayedAtUtc = DateTime.Parse("2026-04-08T08:10:00Z"), DurationSeconds = 10, Lat = 10.77683, Lng = 106.70081, IsCompleted = true, SessionId = "session-002", IsSynced = true },
        new PlaybackLog { BoothId = "booth-02", TriggerType = "QR", Language = "en", PlayedAtUtc = DateTime.Parse("2026-04-08T08:20:00Z"), DurationSeconds = 14, Lat = 10.77698, Lng = 106.7008, IsCompleted = true, SessionId = "session-003", IsSynced = true },
        new PlaybackLog { BoothId = "booth-03", TriggerType = "Manual", Language = "en", PlayedAtUtc = DateTime.Parse("2026-04-08T08:35:00Z"), DurationSeconds = 9, Lat = 10.77716, Lng = 106.7008, IsCompleted = true, SessionId = "session-004", IsSynced = true },
        new PlaybackLog { BoothId = "booth-04", TriggerType = "QR", Language = "vi", PlayedAtUtc = DateTime.Parse("2026-04-08T09:00:00Z"), DurationSeconds = 11, Lat = 10.77734, Lng = 106.7008, IsCompleted = true, SessionId = "session-005", IsSynced = true },
        new PlaybackLog { BoothId = "booth-05", TriggerType = "GPS", Language = "vi", PlayedAtUtc = DateTime.Parse("2026-04-08T09:10:00Z"), DurationSeconds = 13, Lat = 10.77752, Lng = 106.7008, IsCompleted = true, SessionId = "session-006", IsSynced = true },
        new PlaybackLog { BoothId = "booth-06", TriggerType = "QR", Language = "vi", PlayedAtUtc = DateTime.Parse("2026-04-08T09:30:00Z"), DurationSeconds = 12, Lat = 10.7768, Lng = 106.70102, IsCompleted = true, SessionId = "session-007", IsSynced = true },
        new PlaybackLog { BoothId = "booth-07", TriggerType = "GPS", Language = "vi", PlayedAtUtc = DateTime.Parse("2026-04-08T10:00:00Z"), DurationSeconds = 15, Lat = 10.77698, Lng = 106.70102, IsCompleted = true, SessionId = "session-008", IsSynced = true },
        new PlaybackLog { BoothId = "booth-08", TriggerType = "QR", Language = "zh", PlayedAtUtc = DateTime.Parse("2026-04-08T10:20:00Z"), DurationSeconds = 8, Lat = 10.77716, Lng = 106.70102, IsCompleted = true, SessionId = "session-009", IsSynced = true },
        new PlaybackLog { BoothId = "booth-09", TriggerType = "Manual", Language = "en", PlayedAtUtc = DateTime.Parse("2026-04-08T10:40:00Z"), DurationSeconds = 17, Lat = 10.77734, Lng = 106.70102, IsCompleted = true, SessionId = "session-010", IsSynced = true },
        new PlaybackLog { BoothId = "booth-10", TriggerType = "QR", Language = "vi", PlayedAtUtc = DateTime.Parse("2026-04-08T11:00:00Z"), DurationSeconds = 7, Lat = 10.77752, Lng = 106.70102, IsCompleted = true, SessionId = "session-011", IsSynced = true },
        new PlaybackLog { BoothId = "booth-01", TriggerType = "QR", Language = "fr", PlayedAtUtc = DateTime.Parse("2026-04-08T11:10:00Z"), DurationSeconds = 9, Lat = 10.7768, Lng = 106.7008, IsCompleted = true, SessionId = "session-012", IsSynced = true },
        new PlaybackLog { BoothId = "booth-02", TriggerType = "GPS", Language = "es", PlayedAtUtc = DateTime.Parse("2026-04-08T11:20:00Z"), DurationSeconds = 10, Lat = 10.77698, Lng = 106.7008, IsCompleted = true, SessionId = "session-013", IsSynced = true },
        new PlaybackLog { BoothId = "booth-03", TriggerType = "QR", Language = "it", PlayedAtUtc = DateTime.Parse("2026-04-08T11:30:00Z"), DurationSeconds = 11, Lat = 10.77716, Lng = 106.7008, IsCompleted = true, SessionId = "session-014", IsSynced = true },
        new PlaybackLog { BoothId = "booth-04", TriggerType = "QR", Language = "ru", PlayedAtUtc = DateTime.Parse("2026-04-08T11:40:00Z"), DurationSeconds = 12, Lat = 10.77734, Lng = 106.7008, IsCompleted = true, SessionId = "session-015", IsSynced = true }
    });
            await db.SaveChangesAsync();
        }
    }
}