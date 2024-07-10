using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using ProductService.Contracts;
using ProductService.Models;

namespace ProductService.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly ProductServiceDatabaseContext _context;

    public ProductsRepository(ProductServiceDatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> CreateAsync(CreateProductContract contract)
    {
        var product = new Product(contract);

        await _context.AddAsync(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        
        return product ?? throw new ElementNotFoundException($"Product with id {id} not found");
    }

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _context.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }

    public async Task<Product> UpdateAsync(Guid id, UpdateProductContract contract)
    {
        var product = await GetByIdAsync(id);

        product.Title = contract.Title;
        product.Description = contract.Description;
        product.Price = contract.Price;
        product.Category = contract.Category;
        product.StockQuantity = contract.StockQuantity;

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var product = await GetByIdAsync(id);

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetByTitleAsync(string title)
    {
        return await _context.Products
            .Where(product => product.Title == title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        return await _context.Products
            .Where(product => product.Category == category)
            .ToListAsync();
    }
}