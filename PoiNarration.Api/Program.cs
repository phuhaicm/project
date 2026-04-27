using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;
using PoiNarration.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký Controllers
builder.Services.AddControllers();
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();

// 2. CORS
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

// 4. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddOpenApi();

var app = builder.Build();

// 5. Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PoiNarration API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles();
// app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

// 6. Seed Data + QR
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await DataSeeder.SeedAsync(db);         // seed dữ liệu gốc của hệ thống
    await DemoDataSeeder.SeedAsync(db);     // seed visitor + visit log + playback log demo

    var qrService = scope.ServiceProvider.GetRequiredService<IQrCodeService>();
    var appDownloadUrl = "http://192.168.1.237:7269/AppDownload";
    await qrService.GenerateAndSaveAppDownloadQrAsync(appDownloadUrl);
}

app.Run();