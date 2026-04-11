using PoiNarration.Core.Models;

namespace PoiNarration.Api.Services;

public class TranslationService : ITranslationService
{
    private static readonly string[] SupportedLanguages =
    {
        "vi", "en", "zh", "ja", "ko", "fr", "es", "it", "ru"
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

            var (currencyCode, localizedPrice, priceText) = BuildLocalizedPrice(menuItem.Price, lang, menuItem.PriceUsd);

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
        // TODO: Cắm translation engine thật ở đây
        // Hiện tại trả fallback để code chạy ổn định
        await Task.CompletedTask;
        return targetLang == "vi" ? sourceVi : sourceVi;
    }
}