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

    // HỢP NHẤT CONSTRUCTOR: Nhận cả DB và Service
    public BoothsController(AppDbContext db, ITranslationService translationService)
    {
        _db = db;
        _translationService = translationService;
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
            Id = Guid.NewGuid().ToString(),
            ZoneId = request.ZoneId,
            NameVi = request.NameVi,
            NameEn = "", // API sẽ tự sinh translation EN
            DescVi = request.DescVi,
            DescEn = "", // API sẽ tự sinh translation EN
            Lat = request.Lat,
            Lng = request.Lng,
            RadiusMeters = request.RadiusMeters,
            Priority = request.Priority,
            ImageUrl = request.ImageUrl,
            MapUrl = request.MapUrl,
            TtsScriptVi = request.TtsScriptVi,
            TtsScriptEn = null,
            AudioUrlVi = request.AudioUrlVi,
            AudioUrlEn = null,
            IsActive = request.IsActive,
            OwnerUserId = null
        };

        _db.Booths.Add(booth);

        // SỬ DỤNG SERVICE ĐỂ BUILD TRANSLATIONS TỰ ĐỘNG
        var translations = await _translationService.BuildBoothTranslationsAsync(booth);
        if (translations != null && translations.Any())
        {
            _db.BoothTranslations.AddRange(translations);
        }

        await _db.SaveChangesAsync();
        return Ok(booth);
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
        booth.ImageUrl = request.ImageUrl;
        booth.MapUrl = request.MapUrl;
        booth.TtsScriptVi = request.TtsScriptVi;
        booth.TtsScriptEn = null;
        booth.AudioUrlVi = request.AudioUrlVi;
        booth.AudioUrlEn = null;
        booth.IsActive = request.IsActive;

        // XÓA BẢN DỊCH CŨ
        var oldTranslations = _db.BoothTranslations.Where(x => x.BoothId == booth.Id);
        _db.BoothTranslations.RemoveRange(oldTranslations);

        // SỬ DỤNG SERVICE ĐỂ BUILD LẠI BẢN DỊCH MỚI SAU KHI UPDATE
        var translationsToAdd = await _translationService.BuildBoothTranslationsAsync(booth);
        if (translationsToAdd != null && translationsToAdd.Any())
        {
            _db.BoothTranslations.AddRange(translationsToAdd);
        }

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
}