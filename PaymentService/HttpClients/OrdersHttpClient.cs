using System.Text.Json;
using OrderService.Models;

namespace PaymentService.HttpClients;

public class OrdersHttpClient : IOrdersHttpClient
{
    private readonly HttpClient _httpClient;

    public OrdersHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/orders/{id}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<Order>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}