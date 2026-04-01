using Microsoft.AspNetCore.Mvc;

namespace PoiNarration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public MediaController(IWebHostEnvironment env)
        {
            _env = env; // Lấy thông tin thư mục của Server
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Chưa chọn file hình nào cả!");

            // Chỉ định đường dẫn tới thư mục wwwroot/images
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Tạo tên file ngẫu nhiên để không bị trùng lặp (ví dụ: abc-123_phobo.jpg)
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Copy file từ web vào ổ cứng của Server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về cái Link URL để lát Mobile lấy ảnh hiện lên App
            var imageUrl = $"/images/{uniqueFileName}";
            return Ok(new { Url = imageUrl });
        }
    }
}