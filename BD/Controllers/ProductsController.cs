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
        => Ok(await _db.Products.OrderBy(p=>p.Id).ToListAsync());
    
    //GET /api/v1/products/with-categories (INNER JOIN)
    [HttpGet("with-categories")]
    public async Task<ActionResult> GetAllWithCategories()
    {
        var productsWithCategories = await _db.Products
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
        => await _db.Products.FindAsync(id) is { } p ? Ok(p) : NotFound();

    //POST  /api/v1/products
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product p){
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
            
        if(string.IsNullOrWhiteSpace(p.Name)) 
            return BadRequest(new { error = "Nome é obrigatório" });
            
        if(await _db.Products.AnyAsync(x=>x.Name == p.Name)) 
            return Conflict(new {error = "Produto já existe"});
            
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof (GetById), new {id = p.Id}, p);
    }

    //PUT /api/v1/products/1(id)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product p){
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
            
        p.Id = id;

        if(!string.IsNullOrWhiteSpace(p.Name) &&
            await _db.Products.AnyAsync(x=>x.Name == p.Name && x.Id != id)){
                return Conflict(new {error = "Nome já cadastrado."});
            }

        if(!await _db.Products.AnyAsync(x=> x.Id == id)) 
            return NotFound(new { error = "Produto não encontrado" });

        _db.Entry(p).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return Ok(p);
    } 

    // DELETE /api/v1/products/1(id)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete (int id){
        var p = await _db.Products.FindAsync(id);
        if(p is null) 
            return NotFound(new { error = "Produto não encontrado" });

        _db.Products.Remove(p);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
