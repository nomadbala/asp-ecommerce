using System.ComponentModel.DataAnnotations.Schema;
using ProductService.Contracts;

namespace ProductService.Models;

[Table("products")]
public class Product
{
    public Guid Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }
    
    [Column("price")]
    public decimal Price { get; set; }

    public string Category { get; set; }
    
    [Column("stock_quantity")]
    public int StockQuantity { get; set; }
    
    [Column("added_date")]
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