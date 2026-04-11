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


    private readonly ApiService _apiService;

    public BoothListPage(
        AppDatabase db,
        SyncService syncService,
        NarrationService narrationService,
        AutoBoothNavigatorService autoBoothNavigatorService,
        ApiService apiService)
    {
        InitializeComponent();

        _db = db;
        _syncService = syncService;
        _narrationService = narrationService;
        _autoBoothNavigatorService = autoBoothNavigatorService;
        _apiService = apiService;
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
        await ApplyFilterAsync();
    }

    private async Task ApplyFilterAsync()
    {
        var keyword = BoothSearchBar.Text?.Trim().ToLowerInvariant() ?? "";
        var lang = LanguageService.CurrentLanguage;

        var filtered = new List<BoothCardVm>();

        foreach (var booth in _allBooths)
        {
            var translation = await _db.GetBoothTranslationAsync(booth.Id, lang)
                             ?? await _db.GetBoothTranslationAsync(booth.Id, "en")
                             ?? await _db.GetBoothTranslationAsync(booth.Id, "vi");

            var title = translation?.Name
                        ?? (LanguageService.IsVi ? booth.NameVi : booth.NameEn);

            var subtitle = translation?.Description
                           ?? (LanguageService.IsVi ? booth.DescVi : booth.DescEn);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                if (!title.ToLowerInvariant().Contains(keyword) &&
                    !subtitle.ToLowerInvariant().Contains(keyword))
                {
                    continue;
                }
            }

            filtered.Add(new BoothCardVm
            {
                Id = booth.Id,
                Title = title,
                Subtitle = subtitle,
                ZoneText = booth.ZoneId,
                PriorityText = $"{(LanguageService.IsVi ? "Ưu tiên" : "Priority")} {booth.Priority}",
                RadiusText = $"{booth.RadiusMeters}m",
                ImageUrl = _apiService.ResolveMediaUrl(booth.ImageUrl),

                IsActive = booth.IsActive,
                DetailText = LanguageService.IsVi ? "Chi tiết" : "Details",
                PreviewText = LanguageService.IsVi ? "Nghe thử" : "Preview"
            });
        }

        BoothsCollectionView.ItemsSource = filtered;
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        await ApplyFilterAsync();
    }

    private async void OnLanguageChanged(object sender, EventArgs e)
    {
        if (LanguagePicker.SelectedItem is string lang)
        {
            LanguageService.Set(lang);
            CurrentLanguageLabel.Text = lang;
            await ApplyFilterAsync();
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

                _autoBoothNavigatorService.SetAutoNarrationEnabled(true);

                _gpsModeEnabled = true;
                SyncStatusLabel.Text = "GPS Mode: ĐANG BẬT AUTO";

                await DisplayAlertAsync(
                    "GPS Mode",
                    "Đã bật GPS mode. Đứng gần gian hàng khoảng 2 giây là app sẽ tự nhảy vào và thuyết minh.",
                    "OK");
            }
            else
            {
                _autoBoothNavigatorService.SetAutoNarrationEnabled(true);

                _gpsModeEnabled = false;
                SyncStatusLabel.Text = "GPS Mode: CHỈ THEO DÕI";

                await DisplayAlertAsync(
                    "GPS Mode",
                    "Đã tắt tự động thuyết minh. Hệ thống vẫn tiếp tục theo dõi vị trí của bạn.",
                    "OK");
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
