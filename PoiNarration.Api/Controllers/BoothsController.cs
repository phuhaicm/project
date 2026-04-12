using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.DTOs.Booths;
using PoiNarration.Api.Services;
using PoiNarration.Core.Models;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoothsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ITranslationService _translationService;
    private readonly IQrCodeService _qrCodeService;
    public BoothsController(
    AppDbContext db,
    ITranslationService translationService,
    IQrCodeService qrCodeService)
    {
        _db = db;
        _translationService = translationService;
        _qrCodeService = qrCodeService;
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

        var booth = new Booth
        {
            Id = Guid.NewGuid().ToString(), // boothId gốc
            ZoneId = request.ZoneId,
            NameVi = request.NameVi,
            NameEn = "",
            DescVi = request.DescVi,
            DescEn = "",
            Lat = request.Lat,
            Lng = request.Lng,
            RadiusMeters = request.RadiusMeters,
            Priority = request.Priority,
            ImageUrl = request.ImageUrl ?? "",
            MapUrl = request.MapUrl,
            TtsScriptVi = request.TtsScriptVi,
            TtsScriptEn = null,
            AudioUrlVi = request.AudioUrlVi,
            AudioUrlEn = null,
            IsActive = request.IsActive,
            OwnerUserId = null
        };

        // Tạo QR từ đúng boothId gốc
        var qrCodeUrl = await _qrCodeService.GenerateAndSaveQrCodeAsync(booth.Id);

        // Nếu model Booth có cột QrCodeUrl
        booth.QrCodeUrl = qrCodeUrl;

        _db.Booths.Add(booth);

        var translations = await _translationService.BuildBoothTranslationsAsync(booth);
        if (translations.Any())
            _db.BoothTranslations.AddRange(translations);

        await _db.SaveChangesAsync();

        return Ok(new
        {
            booth.Id,
            booth.NameVi,
            booth.QrCodeUrl
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateBoothRequest request)
    {
        var booth = await _db.Booths.FirstOrDefaultAsync(x => x.Id == id);
        if (booth == null)
            return NotFound();

        booth.ZoneId = request.ZoneId;
        booth.NameVi = request.NameVi;
        booth.NameEn = "";
        booth.DescVi = request.DescVi;
        booth.DescEn = "";
        booth.Lat = request.Lat;
        booth.Lng = request.Lng;
        booth.RadiusMeters = request.RadiusMeters;
        booth.Priority = request.Priority;

        // GIỮ ẢNH CŨ NẾU REQUEST KHÔNG CÓ ẢNH MỚI
        if (!string.IsNullOrWhiteSpace(request.ImageUrl))
            booth.ImageUrl = request.ImageUrl;

        // GIỮ FIELD CŨ NẾU REQUEST RỖNG
        if (!string.IsNullOrWhiteSpace(request.MapUrl))
            booth.MapUrl = request.MapUrl;

        if (!string.IsNullOrWhiteSpace(request.TtsScriptVi))
            booth.TtsScriptVi = request.TtsScriptVi;

        if (!string.IsNullOrWhiteSpace(request.AudioUrlVi))
            booth.AudioUrlVi = request.AudioUrlVi;

        booth.TtsScriptEn = null;
        booth.AudioUrlEn = null;
        booth.IsActive = request.IsActive;

        var oldTranslations = _db.BoothTranslations.Where(x => x.BoothId == booth.Id);
        _db.BoothTranslations.RemoveRange(oldTranslations);

        var translationsToAdd = await _translationService.BuildBoothTranslationsAsync(booth);
        if (translationsToAdd.Any())
            _db.BoothTranslations.AddRange(translationsToAdd);

        await _db.SaveChangesAsync();

        var updatedTranslations = await _db.BoothTranslations
            .Where(x => x.BoothId == id)
            .ToListAsync();

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
    [HttpPost("{id}/generate-qr")]
    public async Task<IActionResult> GenerateQr(string id)
    {
        var booth = await _db.Booths.FirstOrDefaultAsync(x => x.Id == id);
        if (booth == null)
            return NotFound("Không tìm thấy booth.");

        var qrCodeUrl = await _qrCodeService.GenerateAndSaveQrCodeAsync(booth.Id);

        booth.QrCodeUrl = qrCodeUrl;
        await _db.SaveChangesAsync();

        return Ok(new
        {
            booth.Id,
            booth.QrCodeUrl
        });
    }
    [HttpPost("generate-qr-all")]
    public async Task<IActionResult> GenerateQrForAll()
    {
        var booths = await _db.Booths.ToListAsync();

        foreach (var booth in booths)
        {
            var qrCodeUrl = await _qrCodeService.GenerateAndSaveQrCodeAsync(booth.Id);
            booth.QrCodeUrl = qrCodeUrl;
        }

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Đã tạo QR cho tất cả booth.",
            count = booths.Count
        });
    }
}