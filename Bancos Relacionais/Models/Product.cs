using System.ComponentModel.DataAnnotations;

namespace BancosRelacionais.Models;

public class Product
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = "";
    
    [Required]
    public string Description { get; set; } = "";
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    public List<ProductCategory> Categories { get; set; } = new();
}