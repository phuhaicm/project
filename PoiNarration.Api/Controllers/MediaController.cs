using Microsoft.AspNetCore.Mvc;
using PoiNarration.Api.DTOs.Media;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public MediaController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<UploadMediaResponse>> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Không có file.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(ext))
            return BadRequest("Định dạng file không hợp lệ.");

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File quá lớn. Giới hạn 5MB.");

        var webRoot = _env.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
        {
            webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        var uploadFolder = Path.Combine(webRoot, "uploads", "menu");
        Directory.CreateDirectory(uploadFolder);

        var safeFileName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploadFolder, safeFileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var fileUrl = $"{baseUrl}/uploads/menu/{safeFileName}";

        return Ok(new UploadMediaResponse
        {
            Url = fileUrl
        });
    }
}