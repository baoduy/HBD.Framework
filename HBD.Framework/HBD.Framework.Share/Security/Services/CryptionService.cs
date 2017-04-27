#region using

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
        public CryptionService(string password)
        {
            Guard.ArgumentIsNotNull(password, "Password");
            Password = password;
            Salt =
                Encoding.ASCII.GetBytes("{2964a705-3452-4625-8f05-530576545efc}-{cc2b9516-0eac-4bfb-bc64-dc2227030e4f}");
        }

        private byte[] Salt { get; }

        //Default Password
        public string Password { get; set; }

        public virtual string Encrypt(string plainText) => DoCryption(plainText, false);

        public virtual string Decrypt(string encryptedText) => DoCryption(encryptedText, true);

        protected virtual string DoCryption(string text, bool isDecryption)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            var keyBytes = new Rfc2898DeriveBytes(Password, Salt);

            using (var symmetricKey = Aes.Create())
            {
                if (symmetricKey == null)
                    throw new InvalidOperationException("Cannot create Aes object by 'Aes.Create()'");

                symmetricKey.Key = keyBytes.GetBytes(32);
                symmetricKey.IV = keyBytes.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    if (!isDecryption)
                    {
                        #region Encryption

                        var clearBytes = Encoding.Unicode.GetBytes(text);

                        using (var cs = new CryptoStream(ms, symmetricKey.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                        }
                        return Convert.ToBase64String(ms.ToArray());

                        #endregion Encryption
                    }

                    #region Decryption

                    var cipherBytes = Convert.FromBase64String(text);

                    using (var cs = new CryptoStream(ms, symmetricKey.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }
                    return Encoding.Unicode.GetString(ms.ToArray());

                    #endregion Decryption
                }
            }
        }
    }
}