using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HBD.Framework.Security
{
    public class Cryptography
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] InitVectorBytes = Encoding.ASCII.GetBytes(DefaultPassword + DefaultPassword);

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int Keysize = 256;

        //Default Password
        private const string DefaultPassword = "StevenHoang24121985";
        public static string Encrypt(string plainText, string password = null)
        {
            if (string.IsNullOrEmpty(password))
                password = DefaultPassword;

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var pass = new PasswordDeriveBytes(password, null);
            byte[] keyBytes = pass.GetBytes(Keysize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, InitVectorBytes))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            byte[] cipherTextBytes = memoryStream.ToArray();
                            return Convert.ToBase64String(cipherTextBytes);
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string password = null)
        {
            if (string.IsNullOrEmpty(password))
                password = DefaultPassword;

            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            var pass = new PasswordDeriveBytes(password, null);
            byte[] keyBytes = pass.GetBytes(Keysize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, InitVectorBytes))
                {
                    using (var memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
        }
    }
}
