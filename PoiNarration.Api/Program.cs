using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PoiNarration.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. CẤU HÌNH SERVICES
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PoiNarration.Api.Data.AppDbContext>(options =>
    options.UseInMemoryDatabase("PoiTestDb"));

var app = builder.Build();

// 2. SEED DATA (Nạp dữ liệu mẫu vào RAM)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PoiNarration.Api.Data.AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.BoothMenuItems.Any())
    {
        db.BoothMenuItems.AddRange(
            new BoothMenuItem { Id = Guid.NewGuid().ToString(), BoothId = "123", Name = "Phở Bò Kobe", Price = 150000, Description = "Ngon nhức nách", ImageUrl = "https://bit.ly/3T9zvXz" },
            new BoothMenuItem { Id = Guid.NewGuid().ToString(), BoothId = "123", Name = "Cà Phê Muối", Price = 35000, Description = "Đậm đà vị biển", ImageUrl = "https://bit.ly/49X7vF8" }
        );
        db.SaveChanges();
    }
}

// 3. CẤU HÌNH MIDDLEWARE (Thứ tự phải chuẩn)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cấu hình file tĩnh (wwwroot)
var webRootPath = builder.Environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(webRootPath)) Directory.CreateDirectory(webRootPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(webRootPath),
    RequestPath = ""
});

app.UseAuthorization();

// 4. KẾT NỐI CONTROLLERS (Cực kỳ quan trọng để hết lỗi 404)
app.MapControllers();

// 5. CHẠY SERVER (Dòng này luôn luôn nằm CUỐI CÙNG)
app.Run("http://0.0.0.0:5174");