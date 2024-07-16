using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Cryptography;

public class PasswordEncrypt
{
    private readonly string _encryptApiKey;
    
    public PasswordEncrypt(string apiKey) => _encryptApiKey = apiKey;
    
    public string Encrypt(string password)
    {
        var parsedPassword = $"{password}{_encryptApiKey}";
        
        var bytes = Encoding.UTF8.GetBytes(parsedPassword);
        var hashBytes = SHA512.HashData(bytes);

        return BytesToString(hashBytes);
    }

    private static string BytesToString(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }
        
        return sb.ToString();
    }
}