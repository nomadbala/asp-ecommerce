using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PaymentService.Contracts;

namespace PaymentService.Services;

public class EpayService : IEpayService
{
    private const string ClientId = "test";

    private const string ClientSecret = "yF587AV9Ms94qN2QShFzVR3vFnWkhjbAK3sG";

    private readonly HttpClient _httpClient;

    public EpayService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetPaymentTokenAsync()
    {
        var tokenUrl = "https://testoauth.homebank.kz/epay2/oauth2/token";

        var data = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("scope",
                "webapi usermanagement email_send verification statement statistics payment"),
            new KeyValuePair<string, string>("client_id", ClientId),
            new KeyValuePair<string, string>("client_secret", ClientSecret)
        });

        var response = await _httpClient.PostAsync(tokenUrl, data);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);

        return tokenResponse!.AccessToken;
    }

    public async Task<string> EncryptDataAsync(object data)
    {
        var publicKeyUrl = "https://testepay.homebank.kz/api/public.rsa";
        var response = await _httpClient.GetAsync(publicKeyUrl);
        response.EnsureSuccessStatusCode();

        var publicKeyPem = await response.Content.ReadAsStringAsync();
        var publicKey = DecodePublicKey(publicKeyPem);

        var jsonData = JsonConvert.SerializeObject(data);
        var encryptedData = EncryptWithPublicKey(jsonData, publicKey);

        return Convert.ToBase64String(encryptedData);
    }
    
    public async Task<PaymentResponse> MakePaymentAsync(PaymentRequest request, string token)
    {
        var paymentUrl = "https://testepay.homebank.kz/api/payment/cryptopay";
        var encryptedData = await EncryptDataAsync(request);

        var paymentData = new
        {
            amount = request.Amount,
            currency = request.Currency,
            name = request.CardHolderName,
            cryptogram = encryptedData,
            invoiceId = request.InvoiceId,
            invoiceIdAlt = request.InvoiceIdAlt,
            description = request.Description,
            accountId = request.AccountId,
            email = request.Email,
            phone = request.Phone,
            cardSave = request.CardSave,
            data = request.Data,
            postLink = request.PostLink,
            failurePostLink = request.FailurePostLink
        };

        var content = new StringContent(JsonConvert.SerializeObject(paymentData), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, paymentUrl)
        {
            Content = content
        };
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PostAsync(paymentUrl, content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<PaymentResponse>(responseContent);
    }

    private RSA DecodePublicKey(string publicKeyPem)
    {
        var publicKeyBytes = Convert.FromBase64String(publicKeyPem);
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
        return rsa;
    }

    private byte[] EncryptWithPublicKey(string data, RSA publicKey)
    {
        return publicKey.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);
    }
}