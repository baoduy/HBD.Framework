using HBD.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using HBD.Framework.Core;

namespace HBD.Framework.Security.Services
{
    public class Base64CryptionService : ICryptionService
    {
        public string Decrypt([NotNull]string encryptedText)
        {
            Guard.ArgumentIsNotNull(encryptedText,nameof(encryptedText));

            var cipherBytes = Convert.FromBase64String(encryptedText);
            return Encoding.Unicode.GetString(cipherBytes);
        }

        public string Encrypt([NotNull]string plainText)
        {
            Guard.ArgumentIsNotNull(plainText, nameof(plainText));

            var clearBytes = Encoding.Unicode.GetBytes(plainText);
            return Convert.ToBase64String(clearBytes);
        }
    }
}
