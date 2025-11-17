
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Estoque.Data;
using EstoqueBD.Models;

namespace EstoqueDB;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Quick seed/test mode: dotnet run --project BD/EstoqueDB.csproj -- --seed
        if (args is not null && args.Contains("--seed"))
        {
            Console.WriteLine("Running seed mode (--seed)...");
            using (var db = new AppDbContext())
            {
                // Ensure database is created and migrations applied
                db.Database.Migrate();

                var prod = new Product { Name = "Seed Product", Description = "Criado por seed", Price = 1.23M, Quantity = 5 };
                if (!db.Products.Any(p => p.Name == prod.Name))
                {
                    db.Products.Add(prod);
                    db.SaveChanges();
                    Console.WriteLine($"Produto criado: Id={prod.Id}");
                }

                var cat = new Category { Name = "Seed Cat", Description = "Categoria seed" };
                if (!db.Categories.Any(c => c.Name == cat.Name))
                {
                    db.Categories.Add(cat);
                    db.SaveChanges();
                    Console.WriteLine($"Categoria criada: Id={cat.Id}");
                }

                var pv = db.Products.First(p => p.Name == prod.Name);
                var cv = db.Categories.First(c => c.Name == cat.Name);
                if (!db.ProductCategories.Any(pc => pc.ProductId == pv.Id && pc.CategoryId == cv.Id))
                {
                    db.ProductCategories.Add(new ProductCategory { ProductId = pv.Id, CategoryId = cv.Id });
                    db.SaveChanges();
                    Console.WriteLine($"Relacionamento criado: ProductId={pv.Id}, CategoryId={cv.Id}");
                }
            }
            return;
        }
    var builder = WebApplication.CreateBuilder(args ?? Array.Empty<string>());

        // Configurar porta (Railway usa PORT env var, localmente usa 5099)
        var port = Environment.GetEnvironmentVariable("PORT") ?? "5099";
        builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

        // Configurar CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlite("Data Source=estoque.db"));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Habilitar CORS
        app.UseCors("AllowAll");

        // Habilitar arquivos estáticos (HTML, CSS, JS)
        // UseDefaultFiles PRECISA vir ANTES de UseStaticFiles
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        var webTask = app.RunAsync();
        var isRailway = Environment.GetEnvironmentVariable("PORT") != null;
        Console.WriteLine("===================================");
        Console.WriteLine($"API online em {(isRailway ? "Railway" : "http://localhost:5099")}");
        Console.WriteLine("Swagger: /swagger");
        Console.WriteLine("Interface Web (GUI): /");
        Console.WriteLine("===================================");

        // Se estiver no Railway (variável PORT existe) ou WEBONLY=1, não mostrar menu
        if (isRailway || Environment.GetEnvironmentVariable("WEBONLY") == "1")
        {
            Console.WriteLine("Running in web-only mode — console menu disabled.");
            await webTask;
            return;
        }

        Console.WriteLine("== Sistema de Controle de Estoque ==");
        Console.WriteLine("Console + API executando juntos!");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1 - Cadastrar produto");
            Console.WriteLine("2 - Listar produtos");
            Console.WriteLine("3 - Atualizar produto (por Id)");
            Console.WriteLine("4 - Remover produto (por Id)");
            Console.WriteLine("5 - Cadastrar categoria");
            Console.WriteLine("6 - Adicionar produto à categoria");
            Console.WriteLine("7 - Listar categorias e produtos");
            Console.WriteLine("8 - Listar produtos por categoria");
            Console.WriteLine("9 - Atualizar estoque");
            Console.WriteLine("0 - Sair");
            Console.Write("> ");

            var opt = Console.ReadLine();

            if (opt == "0") break;

            switch (opt)
            {
                case "1": await CreateProductAsync(); break;
                case "2": await ListProductsAsync(); break;
                case "3": await UpdateProductAsync(); break;
                case "4": await DeleteProductAsync(); break;
                case "5": await CreateCategoryAsync(); break;
                case "6": await AddProductToCategoryAsync(); break;
                case "7": await ListCategoriesWithProductsAsync(); break;
                case "8": await ListProductsByCategoryAsync(); break;
                case "9": await UpdateStockAsync(); break;
                default: Console.WriteLine("Opção inválida."); break; 
            }
        }

        await app.StopAsync();
        await webTask;
    }

    static async Task CreateProductAsync()
    {
        Console.Write("Nome do produto: ");
        var name = (Console.ReadLine() ?? "").Trim();

        Console.Write("Descrição: ");
        var description = (Console.ReadLine() ?? "").Trim();

        Console.Write("Preço: ");
        if (!decimal.TryParse(Console.ReadLine(), out var price) || price <= 0)
        {
            Console.WriteLine("Preço inválido.");
            return;
        }

        Console.Write("Quantidade inicial: ");
        if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity < 0)
        {
            Console.WriteLine("Quantidade inválida.");
            return;
        }

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Nome e descrição são obrigatórios.");
            return;
        }

        using var db = new AppDbContext();
        var exists = await db.Products.AnyAsync(p => p.Name == name);
        if (exists) { Console.WriteLine("Já existe um produto com esse nome."); return; }

    var product = new Product 
        { 
            Name = name, 
            Description = description, 
            Price = price,
            Quantity = quantity 
        };
        db.Products.Add(product);
        await db.SaveChangesAsync();
        Console.WriteLine($"Produto cadastrado com sucesso! Id: {product.Id}");
    }

    static async Task ListProductsAsync()
    {
        using var db = new AppDbContext();
        var products = await db.Products.OrderBy(p => p.Id).ToListAsync();

        if (products.Count == 0) { Console.WriteLine("Nenhum produto encontrado."); return; }

        Console.WriteLine("Id | Nome                 | Preço      | Quantidade | Descrição");
        Console.WriteLine("---+----------------------+------------+------------+------------------");
        foreach (var p in products)
            Console.WriteLine($"{p.Id,2} | {p.Name,-20} | {p.Price,10:C} | {p.Quantity,10} | {p.Description}");
    }

    static async Task UpdateProductAsync()
    {
        Console.Write("Informe o Id do produto a atualizar: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido."); return; }

        using var db = new AppDbContext();
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is null) { Console.WriteLine("Produto não encontrado."); return; }

        Console.WriteLine($"Atualizando Id {product.Id}. Deixe em branco para manter.");
        
        Console.WriteLine($"Nome atual : {product.Name}");
        Console.Write("Novo nome  : ");
        var newName = (Console.ReadLine() ?? "").Trim();

        Console.WriteLine($"Descrição atual: {product.Description}");
        Console.Write("Nova descrição : ");
        var newDescription = (Console.ReadLine() ?? "").Trim();

        Console.WriteLine($"Preço atual: {product.Price:C}");
        Console.Write("Novo preço : ");
        var priceInput = (Console.ReadLine() ?? "").Trim();
        decimal newPrice = 0;
        bool updatePrice = !string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out newPrice) && newPrice > 0;

        if (!string.IsNullOrWhiteSpace(newName)) 
        {
            var exists = await db.Products.AnyAsync(p => p.Name == newName && p.Id != id);
            if (exists) { Console.WriteLine("Já existe outro produto com esse nome."); return; }
            product.Name = newName;
        }
        
        if (!string.IsNullOrWhiteSpace(newDescription)) product.Description = newDescription;
        if (updatePrice) product.Price = newPrice;

        await db.SaveChangesAsync();
        Console.WriteLine("Produto atualizado com sucesso.");
    }

    static async Task DeleteProductAsync()
    {
        Console.Write("Informe o Id do produto a remover: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido."); return; }

        using var db = new AppDbContext();
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is null) { Console.WriteLine("Produto não encontrado."); return; }

        Console.WriteLine($"Confirma a remoção do produto '{product.Name}'? (S/N)");
        if ((Console.ReadLine() ?? "").Trim().ToUpper() != "S") 
        {
            Console.WriteLine("Operação cancelada.");
            return;
        }

        db.Products.Remove(product);
        await db.SaveChangesAsync();
        Console.WriteLine("Produto removido com sucesso.");
    }

    static async Task CreateCategoryAsync()
    {
        Console.Write("Nome da categoria: ");
        var name = (Console.ReadLine() ?? "").Trim();

        Console.Write("Descrição (opcional): ");
        var description = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Nome é obrigatório.");
            return;
        }

        using var db = new AppDbContext();
        if (await db.Categories.AnyAsync(c => c.Name == name))
        {
            Console.WriteLine("Categoria já existe.");
            return;
        }

    var category = new Category { Name = name, Description = description };
        db.Categories.Add(category);
        await db.SaveChangesAsync();
        Console.WriteLine($"Categoria criada! Id: {category.Id}");
    }

    static async Task AddProductToCategoryAsync()
    {
        Console.Write("Id do produto: ");
        if (!int.TryParse(Console.ReadLine(), out var pid))
        {
            Console.WriteLine("Id do produto inválido.");
            return;
        }

        Console.Write("Id da categoria: ");
        if (!int.TryParse(Console.ReadLine(), out var cid))
        {
            Console.WriteLine("Id da categoria inválido.");
            return;
        }

        using var db = new AppDbContext();

        var product = await db.Products.FindAsync(pid);
        var category = await db.Categories.FindAsync(cid);
        if (product is null || category is null)
        {
            Console.WriteLine("Produto ou categoria não encontrado");
            return;
        }

        var exists = await db.ProductCategories.AnyAsync(pc => pc.ProductId == pid && pc.CategoryId == cid);
        if (exists)
        {
            Console.WriteLine("Produto já está nesta categoria");
            return;
        }

    db.ProductCategories.Add(new ProductCategory { ProductId = pid, CategoryId = cid });
        await db.SaveChangesAsync();
        Console.WriteLine($"Produto '{product.Name}' adicionado à categoria '{category.Name}'");
    }

    static async Task ListCategoriesWithProductsAsync()
    {
        using var db = new AppDbContext();
        var rows = await (
            from c in db.Categories
            join pc in db.ProductCategories on c.Id equals pc.CategoryId
            join p in db.Products on pc.ProductId equals p.Id
            orderby c.Name, p.Name
            select new { Category = c.Name, Product = p.Name, p.Price, p.Quantity }
        ).ToListAsync();

        if (rows.Count == 0)
        {
            Console.WriteLine("Nenhum produto categorizado encontrado");
            return;
        }

        string current = "";
        foreach (var r in rows)
        {
            if (current != r.Category)
            {
                current = r.Category;
                Console.WriteLine($"\nCategoria: {current}");
                Console.WriteLine("Produtos:");
            }
            Console.WriteLine($"    - {r.Product} (Preço: {r.Price:C}, Estoque: {r.Quantity})");
        }
        Console.WriteLine();
    }

    static async Task ListProductsByCategoryAsync()
    {
        Console.Write("Id da Categoria: ");
        if (!int.TryParse(Console.ReadLine(), out var cid))
        {
            Console.WriteLine("Id inválido.");
            return;
        }

        using var db = new AppDbContext();
        var category = await db.Categories.FindAsync(cid);
        if (category is null)
        {
            Console.WriteLine("Categoria não encontrada.");
            return;
        }

        var products = await (
            from pc in db.ProductCategories
            join p in db.Products on pc.ProductId equals p.Id
            where pc.CategoryId == cid
            orderby p.Name
            select new { p.Name, p.Price, p.Quantity, p.Description }
        ).ToListAsync();

        Console.WriteLine($"\nCategoria: {category.Name}");
        if (products.Count == 0)
        {
            Console.WriteLine("  (sem produtos)");
            return;
        }

        foreach (var p in products)
            Console.WriteLine($"  - {p.Name} | Preço: {p.Price:C} | Estoque: {p.Quantity} | {p.Description}");
    }

    static async Task UpdateStockAsync()
    {
        Console.Write("Id do produto: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Id inválido.");
            return;
        }

        using var db = new AppDbContext();
        var product = await db.Products.FindAsync(id);
        if (product is null)
        {
            Console.WriteLine("Produto não encontrado.");
            return;
        }

        Console.WriteLine($"Produto: {product.Name}");
        Console.WriteLine($"Estoque atual: {product.Quantity}");
        
        Console.Write("Quantidade a adicionar/remover (use número negativo para remover): ");
        if (!int.TryParse(Console.ReadLine(), out var adjustment))
        {
            Console.WriteLine("Quantidade inválida.");
            return;
        }

        var newQuantity = product.Quantity + adjustment;
        if (newQuantity < 0)
        {
            Console.WriteLine("Estoque não pode ficar negativo.");
            return;
        }

        product.Quantity = newQuantity;
        await db.SaveChangesAsync();
        Console.WriteLine($"Estoque atualizado. Nova quantidade: {product.Quantity}");
    }
}
