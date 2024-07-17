namespace Domain.Services.Cryptography;

public interface IPasswordEncrypter
{
    public string Encrypt(string password);
}