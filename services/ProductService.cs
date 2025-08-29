
using Store.Interface;
using Store.Models;

namespace Store.Services;

public class ProductService : IProductService
{
    IEnumerable<Product> products = [
        new (){Id = 1, Name = "product 1",  Description = "product Description 1 ", Price = 1.2m, Category = "Category 1" },
        new (){Id = 2, Name = "product 2",  Description = "product Description 2 ", Price = 2.2m, Category = "Category 2" },
        new (){Id = 3, Name = "product 3",  Description = "product Description 3 ", Price = 3.2m, Category = "Category 3" }
    ];
    public async Task<Product?> GetProductByIdAsync(long id)
    {
        await Task.Delay(2000); // Waits for 2 seconds without blocking the thread
        return products.FirstOrDefault(p => p.Id == id);

    }

    public async Task<List<Product>> UploadProductCSVFile(IFormFile file)
    {
        List<Product> products = [];
        // Read the CSV content from the uploaded file
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
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
    }


    public async Task Delete(long id)
    {
        await Task.Delay(2000); // Waits for 2 seconds without blocking the thread
        
    }
}