using PoiNarration.Mobile.Services;
using ZXing.Net.Maui;
using PoiNarration.Core.Models;

namespace PoiNarration.Mobile.Views;

public partial class QrScanPage : ContentPage
{
    private readonly AppDatabase _db;
    private readonly ApiService _apiService;
    private readonly NarrationService _narrationService;

    // Biến chống spam quét 1 mã nhiều lần liên tục
    private bool _isProcessing = false;

    public QrScanPage(AppDatabase db, ApiService apiService, NarrationService narrationService)
    {
        InitializeComponent();
        _db = db;
        _apiService = apiService;
        _narrationService = narrationService;

        // ĐÃ ĐỔI TÊN THÀNH QrCameraView
        QrCameraView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.TwoDimensional,
            AutoRotate = true,
            Multiple = false
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _isProcessing = false;
        QrCameraView.IsDetecting = true; // Bật camera
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        QrCameraView.IsDetecting = false; // Tắt camera
    }

    private void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (_isProcessing || !e.Results.Any()) return;

        var firstResult = e.Results.FirstOrDefault();
        if (firstResult == null) return;

        _isProcessing = true;
        QrCameraView.IsDetecting = false; // Tạm dừng camera

        var qrText = firstResult.Value;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var booth = await _db.GetBoothAsync(qrText);

            if (booth != null)
            {
                var visitLog = new BoothVisitLog
                {
                    VisitorUserId = Preferences.Get("visitor_id_server", ""),
                    BoothId = booth.Id,
                    TriggerType = "QR",
                    Language = LanguageService.CurrentLanguage,
                    VisitedAtUtc = DateTime.UtcNow,
                    SessionId = Preferences.Get("session_id", Guid.NewGuid().ToString()),
                    IsSynced = false
                };

                await _db.SaveBoothVisitLogAsync(visitLog);

                try
                {
                    await _apiService.PostBoothVisitLogAsync(visitLog);
                    await _db.MarkBoothVisitLogSyncedAsync(visitLog.Id);
                }
                catch
                {
                    // giữ local
                }

                await _narrationService.SpeakBoothAsync(booth, "QR");
                await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={booth.Id}&trigger=QR");
            }
            else
            {

                await DisplayAlertAsync(
    LanguageService.T("Ui_Alert_QrError"),
    LanguageService.T("Ui_Alert_QrInvalid"),
    LanguageService.T("Ui_Alert_TryAgain"));

                _isProcessing = false;
                QrCameraView.IsDetecting = true; // Mở lại camera nếu quét sai
            }
        });
    }
}