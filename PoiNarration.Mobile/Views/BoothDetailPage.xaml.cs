using PoiNarration.Core.Models;
using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile.Views;

[QueryProperty(nameof(BoothId), "boothId")]
public partial class BoothDetailPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly NarrationService _narrationService;
    private readonly ApiService _apiService;
    private readonly AutoBoothNavigatorService _autoBoothNavigatorService;
    private int _loadVersion = 0;

    public string BoothId { get; set; } = "";
    private Booth? _currentBooth;

    public BoothDetailPage(
        AppDatabase db,
        NarrationService narrationService,
        ApiService apiService,
        AutoBoothNavigatorService autoBoothNavigatorService)
    {
        InitializeComponent();

        _db = db;
        _narrationService = narrationService;
        _apiService = apiService;
        _autoBoothNavigatorService = autoBoothNavigatorService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _autoBoothNavigatorService.BoothTriggered -= OnBoothTriggered;
        _autoBoothNavigatorService.BoothTriggered += OnBoothTriggered;

        await _db.InitAsync();

        if (!string.IsNullOrWhiteSpace(BoothId))
        {
            var version = Interlocked.Increment(ref _loadVersion);
            await LoadBoothAsync(BoothId, version);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _autoBoothNavigatorService.BoothTriggered -= OnBoothTriggered;
    }

    private async void OnBoothTriggered(object? sender, Booth booth)
    {
        if (booth.Id == BoothId)
            return;

        BoothId = booth.Id;
        var version = Interlocked.Increment(ref _loadVersion);

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await LoadBoothAsync(booth.Id, version);
        });
    }

    
    private async void OnPlayMenuItemClicked(object sender, EventArgs e)
    {
        // Khi nút được bấm, nó sẽ mang theo một "gói hàng" (CommandParameter)
        // Gói hàng này chính là đoạn Text mà mình sẽ gắn ở file XAML (Bước 3)
        if (sender is Button btn && btn.CommandParameter is string textToRead)
        {
            var lang = LanguageService.CurrentLanguage;

            // Gọi cô trợ lý ra đọc đoạn text đó
            await _narrationService.SpeakTextAsync(textToRead, lang);
        }
    }

    private async void OnPlayClicked(object sender, EventArgs e)
    {
        if (_currentBooth != null)
        {
            await _narrationService.SpeakBoothAsync(_currentBooth, "Manual");
        }
    }

    private async void OnStopClicked(object sender, EventArgs e)
    {
        await _narrationService.StopAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    private static string BuildPriceText(BoothMenuItem item, string lang)
    {
        return lang switch
        {
            "vi" => $"{item.Price:N0} đ",
            "en" => item.PriceUsd > 0 ? $"${item.PriceUsd:0.##}" : $"{item.Price:N0}",
            "zh" => $"¥{Math.Round(item.Price / 3500m, 2):0.##}",
            "ja" => $"¥{Math.Round(item.Price / 170m, 0):0}",
            "ko" => $"₩{Math.Round(item.Price / 18m, 0):0}",
            "fr" or "es" or "it" => $"€{Math.Round(item.Price / 27000m, 2):0.##}",
            "ru" => $"₽{Math.Round(item.Price / 300m, 2):0.##}",
            _ => item.PriceUsd > 0 ? $"${item.PriceUsd:0.##}" : $"{item.Price:N0}"
        };
    }
    private async Task LoadBoothAsync(string boothId, int version)
    {
        var booth = await _db.GetBoothAsync(boothId);
        if (booth == null)
        {
            await DisplayAlertAsync(
    LanguageService.T("Ui_Alert_Error"),
    LanguageService.T("Ui_Alert_NotFoundBooth"),
    LanguageService.T("Ui_Alert_Ok")); 
            return;
        }

        // nếu trong lúc load đã có request mới hơn thì bỏ
        if (version != _loadVersion)
            return;

        _currentBooth = booth;

        var lang = LanguageService.CurrentLanguage;
        var translation = await _db.GetBoothTranslationAsync(booth.Id, lang)
                         ?? await _db.GetBoothTranslationAsync(booth.Id, "en")
                         ?? await _db.GetBoothTranslationAsync(booth.Id, "vi");

        if (version != _loadVersion)
            return;

        BoothName.Text = translation?.Name
                         ?? (LanguageService.IsVi ? booth.NameVi : booth.NameEn);

        BoothDesc.Text = translation?.Description
                         ?? (LanguageService.IsVi ? booth.DescVi : booth.DescEn);

        if (BoothImage != null)
        {
            BoothImage.Source = _apiService.ResolveMediaUrl(booth.ImageUrl);
        }

        await BindMenuFromLocalAsync(boothId, version);

        _ = Task.Run(async () =>
        {
            try
            {
                var onlineMenu = await _apiService.GetMenuByBoothAsync(boothId);
                if (onlineMenu != null)
                {
                    foreach (var item in onlineMenu)
                    {
                        await _db.UpsertMenuItemAsync(item);
                    }

                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        if (version == _loadVersion)
                        {
                            await BindMenuFromLocalAsync(boothId, version);
                        }
                    });
                }
            }
            catch
            {
                // fallback offline
            }
        });
    }
    private async Task BindMenuFromLocalAsync(string boothId, int version)
    {
        var rawMenu = await _db.GetMenuByBoothAsync(boothId);
        var lang = LanguageService.CurrentLanguage;

        if (version != _loadVersion)
            return;

        var menuDisplay = new List<MenuItemDisplayVm>();

        foreach (var item in rawMenu)
        {
            var translations = await _db.GetMenuTranslationsAsync(item.Id);

            var menuTranslation = translations.FirstOrDefault(x => x.LanguageCode == lang)
                               ?? translations.FirstOrDefault(x => x.LanguageCode == "en")
                               ?? translations.FirstOrDefault(x => x.LanguageCode == "vi");

            var translatedName = menuTranslation?.Name
                ?? (LanguageService.IsVi
                    ? item.Name
                    : (!string.IsNullOrWhiteSpace(item.NameEn) ? item.NameEn : item.Name));

            var translatedDesc = menuTranslation?.Description
                ?? (LanguageService.IsVi
                    ? item.Description
                    : (!string.IsNullOrWhiteSpace(item.DescriptionEn) ? item.DescriptionEn : item.Description));

            var priceText = menuTranslation?.PriceText;
            if (string.IsNullOrWhiteSpace(priceText))
            {
                priceText = BuildPriceText(item, lang);
            }

            menuDisplay.Add(new MenuItemDisplayVm
            {
                Id = item.Id,
                Name = translatedName,
                Description = translatedDesc,
                PriceText = priceText,
                ImageUrl = _apiService.ResolveMediaUrl(item.ImageUrl)
            });
        }

        if (version == _loadVersion)
        {
            MenuView.ItemsSource = menuDisplay;
        }
    }
    private static decimal ConvertCurrency(decimal vnd, string currencyCode)
    {
        return currencyCode switch
        {
            "CNY" => Math.Round(vnd / 3500m, 2),
            "JPY" => Math.Round(vnd / 170m, 0),
            "KRW" => Math.Round(vnd / 18m, 0),
            "EUR" => Math.Round(vnd / 27000m, 2),
            "RUB" => Math.Round(vnd / 300m, 2),
            _ => vnd
        };
    }

}


public class MenuItemDisplayVm
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string PriceText { get; set; } = "";
    public string ImageUrl { get; set; } = "";
}