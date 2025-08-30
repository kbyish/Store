
using System.Text;
using Microsoft.EntityFrameworkCore;
using Store.AppDataContext;
using Store.Interface;
using Store.Models;

namespace Store.Services;

public class ProductService : IProductService
{

    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    IEnumerable<Product> products = [
        new (){Id = 1, Name = "product 1",  Description = "product Description 1 ", Price = 1.2m, Category = "Category 1" },
        new (){Id = 2, Name = "product 2",  Description = "product Description 2 ", Price = 2.2m, Category = "Category 2" },
        new (){Id = 3, Name = "product 3",  Description = "product Description 3 ", Price = 3.2m, Category = "Category 3" }
    ];
    public async Task<Product?> GetProductByIdAsync(long id)
    {
        // await Task.Delay(2000); // Waits for 2 seconds without blocking the thread
        //return products.FirstOrDefault(p => p.Id == id);
        var x = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        return x;

    }

    //Create Product
    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<List<Product>> UploadProductCSVFile(IFormFile file)
    {
        List<Product> products = [];
        // Read the CSV content from the uploaded file
        using var reader = new StreamReader(file.OpenReadStream());
        string csvContent = await reader.ReadToEndAsync();
        string[] lines = csvContent.Split("\r\n");



        for (int i = 1; i < lines.Length; i++)
        {
            string[] records = lines[i].Split(',');

            decimal.TryParse(records[2], out decimal priceDecimalValue);

            products.Add(new Product { Name = records[0], Description = records[1], Price = priceDecimalValue, Category = records[3] });
        }


        // Example: Just return the content for demonstration
        return products;
    }



    public async Task<bool> Delete(long id)
    {
        await Task.Delay(2000); // Waits for 2 seconds without blocking the thread

        var product = await _context.Products.FindAsync(id);

        if (product?.Id == id)
        {
            _context.Products.Remove(product); // Remove from database
            await _context.SaveChangesAsync();
            return true; // Deletion successful
        }

        return false;

    }


    private async void ReadFile(IFormFile file)
    {

        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            // Skip header if present
            await reader.ReadLineAsync();

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                // Split the line by comma to get individual fields
                var fields = line.Split(',');

                // Perform validation and parsing
                if (fields.Length != 4)
                {
                    // Handle invalid line format
                    continue;
                }

                // Extract and parse fields
                string? name = fields[0]?.Trim();
                string? description = fields[1]?.Trim();
                decimal price;
                if (!decimal.TryParse(fields[2]?.Trim(), out price))
                {
                    // Handle invalid price format
                    continue;
                }
                string? category = fields[3]?.Trim();

                // Perform additional validation on each field (e.g., length, content)
                if (string.IsNullOrWhiteSpace(name) || price < 0 || string.IsNullOrWhiteSpace(category))
                {
                    // Handle invalid data
                    continue;
                }

                // Create an object to represent the data
                // var product = new Product
                // {
                //     Name = name,
                //     Description = description,
                //     Price = price,
                //     Category = category
                // };

                // Save to database
                // _dbContext.Products.Add(product);
                // await _dbContext.SaveChangesAsync();
            }
        }

    }


    private async Task<List<Product>> ReadFileNoString(IFormFile file)
    {

        List<Product> products = new List<Product>();
        using (var reader = new StreamReader(file.OpenReadStream()))
        {


            // Skip header if present
            await reader.ReadLineAsync();

            StringBuilder line = new("");
            while ((line.Append(await reader.ReadLineAsync())) != null)
            {
                // Split the line by comma to get individual fields
                var fields = line.ToString().Split(',');

                // Perform validation and parsing
                if (fields.Length != 4)
                {
                    // Handle invalid line format
                    continue;
                }

                // Extract and parse fields
                string? name = fields[0]?.Trim();
                string description = fields[1].Trim();
                decimal price;
                if (!decimal.TryParse(fields[2]?.Trim(), out price))
                {
                    // Handle invalid price format
                    continue;
                }
                string? category = fields[3]?.Trim();

                // Perform additional validation on each field (e.g., length, content)
                if (string.IsNullOrWhiteSpace(name) || price < 0 || string.IsNullOrWhiteSpace(category))
                {
                    // Handle invalid data
                    continue;
                }

                products.Add(new Product
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    Category = category
                });

                // Save to database
                // _dbContext.Products.Add(product);
                // await _dbContext.SaveChangesAsync();


            }
        }
        return products;
    }
}