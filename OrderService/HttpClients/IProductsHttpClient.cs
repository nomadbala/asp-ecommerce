using ProductService.Models;

namespace OrderService.HttpClients;

public interface IProductsHttpClient
{
    Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids);
}