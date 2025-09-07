
using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Store.AppDataContext;
using Store.Contracts;
using Store.Interface;
using Store.Models;

namespace Store.Services;

public class ProductService : IProductService
{

    private readonly AppDbContext _dbContext;

    public ProductService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    IEnumerable<Product> products = [
        new (){Id = 1, Name = "product 1",  Description = "product Description 1 ", Price = 1.2m, Category = "Category 1" },
        new (){Id = 2, Name = "product 2",  Description = "product Description 2 ", Price = 2.2m, Category = "Category 2" },
        new (){Id = 3, Name = "product 3",  Description = "product Description 3 ", Price = 3.2m, Category = "Category 3" }
    ];

    public async Task<Product?> GetProductByIdAsync(long id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }


    public async Task<List<Product>> GetAll(int pageNumber, int pageSize)
    {
        return await _dbContext.Products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    }

    //Create Product
    public async Task<Product> CreateAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(long id, Product product)
    {
        var existing = await _dbContext.Products.FindAsync(id);
        if (existing == null) return null;

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Category = product.Category;

        await _dbContext.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(long id)
    {
        // await Task.Delay(2000); // Waits for 2 seconds without blocking the thread

        var product = await _dbContext.Products.FindAsync(id);

        if (product?.Id == id)
        {
            _dbContext.Products.Remove(product); // Remove from database
            await _dbContext.SaveChangesAsync();
            return true; // Deletion successful
        }

        return false;
    }


    public async Task<(int, int)> UploadProductCSVFile(IFormFile file, int minimumNumOfLines)
    {
        List<string> lines1 = [];
        List<string> lines2 = [];
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            // Skip header if present
            await reader.ReadLineAsync();

            int counter = 0;
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                    if (counter++ % 2 == 0) lines1.Add(line);
                    else lines2.Add(line);
            }
        }

        int NumberOfDataLines = lines1.Count + lines2.Count;

        if (NumberOfDataLines < minimumNumOfLines)
        {
            return (NumberOfDataLines, 0);
        }

        var tasks = new[]
        {
            Task.Run(() => ProcessLines(lines1)),
            Task.Run(() => ProcessLines(lines2))
        };

        int[] NumberOfProductSaved = await Task.WhenAll(tasks);
        return (NumberOfDataLines, NumberOfProductSaved[0] + NumberOfProductSaved[1]);
    }

    private int ProcessLines(List<string> lines)
    {
        int NumberOfProductSaved = 0;
        foreach (var line in lines)
        {
            Thread.Sleep(3500); // 3.5s delay
            var cols = line.Split(',');

            // Validate
            if (cols.Length < 4) continue;
            var name = cols[0].Trim();
            var description = cols[1].Trim();
            if (!decimal.TryParse(cols[2].Trim(), out var price)) continue;
            var category = cols[3].Trim();

            if (string.IsNullOrEmpty(name) || price < 0 || string.IsNullOrEmpty(category))
                continue;


            NumberOfProductSaved++;
            // Store in DB (use locking for thread safety in InMemoryDb)
            lock (_dbContext)
            {
                _dbContext.Products.Add(new Product
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    Category = category
                });
                _dbContext.SaveChanges();
            }

        }

        return NumberOfProductSaved;
    }
}