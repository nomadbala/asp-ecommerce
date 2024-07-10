using System.Text.Json;
using UserService.Models;

namespace OrderService.HttpClients;

public class UsersHttpClient : IUsersHttpClient
{
    private readonly HttpClient _httpClient;

    public UsersHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/users/{id}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}