using System.ComponentModel.DataAnnotations;

namespace EstoqueBD.Models;

public class Category
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = "";
    
    public string? Description { get; set; }
    
    public List<ProductCategory> Products { get; set; } = new();
}
