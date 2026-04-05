using Microsoft.Extensions.DependencyInjection;
using PoiNarration.Core.Models;
using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile.Views;

[QueryProperty(nameof(BoothId), "boothId")]
public partial class BoothDetailPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly NarrationService _narrationService;
    private readonly ApiService _apiService;

    public string BoothId { get; set; } = "";

    private Booth? _currentBooth;

    public BoothDetailPage()
    {
        InitializeComponent();

        var services = Application.Current?.Handler?.MauiContext?.Services
                       ?? throw new Exception("Services is null");

        _db = services.GetRequiredService<AppDatabase>();
        _narrationService = services.GetRequiredService<NarrationService>();
        _apiService = services.GetRequiredService<ApiService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _db.InitAsync();

        if (string.IsNullOrWhiteSpace(BoothId))
            return;

        var booth = await _db.GetBoothAsync(BoothId);
        if (booth == null)
            return;

        _currentBooth = booth;

        BoothName.Text = LanguageService.IsVi ? booth.NameVi : booth.NameEn;
        BoothDesc.Text = LanguageService.IsVi ? booth.DescVi : booth.DescEn;

        await LoadMenuAsync();
    }

    private async Task LoadMenuAsync()
    {
        try
        {
            var onlineMenu = await _apiService.GetMenuByBoothAsync(BoothId);

            foreach (var item in onlineMenu)
            {
                await _db.UpsertMenuItemAsync(item);
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
        if (_currentBooth == null) return;

        await _narrationService.SpeakBoothAsync(_currentBooth, "Manual");
    }

    private async void OnStopClicked(object sender, EventArgs e)
    {
        await _narrationService.StopAsync();
    }
}