using ProductService.Contracts;
using ProductService.Models;
using ProductService.Repositories;

namespace ProductService.Services;

public class ProductsService(IProductsRepository repository) : IProductsService
{
    public async Task<IEnumerable<Product>> GetAllAsync() => await repository.GetAllAsync();

    public async Task<Product> CreateAsync(CreateProductContract contract) => await repository.CreateAsync(contract);

    public async Task<Product> GetByIdAsync(Guid id) => await repository.GetByIdAsync(id);

    public async Task<Product> UpdateAsync(Guid id, UpdateProductContract contract) =>
        await repository.UpdateAsync(id, contract);

    public async Task DeleteByIdAsync(Guid id) => await repository.DeleteByIdAsync(id);

    public async Task<IEnumerable<Product>> GetByTitleAsync(string title) => await repository.GetByTitleAsync(title);

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category) =>
        await repository.GetByCategoryAsync(category);
}