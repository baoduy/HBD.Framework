namespace HBD.Framework.Security.Services
{
    public interface ICryptionService
    {
        string Password { get; set; }

        string Decrypt(string encryptedText);

        string Encrypt(string plainText);
    }
}