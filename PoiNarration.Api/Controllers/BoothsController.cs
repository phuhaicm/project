using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;

namespace PoiNarration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoothsController : ControllerBase
{
    private readonly AppDbContext _db;
    public BoothsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Booths.ToListAsync());
}
