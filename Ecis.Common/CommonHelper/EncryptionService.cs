using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ecis.Common.CommonHelper
{
    public class EncryptionService
    {
        public string EncryptString(string text, string saltContent)
        {
            byte[] rawData = Encoding.UTF8.GetBytes(text);

            var aes = Aes.Create();
            int nBytes = aes.BlockSize >> 3;
            var generateKeys = new Rfc2898DeriveBytes(saltContent, nBytes);
            aes.Key = generateKeys.GetBytes(nBytes);
            aes.IV = generateKeys.Salt;

            using (var memoryStream = new MemoryStream())
            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                memoryStream.Write(generateKeys.Salt, 0, generateKeys.Salt.Length);
                cryptoStream.Write(rawData, 0, rawData.Length);
                cryptoStream.Close();

                byte[] encrytedData = memoryStream.ToArray();

                return Convert.ToBase64String(encrytedData);
            }
        }

        public string DecryptString(string encryptedText, string saltContent)
        {
            byte[] rawData = Convert.FromBase64String(encryptedText);
            Aes aes = Aes.Create();

            // setup the decryption algorithm
            int nBytes = aes.BlockSize >> 3;
            var salt = new byte[nBytes];
            for (int i = 0; i < salt.Length; i++)
                salt[i] = rawData[i];

            var generateKeys = new Rfc2898DeriveBytes(saltContent, salt);

            aes.Key = generateKeys.GetBytes(aes.BlockSize >> 3);
            aes.IV = salt;

            using (var stream = new MemoryStream())
            using (var decryptor = aes.CreateDecryptor())
            {
                var cryptoStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Write);

                cryptoStream.Write(rawData, nBytes, rawData.Length - nBytes);
                cryptoStream.Close();

                byte[] decryptedData = stream.ToArray();

                return Encoding.UTF8.GetString(decryptedData);
            }
        }
    }
}