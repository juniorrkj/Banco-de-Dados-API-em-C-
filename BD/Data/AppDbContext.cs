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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=estoque.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Product
    modelBuilder.Entity<Product>(e => {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(120);
            e.Property(p => p.Description).IsRequired().HasMaxLength(500);
            e.Property(p => p.Price).IsRequired().HasPrecision(10, 2);
            e.Property(p => p.Quantity).IsRequired();
            e.HasIndex(p => p.Name).IsUnique();
        });

        // Category
    modelBuilder.Entity<Category>(e => {
            e.HasKey(c => c.Id);
            e.Property(c => c.Name).IsRequired().HasMaxLength(120);
            e.Property(c => c.Description).HasMaxLength(500);
            e.HasIndex(c => c.Name).IsUnique();
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
