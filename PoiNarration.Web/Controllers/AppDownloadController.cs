using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
        var apkPath = Path.Combine(_environment.WebRootPath, "downloads", "PoiNarration.apk");

        string apkVersion = "0";
        if (System.IO.File.Exists(apkPath))
        {
            apkVersion = System.IO.File.GetLastWriteTimeUtc(apkPath).Ticks.ToString();
        }

        var publicApiBaseUrl = _configuration["Api:PublicBaseUrl"]?.TrimEnd('/');

        ViewBag.ApkUrl = Url.Action("Apk", "AppDownload", new { v = apkVersion });
        ViewBag.QrImageUrl = string.IsNullOrWhiteSpace(publicApiBaseUrl)
            ? ""
            : $"{publicApiBaseUrl}/uploads/qrcodes/qr-app-download.png?v={apkVersion}";
        ViewBag.ApkVersion = apkVersion;

        // Chống cache trang index
        Response.Headers[HeaderNames.CacheControl] = "no-store, no-cache, must-revalidate, max-age=0";
        Response.Headers[HeaderNames.Pragma] = "no-cache";
        Response.Headers[HeaderNames.Expires] = "0";

        return View();
    }

    [HttpGet("apk")]
    public IActionResult Apk(string? v)
    {
        var apkPath = Path.Combine(_environment.WebRootPath, "downloads", "PoiNarration.apk");

        if (!System.IO.File.Exists(apkPath))
        {
            return NotFound($"Không tìm thấy file APK tại: {apkPath}");
        }

        var version = System.IO.File.GetLastWriteTimeUtc(apkPath).ToString("yyyyMMddHHmmss");
        var downloadFileName = $"PoiNarration_{version}.apk";

        // Chống cache file APK
        Response.Headers[HeaderNames.CacheControl] = "no-store, no-cache, must-revalidate, max-age=0";
        Response.Headers[HeaderNames.Pragma] = "no-cache";
        Response.Headers[HeaderNames.Expires] = "0";

        return PhysicalFile(
            apkPath,
            "application/vnd.android.package-archive",
            downloadFileName);
    }
}
