using System.ComponentModel.DataAnnotations;

namespace EstoqueBD.Models;

public class Category
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 120 caracteres")]
    public string Name { get; set; } = "";
    
    [StringLength(500, ErrorMessage = "Descrição pode ter no máximo 500 caracteres")]
    public string? Description { get; set; }
    
    // Relacionamento com User
    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public List<ProductCategory> Products { get; set; } = new();
}
