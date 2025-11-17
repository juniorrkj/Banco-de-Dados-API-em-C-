using System.ComponentModel.DataAnnotations;

namespace EstoqueBD.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome de usuário é obrigatório")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Nome de usuário deve ter entre 3 e 50 caracteres")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string PasswordHash { get; set; } = string.Empty;

    // Relacionamentos
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
