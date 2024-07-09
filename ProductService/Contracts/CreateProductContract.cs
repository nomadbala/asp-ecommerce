using System.ComponentModel.DataAnnotations;

namespace ProductService.Contracts;

public record CreateProductContract(
    [Required] string Title,
    [Required] string Description,
    [Required] decimal Price,
    [Required] string Category,
    [Required] int StockQuantity
);