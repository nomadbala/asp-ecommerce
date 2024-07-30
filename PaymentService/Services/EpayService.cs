
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PaymentService.Repositories;

public class TokenRequest
{
    [JsonProperty("grant_type")]
    public string GrantType { get; set; }

    [JsonProperty("scope")]
    public string Scope { get; set; }
    
    [JsonProperty("client_id")]
    public string ClientId { get; set; }
    
    [JsonProperty("client_secret")]
    public string ClientSecret { get; set; }
    
    [JsonProperty("invoiceID")]
    public string InvoiceId { get; set; }

    [JsonProperty("amount")]
    public int Amount;

    [JsonProperty("currency")]
    public string Currency { get; set; }
    
    [JsonProperty("terminal")]
    public string Terminal { get; set; }
}

public class TokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonProperty("scope")]
    public string Scope { get; set; }
    
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
}

public class CryptogramResponse
{
    [JsonProperty("hpan")]
    public string Hpan { get; set; }
    
    [JsonProperty("expDate")]
    public string ExpDate { get; set; }
    
    [JsonProperty("cvc")]
    public string Cvc { get; set; }
    
    [JsonProperty("terminalId")]
    public string TerminalId { get; set; }
}

public class PaymentResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("amount")]
    public int Amount { get; set; }
    
    [JsonProperty("amountBonus")]
    public int AmountBonus { get; set; }
    
    [JsonProperty("currency")]
    public string Currency { get; set; }
    
    [JsonProperty("invoiceID")]
    public string InvoiceId { get; set; }
    
    [JsonProperty("invoiceIdAlt")]
    public string InvoiceIdAlt { get; set; }
    
    [JsonProperty("accountID")]
    public string AccountId { get; set; }
    
    [JsonProperty("phone")]
    public string Phone { get; set; }
    
    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("reference")]
    public string Reference { get; set; }
    
    [JsonProperty("intReference")]
    public string IntReference { get; set; }
    
    [JsonProperty("secure3D")]
    public bool Secure3D { get; set; }
    
    [JsonProperty("cardID")]
    public string CardId { get; set; }
    
    [JsonProperty("language")]
    public string Language { get; set; }
    
    [JsonProperty("fee")]
    public int Fee { get; set; }
    
    [JsonProperty("code")]
    public int Code { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
}

public class EpayService : IEpayService
{
    private readonly HttpClient _httpClient;

    public EpayService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TokenResponse> GetTokenAsync()
    {
        var url = "https://testoauth.homebank.kz/epay2/oauth2/token";
        
        var requestData = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "scope", "webapi usermanagement email_send verification statement statistics payment" },
            { "client_id", "test" },
            { "client_secret", "yF587AV9Ms94qN2QShFzVR3vFnWkhjbAK3sG" },
            { "invoice_id", "1" },
            { "amount", "100" },
            { "currency", "KZT" },
            { "terminal", "67e34d63-102f-4bd1-898e-370781d0074d" }
        };

        var request = new FormUrlEncodedContent(requestData);

        var response = await _httpClient.PostAsync(url, request);

        response.EnsureSuccessStatusCode();

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<TokenResponse>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new InvalidOperationException();   
        }
        else
        {
            throw new Exception($"Ошибка при запросе токена: {response.ReasonPhrase}");
        }
    }

    public async Task<RSA> GetPublicKeyAsync()
    {
        string publicKeyUrl = "https://testepay.homebank.kz/api/public.rsa";
        HttpResponseMessage response;

        try
        {
            response = await _httpClient.GetAsync(publicKeyUrl);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            throw new Exception("Error fetching public key", e);
        }

        byte[] body;

        try
        {
            body = await response.Content.ReadAsByteArrayAsync();
        }
        catch (IOException e)
        {
            throw new Exception("Error reading response body", e);
        }

        X509Certificate2 certificate;

        try
        {
            certificate = new X509Certificate2(body);
        }
        catch (CryptographicException e)
        {
            throw new Exception("Error parsing X509 certificate", e);
        }

        RSA rsaPublicKey = certificate.GetRSAPublicKey();

        if (rsaPublicKey == null)
        {
            throw new Exception("Failed to parse RSA public key");
        }

        return rsaPublicKey;
    }

    public async Task<string> EncryptDataAsync()
    {
        var key = await GetPublicKeyAsync();

        if (key == null)
            throw new Exception($"NULL RSA KEY");

        var data = new Dictionary<string, string>
        {
            { "hpan", "4405639704015096" },
            { "expDate", "0125" },
            { "cvc", "815" },
            { "terminalId", "67e34d63-102f-4bd1-898e-370781d0074d" }
        };

        var jsonData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));;

        var encryptedData = key.Encrypt(jsonData, RSAEncryptionPadding.Pkcs1);

        return Convert.ToBase64String(encryptedData);
    }

    public async Task<PaymentResponse> MakePaymentAsync()
    {
        var url = "POST URL https://testepay.homebank.kz/api/payment/cryptopay";

        var token = await GetTokenAsync();

        var encryptedData = await EncryptDataAsync();
        
        var requestData = new
        {
            amount = 100,
            currency = "KZT",
            name = "JON JONSON",
            cryptogram = encryptedData,
            invoiceId = "000000001",
            invoiceIdAlt = "8564546",
            description = "test payment",
            accountId = "uuid000001",
            email = "jj@example.com",
            phone = "77777777777",
            cardSave = true,
            data = "{\"statement\":{\"name\":\"Arman Ali\",\"invoiceID\":\"80000016\"}}",
            postLink = "https://testmerchant/order/1123",
            failurePostLink = "https://testmerchant/order/1123/fail"
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Headers.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
        request.Content = content;

        var response = await _httpClient.SendAsync(request);

        var responseBody = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}, Response: {responseBody}");
            throw new Exception($"Error: {response.StatusCode}, Response: {responseBody}");
        }

        return JsonConvert.DeserializeObject<PaymentResponse>(responseBody);


    }
}