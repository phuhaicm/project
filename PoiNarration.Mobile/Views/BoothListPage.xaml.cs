using PoiNarration.Core.Models;
using PoiNarration.Mobile.Models;
using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile.Views;

public partial class BoothListPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly SyncService _syncService;
    private readonly NarrationService _narrationService;

    private List<Booth> _allBooths = new();

    public BoothListPage(AppDatabase db, SyncService syncService, NarrationService narrationService)
    {
        InitializeComponent();

        _db = db;
        _syncService = syncService;
        _narrationService = narrationService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _db.InitAsync();

            LanguagePicker.SelectedItem = LanguageService.CurrentLanguage;
            CurrentLanguageLabel.Text = LanguageService.CurrentLanguage;

            await LoadBoothsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Lỗi", ex.Message, "OK");
        }
    }

    private async Task LoadBoothsAsync()
    {
        _allBooths = await _db.GetAllBoothsAsync();

        TotalBoothsLabel.Text = _allBooths.Count.ToString();
        SyncStatusLabel.Text = $"Đã tải {_allBooths.Count} booth";

        ApplyFilter();
    }

    private void ApplyFilter()
    {
        var keyword = BoothSearchBar.Text?.Trim().ToLowerInvariant() ?? "";

        var filtered = _allBooths
            .Where(x =>
                string.IsNullOrWhiteSpace(keyword) ||
                x.NameVi.ToLowerInvariant().Contains(keyword) ||
                x.NameEn.ToLowerInvariant().Contains(keyword) ||
                x.ZoneId.ToLowerInvariant().Contains(keyword))
            .Select(x => new BoothCardVm
            {
                // Bê nguyên xi dữ liệu từ Database sang ViewModel
                Id = x.Id,
                ZoneId = x.ZoneId,
                NameVi = x.NameVi,
                NameEn = x.NameEn,
                DescVi = x.DescVi,
                DescEn = x.DescEn,
                Lat = x.Lat,
                Lng = x.Lng,
                RadiusMeters = x.RadiusMeters,
                Priority = x.Priority,
                OwnerUserId = x.OwnerUserId,
                // Nếu không có ảnh thì lấy ảnh mặc định của MAUI
                ImageUrl = string.IsNullOrWhiteSpace(x.ImageUrl) ? "dotnet_bot.png" : x.ImageUrl,
                MapUrl = x.MapUrl,
                TtsScriptVi = x.TtsScriptVi,
                TtsScriptEn = x.TtsScriptEn,
                AudioUrlVi = x.AudioUrlVi,
                AudioUrlEn = x.AudioUrlEn,
                IsActive = x.IsActive
            })
            .ToList();

        BoothsCollectionView.ItemsSource = filtered;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void OnLanguageChanged(object sender, EventArgs e)
    {
        if (LanguagePicker.SelectedItem is string lang)
        {
            LanguageService.Set(lang);
            CurrentLanguageLabel.Text = lang;
        }
    }

    private async void OnSyncClicked(object sender, EventArgs e)
    {
        try
        {
            SyncStatusLabel.Text = "Đang đồng bộ...";
            await _syncService.SyncBootstrapAsync();
            await LoadBoothsAsync();
            SyncStatusLabel.Text = "Đồng bộ thành công";
        }
        catch (Exception ex)
        {
            SyncStatusLabel.Text = "Đồng bộ thất bại";
            await DisplayAlertAsync("Lỗi sync", ex.Message, "OK");
        }
    }

    

    private async void OnGpsModeClicked(object sender, EventArgs e)
    {
        // TODO: nếu bạn có GPS page riêng thì đổi route tại đây
        await DisplayAlertAsync("GPS Mode", "Chuyển qua GPS mode / geofence tại page bạn đã làm.", "OK");
    }

    private async void OnMapClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("mappage");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Lỗi mở bản đồ", ex.Message, "OK");
        }
    }

    private async void OnOpenBoothClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button btn && btn.CommandParameter is string boothId)
            {
                await Shell.Current.GoToAsync($"boothdetail?boothId={boothId}");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Lỗi", ex.Message, "OK");
        }
    }

    private async void OnPreviewBoothClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button btn && btn.CommandParameter is string boothId)
            {
                var booth = await _db.GetBoothAsync(boothId);
                if (booth == null) return;

                await _narrationService.SpeakBoothAsync(booth, "Manual");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Lỗi phát thử", ex.Message, "OK");
        }
    }
    private async void OnScanQrClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(QrScanPage));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Lỗi mở QR", ex.Message, "OK");
        }
    }
    


}