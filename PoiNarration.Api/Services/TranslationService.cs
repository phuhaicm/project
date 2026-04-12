using PoiNarration.Core.Models;

namespace PoiNarration.Api.Services;

public class TranslationService : ITranslationService
{
    private static readonly string[] SupportedLanguages =
    {
        "vi", "en", "zh", "ja", "ko", "fr", "es", "it", "ru"
    };

    // ===== TỪ ĐIỂN DEMO CHO MÓN ĂN / GIAN HÀNG =====
    private static readonly Dictionary<string, Dictionary<string, string>> WordMap = new()
    {
        ["phở"] = new() { ["en"] = "pho", ["zh"] = "河粉", ["ja"] = "フォー", ["ko"] = "퍼", ["fr"] = "pho", ["es"] = "pho", ["it"] = "pho", ["ru"] = "фо" },
        ["bún"] = new() { ["en"] = "rice noodle", ["zh"] = "米粉", ["ja"] = "米麺", ["ko"] = "쌀국수", ["fr"] = "nouilles de riz", ["es"] = "fideos de arroz", ["it"] = "spaghetti di riso", ["ru"] = "рисовая лапша" },
        ["bò"] = new() { ["en"] = "beef", ["zh"] = "牛肉", ["ja"] = "牛肉", ["ko"] = "소고기", ["fr"] = "bœuf", ["es"] = "carne de res", ["it"] = "manzo", ["ru"] = "говядина" },
        ["huế"] = new() { ["en"] = "Hue style", ["zh"] = "顺化风味", ["ja"] = "フエ風", ["ko"] = "후에식", ["fr"] = "style Huế", ["es"] = "estilo Huế", ["it"] = "stile Huế", ["ru"] = "по-хюэсски" },
        ["hà"] = new() { ["en"] = "Hanoi", ["zh"] = "河内", ["ja"] = "ハノイ", ["ko"] = "하노이", ["fr"] = "Hanoï", ["es"] = "Hanói", ["it"] = "Hanoi", ["ru"] = "Ханой" },
        ["nội"] = new() { ["en"] = "Hanoi", ["zh"] = "河内", ["ja"] = "ハノイ", ["ko"] = "하노이", ["fr"] = "Hanoï", ["es"] = "Hanói", ["it"] = "Hanoi", ["ru"] = "Ханой" },
        ["cơm"] = new() { ["en"] = "rice", ["zh"] = "米饭", ["ja"] = "ご飯", ["ko"] = "밥", ["fr"] = "riz", ["es"] = "arroz", ["it"] = "riso", ["ru"] = "рис" },
        ["tấm"] = new() { ["en"] = "broken rice", ["zh"] = "碎米饭", ["ja"] = "砕き米", ["ko"] = "껌땀", ["fr"] = "riz brisé", ["es"] = "arroz quebrado", ["it"] = "riso spezzato", ["ru"] = "дроблёный рис" },
        ["bánh"] = new() { ["en"] = "cake / bread", ["zh"] = "饼 / 面包", ["ja"] = "バイン", ["ko"] = "반", ["fr"] = "gâteau / pain", ["es"] = "pastel / pan", ["it"] = "torta / pane", ["ru"] = "лепёшка / хлеб" },
        ["mì"] = new() { ["en"] = "bread", ["zh"] = "面包", ["ja"] = "パン", ["ko"] = "빵", ["fr"] = "pain", ["es"] = "pan", ["it"] = "pane", ["ru"] = "хлеб" },
        ["chè"] = new() { ["en"] = "sweet dessert soup", ["zh"] = "甜汤", ["ja"] = "チェー", ["ko"] = "체", ["fr"] = "dessert sucré", ["es"] = "postre dulce", ["it"] = "dessert dolce", ["ru"] = "сладкий десерт" },
        ["nem"] = new() { ["en"] = "grilled roll", ["zh"] = "烤卷", ["ja"] = "焼きロール", ["ko"] = "구운 롤", ["fr"] = "rouleau grillé", ["es"] = "rollo a la parrilla", ["it"] = "involtino grigliato", ["ru"] = "жареный рулет" },
        ["nướng"] = new() { ["en"] = "grilled", ["zh"] = "烤", ["ja"] = "焼き", ["ko"] = "구운", ["fr"] = "grillé", ["es"] = "a la parrilla", ["it"] = "grigliato", ["ru"] = "жареный / гриль" },
        ["đà"] = new() { ["en"] = "Dalat", ["zh"] = "大叻", ["ja"] = "ダラット", ["ko"] = "달랏", ["fr"] = "Dalat", ["es"] = "Dalat", ["it"] = "Dalat", ["ru"] = "Далат" },
        ["lạt"] = new() { ["en"] = "Dalat", ["zh"] = "大叻", ["ja"] = "ダラット", ["ko"] = "달랏", ["fr"] = "Dalat", ["es"] = "Dalat", ["it"] = "Dalat", ["ru"] = "Далат" },
        ["gỏi"] = new() { ["en"] = "salad roll", ["zh"] = "春卷", ["ja"] = "生春巻き", ["ko"] = "생춘권", ["fr"] = "rouleau frais", ["es"] = "rollito fresco", ["it"] = "involtino fresco", ["ru"] = "свежий ролл" },
        ["cuốn"] = new() { ["en"] = "roll", ["zh"] = "卷", ["ja"] = "巻き", ["ko"] = "롤", ["fr"] = "rouleau", ["es"] = "rollo", ["it"] = "rotolo", ["ru"] = "ролл" },
        ["tôm"] = new() { ["en"] = "shrimp", ["zh"] = "虾", ["ja"] = "エビ", ["ko"] = "새우", ["fr"] = "crevette", ["es"] = "camarón", ["it"] = "gambero", ["ru"] = "креветка" },
        ["thịt"] = new() { ["en"] = "meat", ["zh"] = "肉", ["ja"] = "肉", ["ko"] = "고기", ["fr"] = "viande", ["es"] = "carne", ["it"] = "carne", ["ru"] = "мясо" },
        ["cà"] = new() { ["en"] = "coffee", ["zh"] = "咖啡", ["ja"] = "コーヒー", ["ko"] = "커피", ["fr"] = "café", ["es"] = "café", ["it"] = "caffè", ["ru"] = "кофе" },
        ["phê"] = new() { ["en"] = "coffee", ["zh"] = "咖啡", ["ja"] = "コーヒー", ["ko"] = "커피", ["fr"] = "café", ["es"] = "café", ["it"] = "caffè", ["ru"] = "кофе" },
        ["trà"] = new() { ["en"] = "tea", ["zh"] = "茶", ["ja"] = "お茶", ["ko"] = "차", ["fr"] = "thé", ["es"] = "té", ["it"] = "tè", ["ru"] = "чай" },
        ["sữa"] = new() { ["en"] = "milk", ["zh"] = "奶", ["ja"] = "ミルク", ["ko"] = "우유", ["fr"] = "lait", ["es"] = "leche", ["it"] = "latte", ["ru"] = "молоко" },
        ["đá"] = new() { ["en"] = "iced", ["zh"] = "冰", ["ja"] = "アイス", ["ko"] = "아이스", ["fr"] = "glacé", ["es"] = "helado", ["it"] = "ghiacciato", ["ru"] = "со льдом" },
        ["đặc"] = new() { ["en"] = "special", ["zh"] = "特制", ["ja"] = "スペシャル", ["ko"] = "스페셜", ["fr"] = "spécial", ["es"] = "especial", ["it"] = "speciale", ["ru"] = "специальный" },
        ["biệt"] = new() { ["en"] = "special", ["zh"] = "特制", ["ja"] = "スペシャル", ["ko"] = "스페셜", ["fr"] = "spécial", ["es"] = "especial", ["it"] = "speciale", ["ru"] = "специальный" },
    };

