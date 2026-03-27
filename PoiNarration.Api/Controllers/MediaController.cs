using Microsoft.AspNetCore.Mvc;

namespace PoiNarration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public MediaController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            // 1. Kiểm tra xem có file gửi lên không
            if (file == null || file.Length == 0)
                return BadRequest("Vui lòng chọn file ảnh hợp lệ.");

            // 2. Xác định thư mục lưu trữ (sẽ tự tạo thư mục wwwroot/images/menu nếu chưa có)
            string webRootPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            string uploadsFolder = Path.Combine(webRootPath, "images", "menu");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            // 3. Đổi tên file để không bị trùng (thêm một đoạn mã ngẫu nhiên vào trước tên gốc)
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // 4. Lưu file vật lý xuống ổ cứng
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 5. Trả về cái đường dẫn URL để lát nữa app Mobile lấy về hiển thị
            string imageUrl = $"/images/menu/{uniqueFileName}";
            return Ok(new { Url = imageUrl });
        }
    }
}