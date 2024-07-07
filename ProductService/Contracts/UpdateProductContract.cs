namespace ProductService.Contracts;

public record UpdateProductContract(string Title, string Description, decimal Price, string Category, int StockQuantity);