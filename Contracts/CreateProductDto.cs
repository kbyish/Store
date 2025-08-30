using System.ComponentModel.DataAnnotations;

namespace Store.Contracts
{
    /// <summary>
    /// DTO for creating a product
    /// </summary>
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(10, ErrorMessage = "Product name cannot exceed 10 characters.")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Using placeholders for dynamic messages
        [Range(0.01, 10.98, ErrorMessage = "Price is must be between  {1} and {2}.")]
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
