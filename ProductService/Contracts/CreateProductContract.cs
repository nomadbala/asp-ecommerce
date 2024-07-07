namespace ProductService.Contracts;

public record CreateProductContract(string Title, string Description, decimal Price, string Category, int StockQuantity);