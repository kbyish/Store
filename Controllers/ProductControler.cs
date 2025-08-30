
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
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<EmployeeController> _logger;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, ILogger<EmployeeController> logger, IMapper mapper)
    {
        _productService = productService;
        _logger = logger;
        _mapper = mapper;
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
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0 )
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
        if (file is null || file.Length == 0)
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