    public async Task<List<BoothTranslationLocal>> BuildBoothTranslationsAsync(Booth booth)
    {
        var result = new List<BoothTranslationLocal>();

        foreach (var lang in SupportedLanguages)
        {
            var translatedName = lang == "vi"
                ? booth.NameVi
                : await TranslateTextAsync(booth.NameVi, lang);

            var translatedDesc = lang == "vi"
                ? booth.DescVi
                : await TranslateTextAsync(booth.DescVi, lang);

            var tts = lang == "vi"
                ? BuildBoothTtsScript(booth.NameVi, booth.DescVi, "vi")
                : BuildBoothTtsScript(translatedName, translatedDesc, lang);

            result.Add(new BoothTranslationLocal
            {
                BoothId = booth.Id,
                LanguageCode = lang,
                Name = translatedName,
                Description = translatedDesc,
                TtsScript = tts,
                AudioUrl = null
            });
        }

        return result;
    }

    public async Task<List<BoothMenuItemTranslationLocal>> BuildMenuTranslationsAsync(BoothMenuItem menuItem)
    {
        var result = new List<BoothMenuItemTranslationLocal>();

        foreach (var lang in SupportedLanguages)
        {
            var translatedName = lang == "vi"
                ? menuItem.Name
                : await TranslateTextAsync(menuItem.Name, lang);

            var translatedDesc = lang == "vi"
                ? menuItem.Description
                : await TranslateTextAsync(menuItem.Description, lang);

            var (currencyCode, localizedPrice, priceText) =
                BuildLocalizedPrice(menuItem.Price, lang, menuItem.PriceUsd);

            result.Add(new BoothMenuItemTranslationLocal
            {
                MenuItemId = menuItem.Id,
                LanguageCode = lang,
                Name = translatedName,
                Description = translatedDesc,
                CurrencyCode = currencyCode,
                LocalizedPrice = localizedPrice,
                PriceText = priceText
            });
        }

        return result;
    }

