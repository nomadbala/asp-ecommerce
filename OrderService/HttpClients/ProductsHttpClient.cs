using System.Text.Json;
using ProductService.Models;

namespace OrderService.HttpClients;

public class ProductsHttpClient : IProductsHttpClient
{
    private readonly HttpClient _httpClient;

    public ProductsHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        var response = await _httpClient.GetAsync("api/products");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var allProducts = JsonSerializer.Deserialize<IEnumerable<Product>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    
        return allProducts.Where(p => ids.Contains(p.Id));
    }
}