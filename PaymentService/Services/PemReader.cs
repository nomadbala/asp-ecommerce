namespace PaymentService.Services;

using System;
using System.IO;
using System.Security.Cryptography;

public class PemReader
{
    private readonly string _pem;

    public PemReader(string pem)
    {
        _pem = pem;
    }

    public RSAParameters ReadRsaParameters()
    {
        var pem = _pem.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Trim();
        var publicKeyBytes = Convert.FromBase64String(pem);
        var rsa = new RSACryptoServiceProvider();
        rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
        return rsa.ExportParameters(false);
    }
}