    private static string BuildBoothTtsScript(string name, string description, string lang)
    {
        return lang switch
        {
            "zh" => $"欢迎来到{name}展位。{description}",
            "ja" => $"{name}のブースへようこそ。{description}",
            "ko" => $"{name} 부스에 오신 것을 환영합니다. {description}",
            "fr" => $"Bienvenue au stand {name}. {description}",
            "es" => $"Bienvenido al stand {name}. {description}",
            "it" => $"Benvenuto allo stand {name}. {description}",
            "ru" => $"Добро пожаловать на стенд {name}. {description}",
            "en" => $"Welcome to {name}. {description}",
            _ => $"Xin chào, bạn đang đến với gian hàng {name}. {description}"
        };
    }

    private static (string currencyCode, decimal localizedPrice, string priceText) BuildLocalizedPrice(decimal vnd, string lang, decimal usd)
    {
        return lang switch
        {
            "vi" => ("VND", vnd, $"{vnd:N0} đ"),
            "en" => ("USD", usd > 0 ? usd : Math.Round(vnd / 25000m, 2), $"${(usd > 0 ? usd : Math.Round(vnd / 25000m, 2)):0.##}"),
            "zh" => ("CNY", Math.Round(vnd / 3500m, 2), $"¥{Math.Round(vnd / 3500m, 2):0.##}"),
            "ja" => ("JPY", Math.Round(vnd / 170m, 0), $"¥{Math.Round(vnd / 170m, 0):0}"),
            "ko" => ("KRW", Math.Round(vnd / 18m, 0), $"₩{Math.Round(vnd / 18m, 0):0}"),
            "fr" or "es" or "it" => ("EUR", Math.Round(vnd / 27000m, 2), $"€{Math.Round(vnd / 27000m, 2):0.##}"),
            "ru" => ("RUB", Math.Round(vnd / 300m, 2), $"₽{Math.Round(vnd / 300m, 2):0.##}"),
            _ => ("USD", usd > 0 ? usd : Math.Round(vnd / 25000m, 2), $"${(usd > 0 ? usd : Math.Round(vnd / 25000m, 2)):0.##}")
        };
    }

    private async Task<string> TranslateTextAsync(string sourceVi, string targetLang)
    {
        await Task.CompletedTask;

        if (string.IsNullOrWhiteSpace(sourceVi))
            return "";

        if (targetLang == "vi")
            return sourceVi;

        // Tách từ và dịch theo dictionary demo
        var tokens = sourceVi
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var output = new List<string>();

        foreach (var token in tokens)
        {
            var key = token.Trim().ToLowerInvariant();

            if (WordMap.TryGetValue(key, out var langMap) && langMap.TryGetValue(targetLang, out var translated))
            {
                output.Add(translated);
            }
            else
            {
                // fallback: giữ nguyên nếu chưa có từ điển
                output.Add(token);
            }
        }

        return string.Join(" ", output);
    }
}
