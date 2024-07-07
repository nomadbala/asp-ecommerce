using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using ProductService.Contracts;
using ProductService.Models;

namespace ProductService.Repositories;

public class ProductsRepository(ProductServiceDatabaseContext context) : IProductsRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync() => await context.Products.ToListAsync();

    public async Task<Product> CreateAsync(CreateProductContract contract)
    {
        var product = new Product(contract);

        await context.AddAsync(product);
        await context.SaveChangesAsync();

        return product;
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        var product = await context.Products.FirstOrDefaultAsync(product => product.Id == id);

        return product ?? throw new ElementNotFoundException($"Product with id {id} not found");
    }

    public async Task<Product> UpdateAsync(Guid id, UpdateProductContract contract)
    {
        var product = await context.Products.FirstOrDefaultAsync(product => product.Id == id);

        if (product == null)
            throw new ElementNotFoundException($"Product with id {id} not found");

        product.Title = contract.Title;
        product.Description = contract.Description;
        product.Price = contract.Price;
        product.Category = contract.Category;
        product.StockQuantity = contract.StockQuantity;

        await context.SaveChangesAsync();

        return product;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        if (!await context.Products.AnyAsync(product => product.Id == id))
            throw new ElementNotFoundException($"Product with id {id} not found");

        await context.Products
            .Where(product => product.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<Product>> GetByTitleAsync(string title)
    {
        return await context.Products
            .Where(product => product.Title == title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        return await context.Products
            .Where(product => product.Category == category)
            .ToListAsync();
    }
}