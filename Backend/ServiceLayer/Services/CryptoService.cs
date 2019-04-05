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
            string hashString = Encoding.UTF8.GetString(hash, 0, hash.Length);

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

        public string HashSha256(string message)
        {
            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                //First converts the uID into UTF8 byte encoding before hashing
                var hashedUIDBytes =
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(message));
                var hashToString = new StringBuilder(hashedUIDBytes.Length * 2);
                foreach (byte b in hashedUIDBytes)
                {
                    hashToString.Append(b.ToString("X2"));
                }

                sha256.Dispose();
                return hashToString.ToString();
            }
        }
    }
}
