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

    private readonly GpsModeStateService _gpsModeStateService;

    public BoothListPage(
        AppDatabase db,
        SyncService syncService,
        NarrationService narrationService,
        AutoBoothNavigatorService autoBoothNavigatorService,
        ApiService apiService,

        GpsModeStateService gpsModeStateService)
    {
        InitializeComponent();

        _db = db;
        _syncService = syncService;
        _narrationService = narrationService;
        _autoBoothNavigatorService = autoBoothNavigatorService;
        _apiService = apiService;
        _gpsModeStateService = gpsModeStateService;
        LanguageService.LanguageChanged += OnLanguageServiceChanged;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _db.InitAsync();
            LanguagePicker.SelectedItem = LanguageService.CurrentLanguage;
            RefreshLocalizedTexts();
            await LoadBoothsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(GetErrorTitleText(), ex.ToString(), GetOkText());
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        LanguageService.LanguageChanged -= OnLanguageServiceChanged;
    }
    private void OnLanguageServiceChanged()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            RefreshLocalizedTexts();
            await ApplyFilterAsync();
        });
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
            ? GetVisitorSyncPendingText()
            : GetVisitorSyncDoneText();
    }
    private string GetPageTitleText()
    {
        return LanguageService.CurrentLanguage switch
        {
            "en" => "Booth List",
            "zh" => "展位列表",
            "fr" => "Liste des stands",
            "ja" => "ブース一覧",
            "ko" => "부스 목록",
            "es" => "Lista de stands",
            "it" => "Elenco stand",
            "ru" => "Список стендов",
            _ => "Danh sách trạm"
        };
    }

    private string GetVisitorSyncPendingText()
    {
        return LanguageService.CurrentLanguage switch
        {
            "en" => "Not synced to server",
            "zh" => "尚未同步到服务器",
            "fr" => "Non synchronisé avec le serveur",
            "ja" => "サーバー未同期",
            "ko" => "서버와 아직 동기화되지 않음",
            "es" => "Aún no sincronizado con el servidor",
            "it" => "Non sincronizzato con il server",
            "ru" => "Еще не синхронизировано с сервером",
            _ => "Chưa đồng bộ server"
        };
    }

    private string GetVisitorSyncDoneText()
    {
        return LanguageService.CurrentLanguage switch
        {
            "en" => "Synced to server",
            "zh" => "已同步到服务器",
            "fr" => "Synchronisé avec le serveur",
            "ja" => "サーバーに同期済み",
            "ko" => "서버와 동기화됨",
            "es" => "Sincronizado con el servidor",
            "it" => "Sincronizzato con il server",
            "ru" => "Синхронизировано с сервером",
            _ => "Đã đồng bộ server"
        };
    }

    private string GetErrorTitleText()
    {
        return LanguageService.CurrentLanguage switch
        {
            "en" => "Error",
            "zh" => "错误",
            "fr" => "Erreur",
            "ja" => "エラー",
            "ko" => "오류",
            "es" => "Error",
            "it" => "Errore",
            "ru" => "Ошибка",
            _ => "Lỗi"
        };
    }

    private string GetOkText()
    {
        return LanguageService.CurrentLanguage switch
        {
            "zh" => "确定",
            "fr" => "OK",
            "ja" => "OK",
            "ko" => "확인",
            "es" => "Aceptar",
            "it" => "OK",
            "ru" => "ОК",
            _ => "OK"
        };
    }

    private string GetSyncSuccessWithBoothCountText(int boothCount)
    {
        return LanguageService.CurrentLanguage switch
        {
            "en" => $"Sync successful: {boothCount} booths",
            "zh" => $"同步成功：{boothCount} 个展位",
            "fr" => $"Synchronisation réussie : {boothCount} stands",
            "ja" => $"同期成功：{boothCount} ブース",
            "ko" => $"동기화 성공: 부스 {boothCount}개",
            "es" => $"Sincronización correcta: {boothCount} stands",
            "it" => $"Sincronizzazione riuscita: {boothCount} stand",
            "ru" => $"Синхронизация выполнена: {boothCount} стендов",
            _ => $"Đồng bộ thành công: {boothCount} booth"
        };
    }

    private void RefreshLocalizedTexts()
    {
        Title = GetPageTitleText();
        CurrentLanguageLabel.Text = LanguageService.CurrentLanguage;
        RefreshVisitorInfo();
    }
    private async Task LoadBoothsAsync()
    {
        _allBooths = await _db.GetAllBoothsAsync();
        TotalBoothsLabel.Text = _allBooths.Count.ToString();
        SyncStatusLabel.Text = GetSyncSuccessWithBoothCountText(_allBooths.Count);
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

            await UpdateVisitorLanguageAsync(lang);
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
            if (!_gpsModeStateService.IsEnabled)
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
                _gpsModeStateService.SetEnabled(true);

                SyncStatusLabel.Text = $"{LanguageService.T("Ui_GpsMode")}: {LanguageService.T("Ui_GpsAutoEnabled")}";

                await DisplayAlertAsync(
                    LanguageService.T("Ui_GpsMode"),
                    GetGpsEnabledMessage(),
                    LanguageService.T("Ui_Alert_Ok"));

                // Chuyển sang tab Map ở dưới
                await Shell.Current.GoToAsync("//MapPage");
            }
            else
            {
                _autoBoothNavigatorService.SetAutoNarrationEnabled(false);
                _autoBoothNavigatorService.Stop();

                _gpsModeEnabled = false;
                _gpsModeStateService.SetEnabled(false);

                SyncStatusLabel.Text = $"{LanguageService.T("Ui_GpsMode")}: {LanguageService.T("Ui_GpsManualEnabled")}";

                await DisplayAlertAsync(
                    LanguageService.T("Ui_GpsMode"),
                    GetGpsDisabledMessage(),
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
    private async Task UpdateVisitorLanguageAsync(string lang)
    {
        var visitorId = Preferences.Get("visitor_id_server", "");
        if (string.IsNullOrWhiteSpace(visitorId))
            return;

        try
        {
            await _apiService.UpdateVisitorLanguageAsync(visitorId, lang);
        }
        catch
        {
            // nếu lỗi thì bỏ qua, app vẫn chạy bình thường
        }
    }
    private string GetGpsEnabledMessage()
    {
        return LanguageService.CurrentLanguage switch
        {
            "en" => "GPS has been enabled.",
            "fr" => "Le GPS a été activé.",
            "zh" => "已开启 GPS。",
            _ => "Đã bật GPS."
        };
    }

    private string GetGpsDisabledMessage()
    {
        return LanguageService.CurrentLanguage switch
        {
            "en" => "GPS has been disabled.",
            "fr" => "Le GPS a été désactivé.",
            "zh" => "已关闭 GPS。",
            _ => "Đã tắt GPS."
        };
    }

}
