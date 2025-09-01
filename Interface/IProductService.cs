using Store.Contracts;
using Store.Models;

namespace Store.Interface;

public interface IProductService
{
    // General GET methods for retrieving products
    Task<List<Product>> GetAll();
    Task<Product?> GetProductByIdAsync(long id); // for active products


    //POST 
    Task<Product> CreateAsync(Product product);
    Task<(int numOfLines, int numberofLinesSavedtoDB)> UploadProductCSVFile(IFormFile file, int minimumNumOfLines);

    //Delete 
    Task<bool> Delete(long id);
    /*
        // POST method for creating a new product
        Task<Product> CreateAsync(Product product);

        // PUT and PATCH methods for updating existing products
        Task<Product?> UpdateAsync(Guid id, Product updatedProduct);
        Task<Product?> PatchUpdateAsync(Guid id, UpdateProductDto dto);

        // DELETE methods for removing or deactivating products
        Task<bool> SoftDeleteAsync(Guid id);
        Task<bool> DeleteProductPermanentlyAsync(Guid id);

        // PUT method for reactivating a product
        Task<Product?> ReactivateAsync(Guid id);

    */

}