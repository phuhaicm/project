using ZXing.Net.Maui;

namespace PoiNarration.Mobile.Views;

public partial class QRScanPage : ContentPage
{
    private bool _handled = false;

    public QRScanPage()
    {
        InitializeComponent();

        cameraView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.TwoDimensional,
            AutoRotate = true,
            Multiple = false
        };
    }

    private void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (_handled) return;

        var result = e.Results?.FirstOrDefault();
        if (result == null) return;

        var text = result.Value?.Trim();
        if (string.IsNullOrWhiteSpace(text)) return;

        _handled = true;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (text.StartsWith("booth:", StringComparison.OrdinalIgnoreCase))
            {
                var boothId = text.Substring("booth:".Length).Trim();

                if (!string.IsNullOrWhiteSpace(boothId))
                {
                    await Shell.Current.GoToAsync($"{nameof(BoothDetailPage)}?boothId={boothId}");
                    return;
                }
            }

            await DisplayAlertAsync("QR không hợp lệ", text, "OK");
            _handled = false;
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _handled = false;
    }
}