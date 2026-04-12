using QRCoder;

namespace PoiNarration.Api.Services;

public class QrCodeService : IQrCodeService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public QrCodeService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> GenerateAndSaveQrCodeAsync(string boothId)
    {
        if (string.IsNullOrWhiteSpace(boothId))
            throw new ArgumentException("boothId không hợp lệ.");

        // QR chỉ chứa đúng boothId gốc
        string qrData = boothId;

        // Tạo thư mục nếu chưa có
        string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "qrcodes");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);

        var pngQrCode = new PngByteQRCode(qrCodeData);
        byte[] qrBytes = pngQrCode.GetGraphic(20);

        string fileName = $"qr-{boothId}.png";
        string filePath = Path.Combine(folderPath, fileName);

        await File.WriteAllBytesAsync(filePath, qrBytes);

        // Trả về URL tương đối để lưu DB hoặc hiển thị
        return $"/uploads/qrcodes/{fileName}";
    }
}
