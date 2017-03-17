#region

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Security.Services
{
    public class CryptionService : ICryptionService
    {
        // This constant is used to determine the keysize of the encryption algorithm.
        private const int Keysize = 256;

        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.

        private static readonly byte[] InitVectorBytes = Encoding.ASCII.GetBytes("9963F271-E7E2-4B596-E48B7DB52971");
        private static readonly byte[] SaltBytes = Encoding.ASCII.GetBytes("1C4545BD549C407EBB5A0F41824FE847");

        public CryptionService(string password)
        {
            Guard.ArgumentIsNotNull(password, "Password");
            Password = password;
        }

        //Default Password
        public string Password { get; set; }

        public virtual string Encrypt(string plainText) => DoCryption(plainText, CryptoStreamMode.Write);

        public virtual string Decrypt(string encryptedText) => DoCryption(encryptedText, CryptoStreamMode.Read);

        protected virtual string DoCryption(string text, CryptoStreamMode mode)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            var pass = new Rfc2898DeriveBytes(Password, SaltBytes);
            var keyBytes = pass.GetBytes(Keysize/8);

            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;

                if (keyBytes.Length <= 16)
                    symmetricKey.BlockSize = 128;
                else if (keyBytes.Length > 16 && keyBytes.Length <= 24)
                    symmetricKey.BlockSize = 192;
                else if (keyBytes.Length > 24)
                    symmetricKey.BlockSize = 256;

                if (mode == CryptoStreamMode.Write)
                {
                    #region Encryption

                    using (var cryptor = symmetricKey.CreateEncryptor(keyBytes, InitVectorBytes))
                    {
                        var textBytes = Encoding.UTF8.GetBytes(text);

                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, cryptor, mode))
                            {
                                cryptoStream.Write(textBytes, 0, textBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }

                    #endregion Encryption
                }

                #region Decryption

                using (var cryptor = symmetricKey.CreateDecryptor(keyBytes, InitVectorBytes))
                {
                    var textBytes = Convert.FromBase64String(text);

                    using (var memoryStream = new MemoryStream(textBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, cryptor, mode))
                        {
                            var plainTextBytes = new byte[textBytes.Length];
                            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }

                #endregion Decryption
            }
        }
    }
}