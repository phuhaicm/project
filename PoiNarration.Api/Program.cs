using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký Controllers
builder.Services.AddControllers();
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();

// 2. Cấu hình CORS (Chỉ cần 1 đoạn này là đủ, đừng viết lặp lại)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 3. DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. QUAN TRỌNG: Đăng ký Swagger (Swashbuckle) 
// Bạn phải có dòng này thì app.UseSwagger() mới không bị lỗi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Đây là dòng "cứu cánh" của bạn
    // Nó sẽ biến "LoginRequest" thành "PoiNarration.Api.DTOs.Auth.LoginRequest"
    options.CustomSchemaIds(type => type.FullName);

    // Nếu bạn muốn hiển thị ngắn gọn hơn nhưng vẫn tránh trùng, 
    // có thể dùng: type => type.ToString().Replace("+", ".")
});
// (Tùy chọn) Nếu bạn dùng .NET 9 và muốn dùng bộ OpenAPI mới
builder.Services.AddOpenApi();

var app = builder.Build();

// 5. Cấu hình Middleware cho Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Tạo ra file swagger.json
    app.UseSwaggerUI(c =>
    {
        // Đường dẫn chuẩn cho Swashbuckle
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PoiNarration API v1");
        c.RoutePrefix = "swagger";
    });
}

// 6. Thứ tự Middleware chuẩn
app.UseStaticFiles();
// app.UseHttpsRedirection(); // Tạm comment nếu bạn test http trên điện thoại thật cho dễ
app.UseCors();
app.MapControllers();

// 7. Seed Data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DataSeeder.SeedAsync(db);
}

app.Run();