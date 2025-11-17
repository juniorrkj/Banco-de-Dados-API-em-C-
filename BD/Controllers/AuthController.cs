using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Estoque.Data;
using EstoqueBD.Models;
using System.Security.Cryptography;
using System.Text;

namespace EstoqueBD.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // DTO para Register
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // DTO para Login
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // DTO para Response
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    // POST: api/v1/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterDto dto)
    {
        // Validar se usuário já existe
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
        {
            return BadRequest(new { error = "Usuário já existe" });
        }

        // Validações
        if (string.IsNullOrWhiteSpace(dto.Username) || dto.Username.Length < 3)
        {
            return BadRequest(new { error = "Nome de usuário deve ter pelo menos 3 caracteres" });
        }

        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
        {
            return BadRequest(new { error = "Senha deve ter pelo menos 6 caracteres" });
        }

        // Criar hash da senha
        string passwordHash = HashPassword(dto.Password);

        // Criar usuário
        var user = new User
        {
            Username = dto.Username,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Created("", new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Message = "Usuário registrado com sucesso!"
        });
    }

    // POST: api/v1/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDto dto)
    {
        // Buscar usuário
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user == null)
        {
            return Unauthorized(new { error = "Usuário ou senha inválidos" });
        }

        // Verificar senha
        if (!VerifyPassword(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new { error = "Usuário ou senha inválidos" });
        }

        return Ok(new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Message = "Login realizado com sucesso!"
        });
    }

    // GET: api/v1/auth/admin/users?secret=SUA_SENHA_SECRETA
    [HttpGet("admin/users")]
    public async Task<ActionResult> GetAllUsersAdmin([FromQuery] string secret)
    {
        // Senha de administrador (mude para uma senha sua!)
        if (secret != "admin2024")
        {
            return Unauthorized(new { error = "Acesso negado" });
        }

        var users = await _context.Users
            .Select(u => new
            {
                u.Id,
                u.Username,
                ProductCount = u.Products.Count,
                CategoryCount = u.Categories.Count
            })
            .ToListAsync();

        return Ok(users);
    }

    // Helper: Hash de senha usando SHA256
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    // Helper: Verificar senha
    private bool VerifyPassword(string password, string passwordHash)
    {
        string hash = HashPassword(password);
        return hash == passwordHash;
    }
}
