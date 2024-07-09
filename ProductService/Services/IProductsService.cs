﻿using ProductService.Contracts;
using ProductService.Models;

namespace ProductService.Services;

public interface IProductsService
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product> CreateAsync(CreateProductContract contract);

    Task<Product> GetByIdAsync(Guid id);

    Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids);

    Task<Product> UpdateAsync(Guid id, UpdateProductContract contract);

    Task DeleteByIdAsync(Guid id);

    Task<IEnumerable<Product>> GetByTitleAsync(string title);

    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
}