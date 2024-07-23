using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PaymentService.Services;

public class EpayService : IEpayService
{
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _jsonOptions;

    public EpayService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _jsonOptions = new JsonSerializerOptions();
    }

    public async Task<string> TokenAsync()
    {
        var tokenUrl = "https://testoauth.homebank.kz/epay2/oauth2/token";

        var data = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "scope", "webapi usermanagement email_send verification statement statistics payment" },
            { "client_id", ClientID },
            { "client_secret", ClientSecret }
        };

        var requestContent = new FormUrlEncodedContent(data);

        Console.WriteLine($"Requesting token from: {tokenUrl}");
        var response = await _client.PostAsync(tokenUrl, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Token response: {responseContent}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get token, status code: {response.StatusCode}, response: {responseContent}");
        }

        try
        {
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, _jsonOptions);
            if (tokenResponse == null)
            {
                throw new Exception("Deserialized token response is null");
            }
            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                throw new Exception($"Access token is null or empty: 1. {tokenResponse} 2. {response} 3. {responseContent}");
            }

            Console.WriteLine($"Deserialized token response: {JsonSerializer.Serialize(tokenResponse, _jsonOptions)}");
            Console.WriteLine($"Access token: {tokenResponse.AccessToken.Substring(0, Math.Min(10, tokenResponse.AccessToken.Length))}... (truncated)");

            return tokenResponse.AccessToken;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JsonException during deserialization: {ex}");
            Console.WriteLine($"Response content: {responseContent}");
            throw new Exception("Failed to deserialize token response", ex);
        }
    }

    public async Task<PaymentResponse> PayAsync()
    {
        try
        {
            var token = await TokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Failed to obtain a valid token");
            }

            var payUrl = "https://testepay.homebank.kz/api/payment/cryptopay";

            var encryptedData = await EncryptDataAsync(DefaultPaymentData);
            if (string.IsNullOrEmpty(encryptedData))
            {
                throw new Exception("Failed to encrypt payment data");
            }

            var requestData = new Dictionary<string, string>
            {
                { "amount", "100" },
                { "currency", "KZT" },
                { "name", "JON JONSON" },
                { "cryptogram", encryptedData },
                { "invoiceId", "000001" },
                { "invoiceIdAlt", "8564546" },
                { "description", "test payment" },
                { "accountId", "uuid000001" },
                { "email", "jj@example.com" },
                { "phone", "77777777777" },
                { "cardSave", "true" },
                { "data", "{\"statement\":{\"name\":\"Arman Ali\",\"invoiceID\":\"80000016\"}}" },
                { "postLink", "https://testmerchant/order/1123" },
                { "failurePostLink", "https://testmerchant/order/1123/fail" }
            };

            var formContent = new FormUrlEncodedContent(requestData);

            var request = new HttpRequestMessage(HttpMethod.Post, payUrl)
            {
                Content = formContent
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Console.WriteLine($"Sending payment request to: {payUrl}");
            Console.WriteLine($"Request headers: {request.Headers}");
            Console.WriteLine($"Request content: {await formContent.ReadAsStringAsync()}");

            var response = await _client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Payment response: {responseContent}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to pay, status code: {response.StatusCode}, response: {responseContent}");
            }

            var paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(responseContent);
            if (paymentResponse == null)
            {
                throw new Exception("Failed to deserialize payment response");
            }

            return paymentResponse;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in PayAsync: {ex}");
            throw;
        }
    }

    private async Task<string> EncryptDataAsync(string data)
    {
        var url = "https://testepay.homebank.kz/api/public.rsa";

        Console.WriteLine($"Requesting public key from: {url}");
        var response = await _client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to get public key, status code: " + response.StatusCode);
        }

        var body = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(body))
        {
            throw new Exception("Received empty public key");
        }

        var pem = body.Trim();
        Console.WriteLine($"Received public key: {pem.Substring(0, Math.Min(50, pem.Length))}... (truncated)");

        using var rsa = RSA.Create();
        try
        {
            rsa.ImportFromPem(pem.ToCharArray());
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to import public key", ex);
        }

        var encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);
        var base64EncryptedData = Convert.ToBase64String(encryptedData);
        Console.WriteLine($"Encrypted data: {base64EncryptedData.Substring(0, Math.Min(50, base64EncryptedData.Length))}... (truncated)");

        return base64EncryptedData;
    }

    private const string ClientID = "test";
    private const string ClientSecret = "yF587AV9Ms94qN2QShFzVR3vFnWkhjbAK3sG";
    private const string DefaultPaymentData = @"{
            ""hpan"":""4405639704015096"",
            ""expDate"":""0125"",
            ""cvc"":""815"",
            ""terminalId"":""67e34d63-102f-4bd1-898e-370781d0074d""
        }";
}