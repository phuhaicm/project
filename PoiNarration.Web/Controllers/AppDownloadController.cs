using Microsoft.AspNetCore.Mvc;

namespace PoiNarration.Web.Controllers;

[Route("AppDownload")]
public class AppDownloadController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public AppDownloadController(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var publicApiBaseUrl = _configuration["Api:PublicBaseUrl"]?.TrimEnd('/');

        ViewBag.ApkUrl = Url.Action("Apk", "AppDownload");
        ViewBag.QrImageUrl = string.IsNullOrWhiteSpace(publicApiBaseUrl)
     ? ""
     : $"{publicApiBaseUrl}/uploads/qrcodes/qr-app-download.png";

        return View();
    }

    [HttpGet("apk")]
    public IActionResult Apk()
    {
        var apkPath = Path.Combine(_environment.WebRootPath, "downloads", "PoiNarration.apk");

        if (!System.IO.File.Exists(apkPath))
        {
            return NotFound($"Không tìm thấy file APK tại: {apkPath}");
        }

        return PhysicalFile(
            apkPath,
            "application/vnd.android.package-archive",
            "PoiNarration.apk");
    }
}