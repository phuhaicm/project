using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.DTOs.Booths;
using PoiNarration.Api.Models.Entities;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoothsController : ControllerBase
{
    private readonly AppDbContext _db;

    public BoothsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var booths = await _db.Booths
            .OrderBy(x => x.Priority)
            .ToListAsync();

        return Ok(booths);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var booth = await _db.Booths.FirstOrDefaultAsync(x => x.Id == id);
        if (booth == null)
            return NotFound();

        return Ok(booth);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBoothRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NameVi))
            return BadRequest("Tên booth tiếng Việt là bắt buộc.");

        var booth = new PoiNarration.Api.Models.Entities.Booth
        {
            Id = Guid.NewGuid().ToString(),
            ZoneId = request.ZoneId,
            NameVi = request.NameVi,
            NameEn = request.NameEn,
            DescVi = request.DescVi,
            DescEn = request.DescEn,
            Lat = request.Lat,
            Lng = request.Lng,
            RadiusMeters = request.RadiusMeters,
            Priority = request.Priority,
            ImageUrl = request.ImageUrl,
            MapUrl = request.MapUrl,
            TtsScriptVi = request.TtsScriptVi,
            TtsScriptEn = request.TtsScriptEn,
            AudioUrlVi = request.AudioUrlVi,
            AudioUrlEn = request.AudioUrlEn,
            IsActive = request.IsActive,
            OwnerUserId = null
        };

        _db.Booths.Add(booth);
        if (request.Translations != null && request.Translations.Any())
        {
            var translations = request.Translations.Select(x => new BoothTranslation
            {
                BoothId = booth.Id,
                LanguageCode = x.LanguageCode,
                Name = x.Name,
                Description = x.Description,
                TtsScript = x.TtsScript,
                AudioUrl = x.AudioUrl
            });

            _db.BoothTranslations.AddRange(translations);
        }
        await _db.SaveChangesAsync();

        return Ok(booth);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateBoothRequest request)
    {
        // 1. Tìm Booth hiện tại trong Database
        var booth = await _db.Booths.FirstOrDefaultAsync(x => x.Id == id);
        if (booth == null)
            return NotFound();

        // 2. Cập nhật các thông tin cơ bản của Booth
        booth.ZoneId = request.ZoneId;
        booth.NameVi = request.NameVi;
        booth.NameEn = request.NameEn;
        booth.DescVi = request.DescVi;
        booth.DescEn = request.DescEn;
        booth.Lat = request.Lat;
        booth.Lng = request.Lng;
        booth.RadiusMeters = request.RadiusMeters;
        booth.Priority = request.Priority;
        booth.ImageUrl = request.ImageUrl; // Đảm bảo gán cả ImageUrl nếu request có
        booth.MapUrl = request.MapUrl;
        booth.TtsScriptVi = request.TtsScriptVi;
        booth.TtsScriptEn = request.TtsScriptEn;
        booth.AudioUrlVi = request.AudioUrlVi;
        booth.AudioUrlEn = request.AudioUrlEn;
        booth.IsActive = request.IsActive;

        // 3. XỬ LÝ BẢN DỊCH (Đoạn code bạn muốn thêm)
        // Xóa các bản dịch cũ của Booth này để thay thế bằng dữ liệu mới
        var oldTranslations = _db.BoothTranslations.Where(x => x.BoothId == booth.Id);
        _db.BoothTranslations.RemoveRange(oldTranslations);

        // Thêm các bản dịch mới từ request nếu có
        if (request.Translations != null && request.Translations.Any())
        {
            var translationsToAdd = request.Translations.Select(x => new BoothTranslation
            {
                BoothId = booth.Id,
                LanguageCode = x.LanguageCode,
                Name = x.Name,
                Description = x.Description,
                TtsScript = x.TtsScript,
                AudioUrl = x.AudioUrl
            });

            _db.BoothTranslations.AddRange(translationsToAdd);
        }

        // 4. Lưu tất cả thay đổi vào Database
        await _db.SaveChangesAsync();

        // 5. Lấy lại danh sách bản dịch mới nhất để trả về (Đoạn code bạn muốn thêm)
        var updatedTranslations = await _db.BoothTranslations
            .Where(x => x.BoothId == id)
            .ToListAsync();

        // Trả về kết quả theo cấu trúc bạn yêu cầu
        return Ok(new
        {
            booth.Id,
            booth.ZoneId,
            booth.NameVi,
            booth.NameEn,
            booth.DescVi,
            booth.DescEn,
            booth.Lat,
            booth.Lng,
            booth.RadiusMeters,
            booth.Priority,
            booth.OwnerUserId,
            booth.ImageUrl,
            booth.MapUrl,
            booth.IsActive,
            Translations = updatedTranslations
        });
    }
}