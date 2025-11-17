using System.ComponentModel.DataAnnotations;

namespace EstoqueBD.Models;

public class Product
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 120 caracteres")]
    public string Name { get; set; } = "";
    
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "Descrição deve ter entre 5 e 500 caracteres")]
    public string Description { get; set; } = "";
    
    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0.01, 999999.99, ErrorMessage = "Preço deve estar entre R$ 0,01 e R$ 999.999,99")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Quantidade é obrigatória")]
    [Range(0, 999999, ErrorMessage = "Quantidade deve estar entre 0 e 999.999")]
    public int Quantity { get; set; }
    
    public List<ProductCategory> Categories { get; set; } = new();
}
