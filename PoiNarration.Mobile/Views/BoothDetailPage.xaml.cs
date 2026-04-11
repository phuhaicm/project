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
            await LoadBoothAsync();
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

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await LoadBoothAsync();
        });
    }

    private async Task LoadBoothAsync()
    {
        var booth = await _db.GetBoothAsync(BoothId);
        if (booth == null)
        {
            await DisplayAlertAsync("Lỗi", "Không tìm thấy booth.", "OK");
            return;
        }

        _currentBooth = booth;

        var lang = LanguageService.CurrentLanguage;
        var translation = await _db.GetBoothTranslationAsync(booth.Id, lang)
                         ?? await _db.GetBoothTranslationAsync(booth.Id, "en")
                         ?? await _db.GetBoothTranslationAsync(booth.Id, "vi");

        BoothName.Text = translation?.Name
                         ?? (LanguageService.IsVi ? booth.NameVi : booth.NameEn);

        BoothDesc.Text = translation?.Description
                         ?? (LanguageService.IsVi ? booth.DescVi : booth.DescEn);

        await LoadMenuAsync();
    }

    private async Task LoadMenuAsync()
    {
        try
        {
            var onlineMenu = await _apiService.GetMenuByBoothAsync(BoothId);
            if (onlineMenu != null)
            {
                foreach (var item in onlineMenu)
                {
                    await _db.UpsertMenuItemAsync(item);
                }
            }
        }
        catch
        {
            // fallback offline
        }

        MenuView.ItemsSource = await _db.GetMenuByBoothAsync(BoothId);
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
}

