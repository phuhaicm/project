namespace PoiNarration.Api.Services;

public interface IQrCodeService
{
    Task<string> GenerateAndSaveQrCodeAsync(string boothId);
    Task<string> GenerateAndSaveAppDownloadQrAsync(string downloadUrl);
}