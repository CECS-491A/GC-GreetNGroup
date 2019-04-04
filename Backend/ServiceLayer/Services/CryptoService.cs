using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class CryptoService: ICryptoService
    {
        public string HashHMAC(byte[] key, string message)
        {
            var encoding = new ASCIIEncoding();
            byte[] convertedMessage = encoding.GetBytes(message);


            var hasher = new HMACSHA256(key);
            byte[] hash = hasher.ComputeHash(convertedMessage);
            string hashString = System.Text.Encoding.UTF8.GetString(hash, 0, hash.Length);

            return hashString;
        }

        public string GenerateToken()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            Byte[] b = new byte[64 / 2];
            provider.GetBytes(b);
            string hex = BitConverter.ToString(b).Replace("-", "");
            return hex;
        }
    }
}
