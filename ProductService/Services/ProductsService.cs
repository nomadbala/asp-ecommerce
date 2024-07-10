using ProductService.Contracts;
using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Services;

public class ProductsService : IProductsService
{
    private readonly IProductsRepository _repository;

    public ProductsService(IProductsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Product> CreateAsync(CreateProductContract contract)
    {
        return await _repository.CreateAsync(contract);
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadHttpRequestException($"Invalid ID. {nameof(id)}");
        
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Product> UpdateAsync(Guid id, UpdateProductContract contract)
    {
        if (id == Guid.Empty)
            throw new BadHttpRequestException($"Invalid ID. {nameof(id)}");
        
        return await _repository.UpdateAsync(id, contract);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadHttpRequestException($"Invalid ID. {nameof(id)}");
        
        await _repository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetByTitleAsync(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new BadHttpRequestException($"Invalid title. {nameof(title)}");
        
        return await _repository.GetByTitleAsync(title);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        if (string.IsNullOrEmpty(category))
            throw new BadHttpRequestException($"Invalid category. {nameof(category)}");
        
        return await _repository.GetByCategoryAsync(category);
    }
}