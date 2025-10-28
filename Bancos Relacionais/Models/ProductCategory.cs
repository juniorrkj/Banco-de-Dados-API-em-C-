namespace BancosRelacionais.Models;

public class ProductCategory
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}