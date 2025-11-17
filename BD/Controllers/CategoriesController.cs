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
        => Ok(await _db.Categories.OrderBy(c => c.Id).ToListAsync());

    // GET /api/v1/categories/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        return category is not null ? Ok(category) : NotFound(new { error = "Categoria não encontrada" });
    }

    // POST /api/v1/categories
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
            
        if (string.IsNullOrWhiteSpace(category.Name))
            return BadRequest(new { error = "Nome é obrigatório" });

        if (await _db.Categories.AnyAsync(c => c.Name == category.Name))
            return Conflict(new { error = "Categoria já existe" });

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    // PUT /api/v1/categories/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Category category)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
            
        category.Id = id;

        if (!string.IsNullOrWhiteSpace(category.Name) &&
            await _db.Categories.AnyAsync(c => c.Name == category.Name && c.Id != id))
        {
            return Conflict(new { error = "Nome já cadastrado." });
        }

        if (!await _db.Categories.AnyAsync(c => c.Id == id))
            return NotFound(new { error = "Categoria não encontrada" });

        _db.Entry(category).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return Ok(category);
    }

    // DELETE /api/v1/categories/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category is null)
            return NotFound(new { error = "Categoria não encontrada" });

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
