using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(op => op.UseMySql(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
));


builder.Services.AddCors(op =>
{
    op.AddPolicy(
        "AllowAll", policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>(); // Gọi DbContext của bạn
        if (context.Database.CanConnect())
        {
            Console.WriteLine("Ok");
        }
        else
        {
            Console.WriteLine("Ko ket noi dc");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"lỗi kết nối: {ex.Message}");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();