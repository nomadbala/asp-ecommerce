using ProductService.Contracts;

namespace ProductService.Models;

public class Product
{
    public Product()
    {
    }

    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
    
    public decimal Price { get; set; }

    public string Category { get; set; }
    
    public int StockQuantity { get; set; }
    
    public DateTime AddedDate { get; set; }

    public Product(CreateProductContract contract)
    {
        Id = Guid.NewGuid();
        Title = contract.Title;
        Description = contract.Description;
        Price = contract.Price;
        Category = contract.Category;
        StockQuantity = contract.StockQuantity;
        AddedDate = DateTime.UtcNow;
    }
}