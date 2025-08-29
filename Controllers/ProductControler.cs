
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Interface;
using Store.Models;


namespace Store.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
        private readonly ILogger<EmployeeController> _logger;

    public ProductController(IProductService productService, ILogger<EmployeeController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {

        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NoContent();
        }

        return Ok(product);
    }


    [HttpPut]
    public IActionResult Put()
    {

        Product product = new()
        {
            Name = "product 1",
            Description = "product Description 1 ",
            Price = 1.2m,
            Category = "Category 1"

        };
        return Ok(product);
    }

    [HttpPost("UploadCsv")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded or file is empty.");
        }

        if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Only CSV files are allowed.");
        }



        try
        {
            List<Product> products = await _productService.UploadProductCSVFile(file);

            return Ok(products);

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

[HttpDelete]
    public async Task Delete(long id)
    {
        await _productService.Delete(id);
    }
}