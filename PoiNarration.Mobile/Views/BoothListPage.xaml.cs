using PoiNarration.Core.Models;
using PoiNarration.Mobile.Models;
using PoiNarration.Mobile.Services;

namespace PoiNarration.Mobile.Views;

public partial class BoothListPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly SyncService _syncService;
    private readonly NarrationService _narrationService;
    private readonly AutoBoothNavigatorService _autoBoothNavigatorService;

    private List<Booth> _allBooths = new();
    private bool _gpsModeEnabled;

    public BoothListPage(
        AppDatabase db,
        SyncService syncService,
        NarrationService narrationService,
        AutoBoothNavigatorService autoBoothNavigatorService)
    {
        InitializeComponent();

        _db = db;
        _syncService = syncService;
        _narrationService = narrationService;
        _autoBoothNavigatorService = autoBoothNavigatorService;
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
            await DisplayAlertAsync("Lỗi", ex.ToString(), "OK");
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
                x.NameEn.ToLowerInvariant().Contains(keyword))
            .Select(x => new BoothCardVm
            {
                Id = x.Id,
                Title = x.NameVi,
                Subtitle = x.NameEn,
                ZoneText = x.ZoneId,
                PriorityText = $"Priority {x.Priority}",
                RadiusText = $"{x.RadiusMeters}m",
                ImageUrl = string.IsNullOrWhiteSpace(x.ImageUrl) ? "dotnet_bot.png" : x.ImageUrl,
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
            await DisplayAlertAsync("Lỗi sync", ex.ToString(), "OK");
        }
    }

    private async void OnGpsModeClicked(object sender, EventArgs e)
    {
        try
        {
            if (!_gpsModeEnabled)
            {
                var ok = await _autoBoothNavigatorService.StartAsync();
                if (!ok)
                {
                    await DisplayAlertAsync("GPS Mode", "Không bật được GPS hoặc chưa cấp quyền vị trí.", "OK");
                    return;
                }

                _gpsModeEnabled = true;
                SyncStatusLabel.Text = "GPS Mode: ĐANG BẬT";
                await DisplayAlertAsync("GPS Mode", "Đã bật GPS mode. Đứng gần gian hàng khoảng 2 giây là app sẽ tự nhảy vào và thuyết minh.", "OK");
            }
            else
            {
                _autoBoothNavigatorService.Stop();
                _gpsModeEnabled = false;
                SyncStatusLabel.Text = "GPS Mode: ĐÃ TẮT";
                await DisplayAlertAsync("GPS Mode", "Đã tắt GPS mode.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Lỗi GPS Mode", ex.ToString(), "OK");
        }
    }

    private async void OnMapClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("mappage");
    }

    private async void OnOpenBoothClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string boothId)
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={boothId}");
    }

    private async void OnScanQrClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(QrScanPage));
    }

    private async void OnPreviewBoothClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string boothId)
        {
            var booth = await _db.GetBoothAsync(boothId);
            if (booth != null)
                await _narrationService.SpeakBoothAsync(booth, "Manual");
        }
    }
}
