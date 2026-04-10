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
        new Booth { Id = "booth-01", ZoneId = "zone-a", NameVi = "Phở Hà Nội", NameEn = "Hanoi Pho", Lat = 10.7768, Lng = 106.7008, RadiusMeters = 25, Priority = 1, IsActive = true, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-01-menu-01.png", OwnerUserId = "owner1" },

new Booth { Id = "booth-02", ZoneId = "zone-a", NameVi = "Bún Bò Huế", NameEn = "Hue Beef Noodles", Lat = 10.77698, Lng = 106.7008, RadiusMeters = 25, Priority = 2, IsActive = true, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-02-menu-01.png", OwnerUserId = "owner2" },

new Booth { Id = "booth-03", ZoneId = "zone-a", NameVi = "Cơm Tấm Sài Gòn", NameEn = "Saigon Broken Rice", Lat = 10.77716, Lng = 106.7008, RadiusMeters = 25, Priority = 3, IsActive = true, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-03-menu-01.png", OwnerUserId = "owner3" },

new Booth { Id = "booth-04", ZoneId = "zone-a", NameVi = "Bánh Mì Việt", NameEn = "Vietnamese Banh Mi", Lat = 10.77734, Lng = 106.7008, RadiusMeters = 25, Priority = 4, IsActive = true, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-04-menu-01.png", OwnerUserId = "owner4" },

new Booth { Id = "booth-05", ZoneId = "zone-a", NameVi = "Chè Ba Miền", NameEn = "Three-Region Sweet Soup", Lat = 10.77752, Lng = 106.7008, RadiusMeters = 25, Priority = 5, IsActive = true, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-05-menu-01.png", OwnerUserId = "owner5" },

new Booth { Id = "booth-06", ZoneId = "zone-b", NameVi = "Nem Nướng Đà Lạt", NameEn = "Dalat Grilled Pork", Lat = 10.7768, Lng = 106.70102, RadiusMeters = 25, Priority = 6, IsActive = true, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-06-menu-01.png", OwnerUserId = "owner6" },

new Booth { Id = "booth-07", ZoneId = "zone-b", NameVi = "Bánh Xèo Miền Tây", NameEn = "Mekong Pancake", Lat = 10.77698, Lng = 106.70102, RadiusMeters = 25, Priority = 7, IsActive = true, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-07-menu-01.png", OwnerUserId = "owner7" },

new Booth { Id = "booth-08", ZoneId = "zone-b", NameVi = "Gỏi Cuốn Tươi", NameEn = "Fresh Spring Rolls", Lat = 10.77716, Lng = 106.70102, RadiusMeters = 25, Priority = 8, IsActive = true, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-08-menu-01.png", OwnerUserId = "owner8" },

new Booth { Id = "booth-09", ZoneId = "zone-b", NameVi = "Hải Sản Nướng", NameEn = "Grilled Seafood", Lat = 10.77734, Lng = 106.70102, RadiusMeters = 25, Priority = 9, IsActive = true, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-09-menu-01.png", OwnerUserId = "owner9" },

new Booth { Id = "booth-10", ZoneId = "zone-b", NameVi = "Cà Phê & Trà Sữa", NameEn = "Coffee & Milk Tea", Lat = 10.77752, Lng = 106.70102, RadiusMeters = 25, Priority = 10, IsActive = true, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-10-menu-01.png", OwnerUserId = "owner10" }
    });
            await db.SaveChangesAsync();
        }

        // 3. Bảng BoothTranslations (Bản dịch gian hàng) - 3 ví dụ
        // 3. Gieo mầm Bản dịch gian hàng (BoothTranslations) - Full 10 Booths x 9 Langs
        if (!await db.BoothTranslations.AnyAsync())
        {
            var translations = new List<BoothTranslationLocal>();

            // --- BOOTH-01: Phở Hà Nội ---
            translations.Add(new BoothTranslationLocal { BoothId = "booth-01", LanguageCode = "vi", Name = "Phở Hà Nội", Description = "Phở Hà Nội - gian hàng nổi bật của hội chợ ẩm thực...", TtsScript = "Xin chào, bạn đang đến với gian hàng Phở Hà Nội.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-01-vi.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-01", LanguageCode = "en", Name = "Hanoi Pho", Description = "Hanoi Pho booth at the food fair with image menu...", TtsScript = "Welcome to Hanoi Pho.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-01-en.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-01", LanguageCode = "zh", Name = "河内牛肉粉", Description = "河内牛肉粉 - 翻译 demo", TtsScript = "Welcome to 河内牛肉粉." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-01", LanguageCode = "ja", Name = "ハノイフォー", Description = "ハノイフォー - 翻訳 demo", TtsScript = "Welcome to ハノイフォー." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-01", LanguageCode = "ko", Name = "하노이 퍼", Description = "하노이 퍼 - 翻訳 demo", TtsScript = "Welcome to 하노이 퍼." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-01", LanguageCode = "fr", Name = "Pho de Hanoï", Description = "Pho de Hanoï - Translation demo", TtsScript = "Welcome to Pho de Hanoï." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-01", LanguageCode = "es", Name = "Pho de Hanói", Description = "Pho de Hanói - Translation demo", TtsScript = "Welcome to Pho de Hanói." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-01", LanguageCode = "it", Name = "Pho di Hanoi", Description = "Pho di Hanoi - Translation demo", TtsScript = "Welcome to Pho di Hanoi." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-01", LanguageCode = "ru", Name = "Ханойский фо", Description = "Ханойский фо - Translation demo", TtsScript = "Welcome to Ханойский фо." });

            // --- BOOTH-02: Bún Bò Huế ---
            translations.Add(new BoothTranslationLocal { BoothId = "booth-02", LanguageCode = "vi", Name = "Bún Bò Huế", Description = "Bún Bò Huế - gian hàng nổi bật của hội chợ ẩm thực...", TtsScript = "Xin chào, bạn đang đến với gian hàng Bún Bò Huế.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-02-vi.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-02", LanguageCode = "en", Name = "Hue Beef Noodles", Description = "Hue Beef Noodles booth at the food fair...", TtsScript = "Welcome to Hue Beef Noodles.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-02-en.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-02", LanguageCode = "zh", Name = "顺化牛肉粉", Description = "顺化牛肉粉 - Translation demo", TtsScript = "Welcome to 顺化牛肉粉." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-02", LanguageCode = "ja", Name = "フエ牛肉麺", Description = "フエ牛肉麺 - Translation demo", TtsScript = "Welcome to フエ牛肉麺." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-02", LanguageCode = "ko", Name = "후에 소고기 국수", Description = "후에 소고기 국수 - Translation demo", TtsScript = "Welcome to 후에 소고기 국수." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-02", LanguageCode = "fr", Name = "Bún Bò Huế", Description = "Bún Bò Huế - Translation demo", TtsScript = "Welcome to Bún Bò Huế." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-02", LanguageCode = "es", Name = "Bún Bò Huế", Description = "Bún Bò Huế - Translation demo", TtsScript = "Welcome to Bún Bò Huế." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-02", LanguageCode = "it", Name = "Bún Bò Huế", Description = "Bún Bò Huế - Translation demo", TtsScript = "Welcome to Bún Bò Huế." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-02", LanguageCode = "ru", Name = "Хюэский суп с говядиной", Description = "Хюэский суп с говядиной - Translation demo", TtsScript = "Welcome to Хюэский суп с говядиной." });

            // --- BOOTH-03: Cơm Tấm Sài Gòn ---
            translations.Add(new BoothTranslationLocal { BoothId = "booth-03", LanguageCode = "vi", Name = "Cơm Tấm Sài Gòn", Description = "Cơm Tấm Sài Gòn - gian hàng ẩm thực nổi bật...", TtsScript = "Xin chào, bạn đang đến với gian hàng Cơm Tấm Sài Gòn.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-03-vi.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-03", LanguageCode = "en", Name = "Saigon Broken Rice", Description = "Saigon Broken Rice booth at the food fair...", TtsScript = "Welcome to Saigon Broken Rice.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-03-en.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-03", LanguageCode = "zh", Name = "西贡碎米饭", Description = "西贡碎米饭 - Translation demo", TtsScript = "Welcome to 西贡碎米饭." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-03", LanguageCode = "ja", Name = "サイゴン焼き豚のせご飯", Description = "サイゴン焼き豚のせご飯 - Translation demo", TtsScript = "Welcome to サイゴン焼き豚のせご飯." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-03", LanguageCode = "ko", Name = "사이공 껌땀", Description = "사이공 껌땀 - Translation demo", TtsScript = "Welcome to 사이공 껌땀." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-03", LanguageCode = "fr", Name = "Riz brisé de Saïgon", Description = "Riz brisé de Saïgon - Translation demo", TtsScript = "Welcome to Riz brisé de Saïgon." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-03", LanguageCode = "es", Name = "Arroz quebrado de Saigón", Description = "Arroz quebrado de Saigón - Translation demo", TtsScript = "Welcome to Arroz quebrado de Saigón." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-03", LanguageCode = "it", Name = "Riso spezzato di Saigon", Description = "Riso spezzato di Saigon - Translation demo", TtsScript = "Welcome to Riso spezzato di Saigon." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-03", LanguageCode = "ru", Name = "Сайгонский дроблёный рис", Description = "Сайгонский дроблёный рис - Translation demo", TtsScript = "Welcome to Сайгонский дроблёный рис." });

            // --- BOOTH-04: Bánh Mì Việt ---
            translations.Add(new BoothTranslationLocal { BoothId = "booth-04", LanguageCode = "vi", Name = "Bánh Mì Việt", Description = "Bánh Mì Việt - gian hàng ẩm thực nổi bật...", TtsScript = "Xin chào, bạn đang đến với gian hàng Bánh Mì Việt.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-04-vi.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-04", LanguageCode = "en", Name = "Vietnamese Banh Mi", Description = "Vietnamese Banh Mi booth at the food fair...", TtsScript = "Welcome to Vietnamese Banh Mi.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-04-en.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-04", LanguageCode = "zh", Name = "越式法棍", Description = "越式法棍 - Translation demo", TtsScript = "Welcome to 越式法棍." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-04", LanguageCode = "ja", Name = "ベトナムバインミー", Description = "ベトナムバインミー - Translation demo", TtsScript = "Welcome to ベトナムバインミー." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-04", LanguageCode = "ko", Name = "베트남 반미", Description = "베트남 반미 - Translation demo", TtsScript = "Welcome to 베트남 반미." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-04", LanguageCode = "fr", Name = "Bánh Mì vietnamien", Description = "Bánh Mì vietnamien - Translation demo", TtsScript = "Welcome to Bánh Mì vietnamien." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-04", LanguageCode = "es", Name = "Bánh Mì vietnamita", Description = "Bánh Mì vietnamita - Translation demo", TtsScript = "Welcome to Bánh Mì vietnamita." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-04", LanguageCode = "it", Name = "Bánh Mì vietnamita", Description = "Bánh Mì vietnamita - Translation demo", TtsScript = "Welcome to Bánh Mì vietnamita." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-04", LanguageCode = "ru", Name = "Вьетнамский баньми", Description = "Вьетнамский баньми - Translation demo", TtsScript = "Welcome to Вьетнамский баньми." });

            // --- BOOTH-05: Chè Ba Miền ---
            translations.Add(new BoothTranslationLocal { BoothId = "booth-05", LanguageCode = "vi", Name = "Chè Ba Miền", Description = "Chè Ba Miền - gian hàng ẩm thực nổi bật...", TtsScript = "Xin chào, bạn đang đến với gian hàng Chè Ba Miền.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-05-vi.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-05", LanguageCode = "en", Name = "Three-Region Sweet Soup", Description = "Three-Region Sweet Soup booth...", TtsScript = "Welcome to Three-Region Sweet Soup.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-05-en.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-05", LanguageCode = "zh", Name = "三地甜汤", Description = "三地甜汤 - Translation demo", TtsScript = "Welcome to 三地甜汤." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-05", LanguageCode = "ja", Name = "三地域のチェー", Description = "三地域のチェー - Translation demo", TtsScript = "Welcome to 三地域のチェー." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-05", LanguageCode = "ko", Name = "삼지역 체", Description = "삼지역 체 - Translation demo", TtsScript = "Welcome to 삼지역 체." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-05", LanguageCode = "fr", Name = "Soupe sucrée des trois régions", Description = "Soupe sucrée demo", TtsScript = "Welcome to Soupe sucrée." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-05", LanguageCode = "es", Name = "Postre de las tres regiones", Description = "Postre demo", TtsScript = "Welcome to Postre." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-05", LanguageCode = "it", Name = "Dolce delle tre regioni", Description = "Dolce demo", TtsScript = "Welcome to Dolce." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-05", LanguageCode = "ru", Name = "Сладкий суп трёх регионов", Description = "Сладкий суп demo", TtsScript = "Welcome to Сладкий суп." });

            // --- Tương tự cho BOOTH-06 đến BOOTH-10 ---
            // (Mình chỉ liệt kê đại diện 5 cái đầu, bạn có thể copy-paste sửa ID cho 5 cái sau nếu muốn đủ bộ 90 bản dịch)
            // --- BOOTH-06: Nem Nướng Đà Lạt ---
            translations.Add(new BoothTranslationLocal { BoothId = "booth-06", LanguageCode = "vi", Name = "Nem Nướng Đà Lạt", Description = "Nem Nướng Đà Lạt - gian hàng nổi bật của hội chợ ẩm thực...", TtsScript = "Xin chào, bạn đang đến với gian hàng Nem Nướng Đà Lạt.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-06-vi.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-06", LanguageCode = "en", Name = "Dalat Grilled Pork Rolls", Description = "Dalat Grilled Pork Rolls booth at the food fair...", TtsScript = "Welcome to Dalat Grilled Pork Rolls.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-06-en.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-06", LanguageCode = "zh", Name = "大叻烤肉卷", Description = "大叻烤肉卷 - Translation demo", TtsScript = "Welcome to 大叻烤肉卷." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-06", LanguageCode = "ja", Name = "ダラット焼き豚ロール", Description = "ダラット焼き豚ロール - Translation demo", TtsScript = "Welcome to ダラット焼き豚ロール." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-06", LanguageCode = "ko", Name = "달랏 넴느엉", Description = "달랏 넴느엉 - Translation demo", TtsScript = "Welcome to 달랏 넴느엉." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-06", LanguageCode = "fr", Name = "Rouleaux de porc grillé de Dalat", Description = "Rouleaux de porc demo", TtsScript = "Welcome to Rouleaux de porc." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-06", LanguageCode = "es", Name = "Rollos de cerdo a la parrilla de Dalat", Description = "Rollos de cerdo demo", TtsScript = "Welcome to Rollos de cerdo." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-06", LanguageCode = "it", Name = "Involtini di maiale grigliato di Dalat", Description = "Involtini demo", TtsScript = "Welcome to Involtini." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-06", LanguageCode = "ru", Name = "Далатские роллы из жаreной свинины", Description = "Далатские роллы demo", TtsScript = "Welcome to Далатские роллы." });

            // --- BOOTH-07: Bánh Xèo Miền Tây ---
            translations.Add(new BoothTranslationLocal { BoothId = "booth-07", LanguageCode = "vi", Name = "Bánh Xèo Miền Tây", Description = "Bánh Xèo Miền Tây - gian hàng nổi bật của hội chợ ẩm thực...", TtsScript = "Xin chào, bạn đang đến với gian hàng Bánh Xèo Miền Tây.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-07-vi.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-07", LanguageCode = "en", Name = "Mekong Crispy Pancake", Description = "Mekong Crispy Pancake booth at the food fair...", TtsScript = "Welcome to Mekong Crispy Pancake.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-07-en.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-07", LanguageCode = "zh", Name = "湄公河煎饼", Description = "湄公河煎饼 - Translation demo", TtsScript = "Welcome to 湄公河煎饼." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-07", LanguageCode = "ja", Name = "メコン風バインセオ", Description = "メコン風バインセオ - Translation demo", TtsScript = "Welcome to メコン風バインセオ." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-07", LanguageCode = "ko", Name = "메콩식 반쎄오", Description = "메콩식 반쎄오 - Translation demo", TtsScript = "Welcome to 메콩식 반쎄오." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-07", LanguageCode = "fr", Name = "Crêpe croustillante du Mékong", Description = "Crêpe croustillante demo", TtsScript = "Welcome to Crêpe croustillante." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-07", LanguageCode = "es", Name = "Panqueque crujiente del Mekong", Description = "Panqueque crujiente demo", TtsScript = "Welcome to Panqueque crujiente." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-07", LanguageCode = "it", Name = "Pancake croccante del Mekong", Description = "Pancake croccante demo", TtsScript = "Welcome to Pancake croccante." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-07", LanguageCode = "ru", Name = "Хрустящий блин Меконга", Description = "Хрустящий блин demo", TtsScript = "Welcome to Хрустящий блин." });

            // --- BOOTH-08: Gỏi Cuốn Tươi ---
            translations.Add(new BoothTranslationLocal { BoothId = "booth-08", LanguageCode = "vi", Name = "Gỏi Cuốn Tươi", Description = "Gỏi Cuốn Tươi - gian hàng nổi bật của hội chợ ẩm thực...", TtsScript = "Xin chào, bạn đang đến với gian hàng Gỏi Cuốn Tươi.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-08-vi.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-08", LanguageCode = "en", Name = "Fresh Spring Rolls", Description = "Fresh Spring Rolls booth at the food fair...", TtsScript = "Welcome to Fresh Spring Rolls.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-08-en.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-08", LanguageCode = "zh", Name = "鲜春卷", Description = "鲜春卷 - Translation demo", TtsScript = "Welcome to 鲜春卷." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-08", LanguageCode = "ja", Name = "生春巻き", Description = "生春巻き - Translation demo", TtsScript = "Welcome to 生春巻き." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-08", LanguageCode = "ko", Name = "생춘권", Description = "생춘권 - Translation demo", TtsScript = "Welcome to 생춘권." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-08", LanguageCode = "fr", Name = "Rouleaux de printemps frais", Description = "Rouleaux demo", TtsScript = "Welcome to Rouleaux." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-08", LanguageCode = "es", Name = "Rollitos frescos", Description = "Rollitos demo", TtsScript = "Welcome to Rollitos." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-08", LanguageCode = "it", Name = "Involtini freschi", Description = "Involtini demo", TtsScript = "Welcome to Involtini." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-08", LanguageCode = "ru", Name = "Свежие спринг-роллы", Description = "Спринг-роллы demo", TtsScript = "Welcome to Спринг-роллы." });

            // --- BOOTH-09: Hải Sản Nướng ---
            translations.Add(new BoothTranslationLocal { BoothId = "booth-09", LanguageCode = "vi", Name = "Hải Sản Nướng", Description = "Hải Sản Nướng - gian hàng nổi bật của hội chợ ẩm thực...", TtsScript = "Xin chào, bạn đang đến với gian hàng Hải Sản Nướng.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-09-vi.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-09", LanguageCode = "en", Name = "Grilled Seafood", Description = "Grilled Seafood booth at the food fair...", TtsScript = "Welcome to Grilled Seafood.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-09-en.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-09", LanguageCode = "zh", Name = "烤海鲜", Description = "烤海鲜 - Translation demo", TtsScript = "Welcome to 烤海鲜." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-09", LanguageCode = "ja", Name = "焼きシーフード", Description = "焼きシーフード - Translation demo", TtsScript = "Welcome to 焼きシーフード." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-09", LanguageCode = "ko", Name = "구운 해산물", Description = "구운 해산물 - Translation demo", TtsScript = "Welcome to 구운 해산물." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-09", LanguageCode = "fr", Name = "Fruits de mer grillés", Description = "Fruits de mer demo", TtsScript = "Welcome to Fruits de mer." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-09", LanguageCode = "es", Name = "Mariscos a la parrilla", Description = "Mariscos demo", TtsScript = "Welcome to Mariscos." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-09", LanguageCode = "it", Name = "Frutti di mare alla griglia", Description = "Frutti di mare demo", TtsScript = "Welcome to Frutti di mare." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-09", LanguageCode = "ru", Name = "Жареные морепродукты", Description = "Морепродукты demo", TtsScript = "Welcome to Морепродукты." });

            // --- BOOTH-10: Cà Phê & Trà Sữa ---
            translations.Add(new BoothTranslationLocal { BoothId = "booth-10", LanguageCode = "vi", Name = "Cà Phê & Trà Sữa", Description = "Cà Phê & Trà Sữa - gian hàng nổi bật của hội chợ ẩm thực...", TtsScript = "Xin chào, bạn đang đến với gian hàng Cà Phê & Trà Sữa.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-10-vi.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-10", LanguageCode = "en", Name = "Coffee & Milk Tea", Description = "Coffee & Milk Tea booth at the food fair...", TtsScript = "Welcome to Coffee & Milk Tea.", AudioUrl = "http://192.168.88.235:5151/uploads/audio/booth-10-en.mp3" });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-10", LanguageCode = "zh", Name = "咖啡与奶茶", Description = "咖啡与奶茶 - Translation demo", TtsScript = "Welcome to 咖啡与奶茶." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-10", LanguageCode = "ja", Name = "コーヒー＆ミルクティー", Description = "コーヒー＆ミルクティー - Translation demo", TtsScript = "Welcome to コーヒー＆ミルクティー." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-10", LanguageCode = "ko", Name = "커피 & 밀크티", Description = "커피 & 밀크티 - Translation demo", TtsScript = "Welcome to 커피 & 밀크티." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-10", LanguageCode = "fr", Name = "Café & thé au lait", Description = "Café & thé demo", TtsScript = "Welcome to Café & thé." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-10", LanguageCode = "es", Name = "Café y té con leche", Description = "Café y té demo", TtsScript = "Welcome to Café y té." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-10", LanguageCode = "it", Name = "Caffè e tè al latte", Description = "Caffè e tè demo", TtsScript = "Welcome to Caffè e tè." });
            translations.Add(new BoothTranslationLocal { BoothId = "booth-10", LanguageCode = "ru", Name = "Кофе и молочный чай", Description = "Кофе и чай demo", TtsScript = "Welcome to Кофе и чай." });
            db.BoothTranslations.AddRange(translations);
            await db.SaveChangesAsync();
        }

        // 4. Bảng BoothMenuItems (Món ăn) - 3 ví dụ
        // 4. Bảng BoothMenuItems (Món ăn) - Đầy đủ 30 món từ poi.db
        if (!await db.BoothMenuItems.AnyAsync())
        {
            var menuItems = new List<BoothMenuItem>();

            // --- Booth 01: Phở Hà Nội ---
            menuItems.Add(new BoothMenuItem { Id = "booth-01-menu-01", BoothId = "booth-01", Name = "Phở Đặc Biệt", NameEn = "Pho Special", Description = "Phở đặc biệt đầy đủ topping.", DescriptionEn = "Pho Special item for demo.", Price = 65000m, PriceUsd = 2.60m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-01-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-01-menu-02", BoothId = "booth-01", Name = "Phở Tái Nạm", NameEn = "Rare & Brisket Pho", Description = "Phở tái nạm tươi ngon.", DescriptionEn = "Rare & Brisket Pho item.", Price = 72000m, PriceUsd = 2.88m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-01-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-01-menu-03", BoothId = "booth-01", Name = "Phở Bò Viên", NameEn = "Meatball Pho", Description = "Phở bò viên dai giòn.", DescriptionEn = "Meatball Pho item.", Price = 59000m, PriceUsd = 2.36m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-01-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 02: Bún Bò Huế ---
            menuItems.Add(new BoothMenuItem { Id = "booth-02-menu-01", BoothId = "booth-02", Name = "Bún Bò Đặc Biệt", NameEn = "Special Hue Noodles", Description = "Bún bò Huế đặc biệt.", DescriptionEn = "Special Hue Noodles item.", Price = 68000m, PriceUsd = 2.72m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-02-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-02-menu-02", BoothId = "booth-02", Name = "Bún Bò Giò Heo", NameEn = "Hue Noodles with Pork Hock", Description = "Bún bò giò heo đặc trưng.", DescriptionEn = "Hue Noodles with Pork Hock item.", Price = 75000m, PriceUsd = 3.00m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-02-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-02-menu-03", BoothId = "booth-02", Name = "Bún Bò Thập Cẩm", NameEn = "Mixed Hue Noodles", Description = "Bún bò thập cẩm đầy đủ.", DescriptionEn = "Mixed Hue Noodles item.", Price = 79000m, PriceUsd = 3.16m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-02-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 03: Cơm Tấm Sài Gòn ---
            menuItems.Add(new BoothMenuItem { Id = "booth-03-menu-01", BoothId = "booth-03", Name = "Cơm Tấm Sườn Bì Chả", NameEn = "Broken Rice Combo", Description = "Cơm tấm sườn bì chả truyền thống.", DescriptionEn = "Broken Rice Combo item.", Price = 62000m, PriceUsd = 2.48m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-03-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-03-menu-02", BoothId = "booth-03", Name = "Cơm Tấm Sườn Nướng", NameEn = "Broken Rice Grilled Pork", Description = "Sườn nướng thơm ngon.", DescriptionEn = "Broken Rice Grilled Pork item.", Price = 69000m, PriceUsd = 2.76m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-03-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-03-menu-03", BoothId = "booth-03", Name = "Cơm Tấm Đặc Biệt", NameEn = "Special Broken Rice", Description = "Cơm tấm đặc biệt.", DescriptionEn = "Special Broken Rice item.", Price = 79000m, PriceUsd = 3.16m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-03-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 04: Bánh Mì Việt ---
            menuItems.Add(new BoothMenuItem { Id = "booth-04-menu-01", BoothId = "booth-04", Name = "Bánh Mì Thịt Nướng", NameEn = "Grilled Pork Banh Mi", Description = "Bánh mì kẹp thịt nướng.", DescriptionEn = "Grilled Pork Banh Mi item.", Price = 35000m, PriceUsd = 1.40m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-04-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-04-menu-02", BoothId = "booth-04", Name = "Bánh Mì Gà", NameEn = "Chicken Banh Mi", Description = "Bánh mì gà xé.", DescriptionEn = "Chicken Banh Mi item.", Price = 38000m, PriceUsd = 1.52m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-04-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-04-menu-03", BoothId = "booth-04", Name = "Bánh Mì Đặc Biệt", NameEn = "Special Banh Mi", Description = "Bánh mì đặc biệt đầy đủ.", DescriptionEn = "Special Banh Mi item.", Price = 45000m, PriceUsd = 1.80m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-04-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 05: Chè Ba Miền ---
            menuItems.Add(new BoothMenuItem { Id = "booth-05-menu-01", BoothId = "booth-05", Name = "Chè Đậu Xanh", NameEn = "Mung Bean Sweet Soup", Description = "Chè đậu xanh thanh mát.", DescriptionEn = "Mung Bean Sweet Soup item.", Price = 28000m, PriceUsd = 1.12m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-05-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-05-menu-02", BoothId = "booth-05", Name = "Chè Thập Cẩm", NameEn = "Mixed Sweet Soup", Description = "Chè thập cẩm đủ loại.", DescriptionEn = "Mixed Sweet Soup item.", Price = 32000m, PriceUsd = 1.28m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-05-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-05-menu-03", BoothId = "booth-05", Name = "Chè Dừa Non", NameEn = "Young Coconut Sweet Soup", Description = "Chè dừa non béo ngậy.", DescriptionEn = "Young Coconut Sweet Soup item.", Price = 36000m, PriceUsd = 1.44m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-05-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 06: Nem Nướng Đà Lạt ---
            menuItems.Add(new BoothMenuItem { Id = "booth-06-menu-01", BoothId = "booth-06", Name = "Nem Nướng Phần", NameEn = "Grilled Pork Rolls Set", Description = "Nem nướng đặc sản Đà Lạt.", DescriptionEn = "Grilled Pork Rolls Set item.", Price = 68000m, PriceUsd = 2.72m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-06-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-06-menu-02", BoothId = "booth-06", Name = "Nem Nướng Combo", NameEn = "Grilled Pork Combo", Description = "Combo nem nướng hấp dẫn.", DescriptionEn = "Grilled Pork Combo item.", Price = 79000m, PriceUsd = 3.16m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-06-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-06-menu-03", BoothId = "booth-06", Name = "Nem Nướng Đặc Biệt", NameEn = "Special Grilled Pork Rolls", Description = "Nem nướng đặc biệt.", DescriptionEn = "Special Grilled Pork Rolls item.", Price = 89000m, PriceUsd = 3.56m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-06-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 07: Bánh Xèo Miền Tây ---
            menuItems.Add(new BoothMenuItem { Id = "booth-07-menu-01", BoothId = "booth-07", Name = "Bánh Xèo Tôm Thịt", NameEn = "Shrimp Pork Pancake", Description = "Bánh xèo nhân tôm thịt.", DescriptionEn = "Shrimp Pork Pancake item.", Price = 65000m, PriceUsd = 2.60m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-07-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-07-menu-02", BoothId = "booth-07", Name = "Bánh Xèo Chay", NameEn = "Vegetarian Pancake", Description = "Bánh xèo nhân đậu xanh.", DescriptionEn = "Vegetarian Pancake item.", Price = 58000m, PriceUsd = 2.32m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-07-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-07-menu-03", BoothId = "booth-07", Name = "Bánh Xèo Đặc Biệt", NameEn = "Special Pancake", Description = "Bánh xèo đặc biệt siêu to.", DescriptionEn = "Special Pancake item.", Price = 76000m, PriceUsd = 3.04m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-07-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 08: Gỏi Cuốn Tươi ---
            menuItems.Add(new BoothMenuItem { Id = "booth-08-menu-01", BoothId = "booth-08", Name = "Gỏi Cuốn Tôm Thịt", NameEn = "Shrimp Pork Spring Rolls", Description = "Gỏi cuốn tôm thịt tươi ngon.", DescriptionEn = "Shrimp Pork Spring Rolls item.", Price = 42000m, PriceUsd = 1.68m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-08-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-08-menu-02", BoothId = "booth-08", Name = "Gỏi Cuốn Bò Nướng", NameEn = "Beef Spring Rolls", Description = "Gỏi cuốn nhân bò nướng.", DescriptionEn = "Beef Spring Rolls item.", Price = 48000m, PriceUsd = 1.92m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-08-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-08-menu-03", BoothId = "booth-08", Name = "Combo 6 Cuốn", NameEn = "6-roll Combo", Description = "Combo 6 cuốn đầy đủ.", DescriptionEn = "6-roll Combo item.", Price = 75000m, PriceUsd = 3.00m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-08-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 09: Hải Sản Nướng ---
            menuItems.Add(new BoothMenuItem { Id = "booth-09-menu-01", BoothId = "booth-09", Name = "Mực Nướng Sa Tế", NameEn = "Grilled Squid", Description = "Mực nướng sa tế cay nồng.", DescriptionEn = "Grilled Squid item.", Price = 98000m, PriceUsd = 3.92m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-09-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-09-menu-02", BoothId = "booth-09", Name = "Tôm Nướng Muối Ớt", NameEn = "Grilled Shrimp", Description = "Tôm nướng muối ớt đậm đà.", DescriptionEn = "Grilled Shrimp item.", Price = 115000m, PriceUsd = 4.60m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-09-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-09-menu-03", BoothId = "booth-09", Name = "Combo Hải Sản", NameEn = "Seafood Combo", Description = "Combo hải sản nướng thập cẩm.", DescriptionEn = "Seafood Combo item.", Price = 149000m, PriceUsd = 5.96m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-09-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            // --- Booth 10: Cà Phê & Trà Sữa ---
            menuItems.Add(new BoothMenuItem { Id = "booth-10-menu-01", BoothId = "booth-10", Name = "Cà Phê Sữa Đá", NameEn = "Iced Milk Coffee", Description = "Cà phê sữa đá truyền thống.", DescriptionEn = "Iced Milk Coffee item.", Price = 30000m, PriceUsd = 1.20m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-10-menu-01.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-10-menu-02", BoothId = "booth-10", Name = "Trà Sữa Trân Châu", NameEn = "Bubble Milk Tea", Description = "Trà sữa trân châu đường đen.", DescriptionEn = "Bubble Milk Tea item.", Price = 42000m, PriceUsd = 1.68m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-10-menu-02.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });
            menuItems.Add(new BoothMenuItem { Id = "booth-10-menu-03", BoothId = "booth-10", Name = "Combo Đồ Uống", NameEn = "Drink Combo", Description = "Combo cà phê và trà sữa.", DescriptionEn = "Drink Combo item.", Price = 70000m, PriceUsd = 2.80m, ImageUrl = "http://192.168.88.235:5151/uploads/menu/booth-10-menu-03.png", UpdatedAtUtc = DateTime.UtcNow, IsDeleted = false });

            db.BoothMenuItems.AddRange(menuItems);
            await db.SaveChangesAsync();
        }

        // 5. Bảng BoothMenuItemTranslations (Bản dịch món ăn) - 3 ví dụ
        // 5. Bảng BoothMenuItemTranslations (Bản dịch món ăn) - Full 90 bản ghi từ poi.db
        if (!await db.BoothMenuItemTranslations.AnyAsync())
        {
            var itemTranslations = new List<BoothMenuItemTranslationLocal>();

            // Booth 01 - Phở Hà Nội
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-01-menu-01", LanguageCode = "vi", Name = "Phở Hà Nội Phở đặc biệt", Description = "Phở đặc biệt của Phở Hà Nội, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-01-menu-01", LanguageCode = "en", Name = "Pho Special", Description = "Pho Special item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-01-menu-01", LanguageCode = "zh", Name = "Pho Special 中文", Description = "Chinese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-01-menu-01", LanguageCode = "ja", Name = "Pho Special 日本語", Description = "Japanese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-01-menu-01", LanguageCode = "ko", Name = "Pho Special 한국어", Description = "Korean localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-01-menu-02", LanguageCode = "vi", Name = "Phở Hà Nội Phở tái nạm", Description = "Phở tái nạm của Phở Hà Nội, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-01-menu-02", LanguageCode = "en", Name = "Rare & Brisket Pho", Description = "Rare & Brisket Pho item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-01-menu-03", LanguageCode = "vi", Name = "Phở Hà Nội Phở bò viên", Description = "Phở bò viên của Phở Hà Nội, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-01-menu-03", LanguageCode = "en", Name = "Meatball Pho", Description = "Meatball Pho item for demo and search." });

            // Booth 02 - Bún Bò Huế
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-02-menu-01", LanguageCode = "vi", Name = "Bún Bò Huế Bún bò đặc biệt", Description = "Bún bò đặc biệt của Bún Bò Huế, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-02-menu-01", LanguageCode = "en", Name = "Special Hue Noodles", Description = "Special Hue Noodles item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-02-menu-01", LanguageCode = "zh", Name = "Special Hue Noodles 中文", Description = "Chinese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-02-menu-01", LanguageCode = "ja", Name = "Special Hue Noodles 日本語", Description = "Japanese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-02-menu-01", LanguageCode = "ko", Name = "Special Hue Noodles 한국어", Description = "Korean localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-02-menu-02", LanguageCode = "vi", Name = "Bún Bò Huế Bún bò giò heo", Description = "Bún bò giò heo của Bún Bò Huế, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-02-menu-02", LanguageCode = "en", Name = "Hue Noodles with Pork Hock", Description = "Hue Noodles with Pork Hock item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-02-menu-03", LanguageCode = "vi", Name = "Bún Bò Huế Bún bò thập cẩm", Description = "Bún bò thập cẩm của Bún Bò Huế, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-02-menu-03", LanguageCode = "en", Name = "Mixed Hue Noodles", Description = "Mixed Hue Noodles item for demo and search." });

            // Booth 03 - Cơm Tấm Sài Gòn
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-03-menu-01", LanguageCode = "vi", Name = "Cơm Tấm Sài Gòn Cơm tấm sườn bì chả", Description = "Cơm tấm sườn bì chả của Cơm Tấm Sài Gòn, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-03-menu-01", LanguageCode = "en", Name = "Broken Rice Combo", Description = "Broken Rice Combo item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-03-menu-01", LanguageCode = "zh", Name = "Broken Rice Combo 中文", Description = "Chinese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-03-menu-01", LanguageCode = "ja", Name = "Broken Rice Combo 日本語", Description = "Japanese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-03-menu-01", LanguageCode = "ko", Name = "Broken Rice Combo 한국어", Description = "Korean localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-03-menu-02", LanguageCode = "vi", Name = "Cơm Tấm Sài Gòn Cơm tấm sườn nướng", Description = "Cơm tấm sườn nướng của Cơm Tấm Sài Gòn, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-03-menu-02", LanguageCode = "en", Name = "Broken Rice Grilled Pork", Description = "Broken Rice Grilled Pork item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-03-menu-03", LanguageCode = "vi", Name = "Cơm Tấm Sài Gòn Cơm tấm đặc biệt", Description = "Cơm tấm đặc biệt của Cơm Tấm Sài Gòn, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-03-menu-03", LanguageCode = "en", Name = "Special Broken Rice", Description = "Special Broken Rice item for demo and search." });

            // Booth 04 - Bánh Mì Việt
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-04-menu-01", LanguageCode = "vi", Name = "Bánh Mì Việt Bánh mì thịt nướng", Description = "Bánh mì thịt nướng của Bánh Mì Việt, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-04-menu-01", LanguageCode = "en", Name = "Grilled Pork Banh Mi", Description = "Grilled Pork Banh Mi item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-04-menu-01", LanguageCode = "zh", Name = "Grilled Pork Banh Mi 中文", Description = "Chinese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-04-menu-01", LanguageCode = "ja", Name = "Grilled Pork Banh Mi 日本語", Description = "Japanese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-04-menu-01", LanguageCode = "ko", Name = "Grilled Pork Banh Mi 한국어", Description = "Korean localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-04-menu-02", LanguageCode = "vi", Name = "Bánh Mì Việt Bánh mì gà", Description = "Bánh mì gà của Bánh Mì Việt, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-04-menu-02", LanguageCode = "en", Name = "Chicken Banh Mi", Description = "Chicken Banh Mi item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-04-menu-03", LanguageCode = "vi", Name = "Bánh Mì Việt Bánh mì đặc biệt", Description = "Bánh mì đặc biệt của Bánh Mì Việt, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-04-menu-03", LanguageCode = "en", Name = "Special Banh Mi", Description = "Special Banh Mi item for demo and search." });

            // Booth 05 - Chè Ba Miền
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-05-menu-01", LanguageCode = "vi", Name = "Chè Ba Miền Chè đậu xanh", Description = "Chè đậu xanh của Chè Ba Miền, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-05-menu-01", LanguageCode = "en", Name = "Mung Bean Sweet Soup", Description = "Mung Bean Sweet Soup item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-05-menu-01", LanguageCode = "zh", Name = "Mung Bean Sweet Soup 中文", Description = "Chinese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-05-menu-01", LanguageCode = "ja", Name = "Mung Bean Sweet Soup 日本語", Description = "Japanese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-05-menu-01", LanguageCode = "ko", Name = "Mung Bean Sweet Soup 한국어", Description = "Korean localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-05-menu-02", LanguageCode = "vi", Name = "Chè Ba Miền Chè thập cẩm", Description = "Chè thập cẩm của Chè Ba Miền, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-05-menu-02", LanguageCode = "en", Name = "Mixed Sweet Soup", Description = "Mixed Sweet Soup item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-05-menu-03", LanguageCode = "vi", Name = "Chè Ba Miền Chè dừa non", Description = "Chè dừa non của Chè Ba Miền, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-05-menu-03", LanguageCode = "en", Name = "Young Coconut Sweet Soup", Description = "Young Coconut Sweet Soup item for demo and search." });

            // Booth 06 - Nem Nướng Đà Lạt
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-06-menu-01", LanguageCode = "vi", Name = "Nem Nướng Đà Lạt Nem nướng phần", Description = "Nem nướng phần của Nem Nướng Đà Lạt, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-06-menu-01", LanguageCode = "en", Name = "Grilled Pork Rolls Set", Description = "Grilled Pork Rolls Set item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-06-menu-01", LanguageCode = "zh", Name = "Grilled Pork Rolls Set 中文", Description = "Chinese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-06-menu-01", LanguageCode = "ja", Name = "Grilled Pork Rolls Set 日本語", Description = "Japanese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-06-menu-01", LanguageCode = "ko", Name = "Grilled Pork Rolls Set 한국어", Description = "Korean localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-06-menu-02", LanguageCode = "vi", Name = "Nem Nướng Đà Lạt Nem nướng combo", Description = "Nem nướng combo của Nem Nướng Đà Lạt, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-06-menu-02", LanguageCode = "en", Name = "Grilled Pork Combo", Description = "Grilled Pork Combo item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-06-menu-03", LanguageCode = "vi", Name = "Nem Nướng Đà Lạt Nem nướng đặc biệt", Description = "Nem nướng đặc biệt của Nem Nướng Đà Lạt, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-06-menu-03", LanguageCode = "en", Name = "Special Grilled Pork Rolls", Description = "Special Grilled Pork Rolls item for demo and search." });

            // Booth 07 - Bánh Xèo Miền Tây
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-07-menu-01", LanguageCode = "vi", Name = "Bánh Xèo Miền Tây Bánh xèo tôm thịt", Description = "Bánh xèo tôm thịt của Bánh Xèo Miền Tây, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-07-menu-01", LanguageCode = "en", Name = "Shrimp Pork Pancake", Description = "Shrimp Pork Pancake item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-07-menu-01", LanguageCode = "zh", Name = "Shrimp Pork Pancake 中文", Description = "Chinese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-07-menu-01", LanguageCode = "ja", Name = "Shrimp Pork Pancake 日本語", Description = "Japanese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-07-menu-01", LanguageCode = "ko", Name = "Shrimp Pork Pancake 한국어", Description = "Korean localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-07-menu-02", LanguageCode = "vi", Name = "Bánh Xèo Miền Tây Bánh xèo chay", Description = "Bánh xèo chay của Bánh Xèo Miền Tây, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-07-menu-02", LanguageCode = "en", Name = "Vegetarian Pancake", Description = "Vegetarian Pancake item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-07-menu-03", LanguageCode = "vi", Name = "Bánh Xèo Miền Tây Bánh xèo đặc biệt", Description = "Bánh xèo đặc biệt của Bánh Xèo Miền Tây, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-07-menu-03", LanguageCode = "en", Name = "Special Pancake", Description = "Special Pancake item for demo and search." });

            // Booth 08 - Gỏi Cuốn Tươi
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-08-menu-01", LanguageCode = "vi", Name = "Gỏi Cuốn Tươi Gỏi cuốn tôm thịt", Description = "Gỏi cuốn tôm thịt của Gỏi Cuốn Tươi, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-08-menu-01", LanguageCode = "en", Name = "Shrimp Pork Spring Rolls", Description = "Shrimp Pork Spring Rolls item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-08-menu-01", LanguageCode = "zh", Name = "Shrimp Pork Spring Rolls 中文", Description = "Chinese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-08-menu-01", LanguageCode = "ja", Name = "Shrimp Pork Spring Rolls 日本語", Description = "Japanese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-08-menu-01", LanguageCode = "ko", Name = "Shrimp Pork Spring Rolls 한국어", Description = "Korean localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-08-menu-02", LanguageCode = "vi", Name = "Gỏi Cuốn Tươi Gỏi cuốn bò nướng", Description = "Gỏi cuốn bò nướng của Gỏi Cuốn Tươi, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-08-menu-02", LanguageCode = "en", Name = "Beef Spring Rolls", Description = "Beef Spring Rolls item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-08-menu-03", LanguageCode = "vi", Name = "Gỏi Cuốn Tươi Combo 6 cuốn", Description = "Combo 6 cuốn của Gỏi Cuốn Tươi, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-08-menu-03", LanguageCode = "en", Name = "6-roll Combo", Description = "6-roll Combo item for demo and search." });

            // Booth 09 - Hải Sản Nướng
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-09-menu-01", LanguageCode = "vi", Name = "Hải Sản Nướng Mực nướng sa tế", Description = "Mực nướng sa tế của Hải Sản Nướng, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-09-menu-01", LanguageCode = "en", Name = "Grilled Squid", Description = "Grilled Squid item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-09-menu-01", LanguageCode = "zh", Name = "Grilled Squid 中文", Description = "Chinese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-09-menu-01", LanguageCode = "ja", Name = "Grilled Squid 日本語", Description = "Japanese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-09-menu-01", LanguageCode = "ko", Name = "Grilled Squid 한국어", Description = "Korean localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-09-menu-02", LanguageCode = "vi", Name = "Hải Sản Nướng Tôm nướng muối ớt", Description = "Tôm nướng muối ớt của Hải Sản Nướng, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-09-menu-02", LanguageCode = "en", Name = "Grilled Shrimp", Description = "Grilled Shrimp item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-09-menu-03", LanguageCode = "vi", Name = "Hải Sản Nướng Combo hải sản", Description = "Combo hải sản của Hải Sản Nướng, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-09-menu-03", LanguageCode = "en", Name = "Seafood Combo", Description = "Seafood Combo item for demo and search." });

            // Booth 10 - Cà Phê & Trà Sữa
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-10-menu-01", LanguageCode = "vi", Name = "Cà Phê & Trà Sữa Cà phê sữa đá", Description = "Cà phê sữa đá của Cà Phê & Trà Sữa, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-10-menu-01", LanguageCode = "en", Name = "Iced Milk Coffee", Description = "Iced Milk Coffee item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-10-menu-01", LanguageCode = "zh", Name = "Iced Milk Coffee 中文", Description = "Chinese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-10-menu-01", LanguageCode = "ja", Name = "Iced Milk Coffee 日本語", Description = "Japanese localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-10-menu-01", LanguageCode = "ko", Name = "Iced Milk Coffee 한국어", Description = "Korean localized menu description." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-10-menu-02", LanguageCode = "vi", Name = "Cà Phê & Trà Sữa Trà sữa trân châu", Description = "Trà sữa trân châu của Cà Phê & Trà Sữa, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-10-menu-02", LanguageCode = "en", Name = "Bubble Milk Tea", Description = "Bubble Milk Tea item for demo and search." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-10-menu-03", LanguageCode = "vi", Name = "Cà Phê & Trà Sữa Combo đồ uống", Description = "Combo đồ uống của Cà Phê & Trà Sữa, dữ liệu phù hợp với schema hiện tại để test web, mobile, dashboard và tìm kiếm." });
            itemTranslations.Add(new BoothMenuItemTranslationLocal { MenuItemId = "booth-10-menu-03", LanguageCode = "en", Name = "Drink Combo", Description = "Drink Combo item for demo and search." });

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