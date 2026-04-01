using Microsoft.EntityFrameworkCore;
using PoiNarration.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Thêm các dịch vụ cần thiết
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Bật Swagger
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=poi.db"));

var app = builder.Build();

// Cấu hình môi trường chạy
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Bật giao diện Swagger UI
}

app.UseStaticFiles(); // Cho phép server mở thư mục chứa hình ảnh

// 🛑 ĐÃ KHÓA MÕM KẺ PHẢN DIỆN: Không ép chuyển sang HTTPS nữa!
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();
app.Run();