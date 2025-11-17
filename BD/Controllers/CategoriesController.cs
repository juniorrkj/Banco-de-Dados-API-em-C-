using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Estoque.Data;
using EstoqueBD.Models;

namespace Estoque.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;

    // GET /api/v1/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        var userId = GetUserIdFromHeader();
        Console.WriteLine($"[Categories GET] UserId recebido: {userId}");
        Console.WriteLine($"[Categories GET] Headers: {string.Join(", ", Request.Headers.Select(h => $"{h.Key}={h.Value}"))}");
        
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        return Ok(await _db.Categories
            .Where(c => c.UserId == userId.Value)
            .OrderBy(c => c.Id)
            .ToListAsync());
    }

    // GET /api/v1/categories/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var userId = GetUserIdFromHeader();
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        var category = await _db.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);

        return category is not null ? Ok(category) : NotFound(new { error = "Categoria não encontrada" });
    }

    // POST /api/v1/categories
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        var userId = GetUserIdFromHeader();
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
            
        if (string.IsNullOrWhiteSpace(category.Name))
            return BadRequest(new { error = "Nome é obrigatório" });

        if (await _db.Categories.AnyAsync(c => c.Name == category.Name && c.UserId == userId.Value))
            return Conflict(new { error = "Você já tem uma categoria com este nome" });

        category.UserId = userId.Value;
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    // PUT /api/v1/categories/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        var userId = GetUserIdFromHeader();
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var existingCategory = await _db.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);

        if (existingCategory == null)
            return NotFound(new { error = "Categoria não encontrada" });

        if (!string.IsNullOrWhiteSpace(category.Name) &&
            await _db.Categories.AnyAsync(c => c.Name == category.Name && c.Id != id && c.UserId == userId.Value))
        {
            return Conflict(new { error = "Você já tem uma categoria com este nome" });
        }

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;

        await _db.SaveChangesAsync();
        return Ok(existingCategory);
    }

    // DELETE /api/v1/categories/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserIdFromHeader();
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        var category = await _db.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);

        if (category is null)
            return NotFound(new { error = "Categoria não encontrada" });

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Helper: Pegar UserId do header
    private int? GetUserIdFromHeader()
    {
        // Tentar diferentes variações do header
        string[] headerVariations = { "X-User-Id", "x-user-id", "X-USER-ID" };
        
        foreach (var headerName in headerVariations)
        {
            if (Request.Headers.TryGetValue(headerName, out var userIdHeader))
            {
                Console.WriteLine($"[GetUserIdFromHeader] Header '{headerName}' encontrado: '{userIdHeader}'");
                if (int.TryParse(userIdHeader, out int userId))
                {
                    Console.WriteLine($"[GetUserIdFromHeader] UserId parseado: {userId}");
                    return userId;
                }
                Console.WriteLine($"[GetUserIdFromHeader] Falha ao parsear '{userIdHeader}'");
            }
        }
        
        Console.WriteLine($"[GetUserIdFromHeader] Nenhum header de UserId encontrado");
        Console.WriteLine($"[GetUserIdFromHeader] Headers disponíveis: {string.Join(", ", Request.Headers.Keys)}");
        return null;
    }
}
