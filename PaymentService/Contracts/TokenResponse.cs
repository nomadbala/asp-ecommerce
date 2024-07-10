using Newtonsoft.Json;

namespace PaymentService.Contracts;

public class TokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public string ExpiresIn { get; set; }
    public string Scope { get; set; }
}