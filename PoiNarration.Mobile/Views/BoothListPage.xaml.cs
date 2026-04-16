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
            RefreshVisitorInfo();
            await LoadBoothsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Lỗi", ex.ToString(), "OK");
        }
    }

    private void RefreshVisitorInfo()
    {
        var visitorCode = Preferences.Get("visitor_code", "");
        var visitorServerId = Preferences.Get("visitor_id_server", "");
        var currentLang = LanguageService.CurrentLanguage;

        if (string.IsNullOrWhiteSpace(visitorCode))
            visitorCode = "VIS-LOCAL";

        VisitorCodeLabel.Text = visitorCode;
        VisitorLangLabel.Text = currentLang;
        VisitorSyncStatusLabel.Text = string.IsNullOrWhiteSpace(visitorServerId)
            ? "Chưa đồng bộ server"
            : "Đã đồng bộ server";
    }

    private async Task LoadBoothsAsync()
    {
        _allBooths = await _db.GetAllBoothsAsync();
        TotalBoothsLabel.Text = _allBooths.Count.ToString();
        SyncStatusLabel.Text = $"{LanguageService.T("Ui_SyncSuccess")}: {_allBooths.Count} booth";
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
                DetailText = LanguageService.T("Ui_Detail"),
                PreviewText = LanguageService.T("Ui_Preview"),
                ImageUrl = _apiService.ResolveMediaUrl(booth.ImageUrl),
                IsActive = booth.IsActive,
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
            RefreshVisitorInfo();
            await ApplyFilterAsync();
        }
    }

    private async void OnSyncClicked(object sender, EventArgs e)
    {
        try
        {
            SyncStatusLabel.Text = LanguageService.T("Ui_Syncing");
            await _syncService.SyncBootstrapAsync();
            await _syncService.SyncBoothVisitLogsAsync();
            await _syncService.SyncPlaybackLogsAsync();
            await LoadBoothsAsync();
            RefreshVisitorInfo();
            SyncStatusLabel.Text = LanguageService.T("Ui_SyncSuccess");
        }
        catch (Exception ex)
        {
            SyncStatusLabel.Text = LanguageService.T("Ui_SyncFailed");
            await DisplayAlertAsync(
                LanguageService.T("Ui_Alert_SyncError"),
                ex.ToString(),
                LanguageService.T("Ui_Alert_Ok"));
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
                    await DisplayAlertAsync(
                        LanguageService.T("Ui_GpsMode"),
                        LanguageService.T("Ui_GpsNotEnabledOrPermissionDenied"),
                        LanguageService.T("Ui_Alert_Ok"));
                    return;
                }

                _autoBoothNavigatorService.SetAutoNarrationEnabled(true);
                _gpsModeEnabled = true;
                SyncStatusLabel.Text = $"{LanguageService.T("Ui_GpsMode")}: {LanguageService.T("Ui_GpsAutoEnabled")}";
                await DisplayAlertAsync(
                    LanguageService.T("Ui_GpsMode"),
                    LanguageService.T("Ui_GpsAutoEnabledMessage"),
                    LanguageService.T("Ui_Alert_Ok"));
            }
            else
            {
                _autoBoothNavigatorService.SetAutoNarrationEnabled(false);
                _gpsModeEnabled = false;
                SyncStatusLabel.Text = $"{LanguageService.T("Ui_GpsMode")}: {LanguageService.T("Ui_GpsManualEnabled")}";
                await DisplayAlertAsync(
                    LanguageService.T("Ui_GpsMode"),
                    LanguageService.T("Ui_GpsManualEnabledMessage"),
                    LanguageService.T("Ui_Alert_Ok"));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                LanguageService.T("Ui_GpsModeError"),
                ex.ToString(),
                LanguageService.T("Ui_Alert_Ok"));
        }
    }

    private async void OnMapClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("mappage");
    }

    private async void OnOpenBoothClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string boothId)
            await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={boothId}&trigger=ManualOpen");
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
