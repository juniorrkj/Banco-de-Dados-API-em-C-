using Microsoft.EntityFrameworkCore;
using EstoqueBD.Models;

namespace Estoque.Data;

public class AppDbContext : DbContext{
    public AppDbContext(){}
    public AppDbContext(DbContextOptions<AppDbContext> options) :base(options){}

    // Model types for this DbContext
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=estoque.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>(e => {
            e.HasKey(u => u.Id);
            e.Property(u => u.Username).IsRequired().HasMaxLength(50);
            e.Property(u => u.PasswordHash).IsRequired();
            e.HasIndex(u => u.Username).IsUnique();
        });

        // Product
        modelBuilder.Entity<Product>(e => {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(120);
            e.Property(p => p.Description).IsRequired().HasMaxLength(500);
            e.Property(p => p.Price).IsRequired().HasPrecision(10, 2);
            e.Property(p => p.Quantity).IsRequired();
            e.HasIndex(p => new { p.UserId, p.Name }).IsUnique();
            e.HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Category
        modelBuilder.Entity<Category>(e => {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired().HasMaxLength(120);
            e.Property(c => c.Description).HasMaxLength(500);
            e.HasIndex(c => new { c.UserId, c.Name }).IsUnique();
            e.HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ProductCategory (join N:N)
        modelBuilder.Entity<ProductCategory>(e => {
            e.HasKey(pc => new { pc.ProductId, pc.CategoryId });
            e.HasOne(pc => pc.Product)
                .WithMany(p => p.Categories)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(pc => pc.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            e.Property(pc => pc.AddedAt).IsRequired();
        });
    }
}
