using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Estoque.Data;

namespace EstoqueDB;

public class Program
{
    public static void Main(string[] args)
    {
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
        Environment.SetEnvironmentVariable("ASPNETCORE_URLS", $"http://0.0.0.0:{port}");
        
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlite("Data Source=estoque.db"));

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors("AllowAll");
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated(); // Cria o banco se não existir
        }

        Console.WriteLine("====================================");
        Console.WriteLine($"🚀 API rodando na porta {port}");
        Console.WriteLine("📚 Swagger: /swagger");
        Console.WriteLine("🌐 Interface: /");
        Console.WriteLine("====================================");

        app.Run();
    }
}
