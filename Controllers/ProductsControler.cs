
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Contracts;
using Store.Interface;
using Store.Models;


namespace Store.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IMapper _mapper;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger, IMapper mapper)
    {
        _productService = productService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll( int pageNumber = 1, int pageSize = 10)
    {
        var products = await _productService.GetAll(pageNumber, pageSize);
        if (products.Count == 0)
        {
            return NoContent();
        }

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {

        var product = await _productService.GetProductByIdAsync(id);
        if (product is null)
        {
            return NoContent();
        }

        return Ok(product);
    }

    /// <summary>
    /// Create a new product.
    /// </summary>
    /// <param name="dto">Product creation details</param>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Product>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0)
            return BadRequest(new ApiResponse<string>(false, "Invalid product input"));

        var product = _mapper.Map<Product>(dto);
        var createdProduct = await _productService.CreateAsync(product);

        var response = new ApiResponse<Product>(
            true,
            "Product created successfully",
            _mapper.Map<Product>(createdProduct)
        );

        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, response);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> Update(long id, Product product)
    {
        var updated = await _productService.UpdateAsync(id, product);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpPost("UploadCsv")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("No file uploaded or file is empty.");
        }

        if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Only CSV files are allowed.");
        }

        const int minimumNumOfLines = 3; // Minimum number of lines that csv file must have  

        try
        {
            (int NumberOfDataLines, int NumberOfProductSaved) = await _productService.UploadProductCSVFile(file, minimumNumOfLines);

            if (NumberOfDataLines < minimumNumOfLines)
            {
                return BadRequest("CSV File must have at least 10 lines of data.");
            }

            if (NumberOfProductSaved == 0)
            {
                return NoContent();
            }

            StringBuilder message = new($"there are {NumberOfDataLines} of lines in CSV file. ");

            if (NumberOfDataLines == NumberOfProductSaved)
                message.Append(" All Lines Has been Saved Sucessfully.");
            else message.Append($" Only {NumberOfProductSaved} lines has been saved  succesfuly.");

            return Ok(message.ToString());
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}") ]
    public async Task<ActionResult> Delete(long id)
    {
        bool isDeleted = await _productService.Delete(id);
        if (isDeleted)
        {
            return NoContent();
        }
        return NotFound();
    }
}