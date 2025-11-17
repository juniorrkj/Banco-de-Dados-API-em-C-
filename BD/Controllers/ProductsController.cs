using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Estoque.Data;
using EstoqueBD.Models;

namespace Estoque.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController : ControllerBase{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db)=>_db =db;

    //GET /api/v1/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var userId = GetUserIdFromHeader();
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        return Ok(await _db.Products
            .Where(p => p.UserId == userId.Value)
            .OrderBy(p => p.Id)
            .ToListAsync());
    }
    
    //GET /api/v1/products/with-categories (INNER JOIN)
    [HttpGet("with-categories")]
    public async Task<ActionResult> GetAllWithCategories()
    {
        var userId = GetUserIdFromHeader();
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        var productsWithCategories = await _db.Products
            .Where(p => p.UserId == userId.Value)
            .Include(p => p.Categories)
                .ThenInclude(pc => pc.Category)
            .OrderBy(p => p.Id)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Quantity,
                Categories = p.Categories.Select(pc => new
                {
                    pc.Category!.Id,
                    pc.Category.Name,
                    pc.Category.Description
                })
            })
            .ToListAsync();

        return Ok(productsWithCategories);
    }
    
    //GET /api/v1/products/1(id)
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var userId = GetUserIdFromHeader();
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        var product = await _db.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId.Value);

        return product is { } p ? Ok(p) : NotFound();
    }
    //POST  /api/v1/products
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product p)
    {
        var userId = GetUserIdFromHeader();
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
            
        if(string.IsNullOrWhiteSpace(p.Name)) 
            return BadRequest(new { error = "Nome é obrigatório" });
            
        if(await _db.Products.AnyAsync(x => x.Name == p.Name && x.UserId == userId.Value)) 
            return Conflict(new {error = "Você já tem um produto com este nome"});

        p.UserId = userId.Value;
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof (GetById), new {id = p.Id}, p);
    }

    //PUT /api/v1/products/1(id)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product p)
    {
        var userId = GetUserIdFromHeader();
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
            
        var existingProduct = await _db.Products
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId.Value);

        if (existingProduct == null)
            return NotFound(new { error = "Produto não encontrado" });

        if(!string.IsNullOrWhiteSpace(p.Name) &&
            await _db.Products.AnyAsync(x => x.Name == p.Name && x.Id != id && x.UserId == userId.Value)){
                return Conflict(new {error = "Você já tem um produto com este nome"});
            }

        existingProduct.Name = p.Name;
        existingProduct.Description = p.Description;
        existingProduct.Price = p.Price;
        existingProduct.Quantity = p.Quantity;

        await _db.SaveChangesAsync();
        return Ok(existingProduct);
    } 

    // DELETE /api/v1/products/1(id)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete (int id)
    {
        var userId = GetUserIdFromHeader();
        if (userId == null) return Unauthorized(new { error = "Usuário não autenticado" });

        var p = await _db.Products
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId.Value);

        if(p is null) 
            return NotFound(new { error = "Produto não encontrado" });

        _db.Products.Remove(p);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // POST /api/v1/products/1/categories/2 - Adiciona produto à categoria
    [HttpPost("{productId:int}/categories/{categoryId:int}")]
    public async Task<IActionResult> AddToCategory(int productId, int categoryId)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product is null)
            return NotFound(new { error = "Produto não encontrado" });

        var category = await _db.Categories.FindAsync(categoryId);
        if (category is null)
            return NotFound(new { error = "Categoria não encontrada" });

        // Verifica se já existe o relacionamento
        var exists = await _db.ProductCategories
            .AnyAsync(pc => pc.ProductId == productId && pc.CategoryId == categoryId);
        
        if (exists)
            return Conflict(new { error = "Produto já está vinculado a esta categoria" });

        _db.ProductCategories.Add(new ProductCategory 
        { 
            ProductId = productId, 
            CategoryId = categoryId 
        });
        
        await _db.SaveChangesAsync();
        return Ok(new { message = "Produto adicionado à categoria com sucesso" });
    }

    // DELETE /api/v1/products/1/categories/2 - Remove produto da categoria
    [HttpDelete("{productId:int}/categories/{categoryId:int}")]
    public async Task<IActionResult> RemoveFromCategory(int productId, int categoryId)
    {
        var relation = await _db.ProductCategories
            .FirstOrDefaultAsync(pc => pc.ProductId == productId && pc.CategoryId == categoryId);
        
        if (relation is null)
            return NotFound(new { error = "Relacionamento não encontrado" });

        _db.ProductCategories.Remove(relation);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Helper: Pegar UserId do header
    private int? GetUserIdFromHeader()
    {
        if (Request.Headers.TryGetValue("X-User-Id", out var userIdHeader))
        {
            if (int.TryParse(userIdHeader, out int userId))
            {
                return userId;
            }
        }
        return null;
    }
}
