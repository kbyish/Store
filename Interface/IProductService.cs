using Store.Contracts;
using Store.Models;

namespace Store.Interface;

public interface IProductService
{
    // General GET methods for retrieving products
    Task<List<Product>> GetAll(int pageNumber, int pageSize);
    Task<Product?> GetProductByIdAsync(long id); // for active products

    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(long id, Product product);

    Task<(int numOfLines, int numberofLinesSavedtoDB)> UploadProductCSVFile(IFormFile file, int minimumNumOfLines);

    //Delete 
    Task<bool> Delete(long id);

}