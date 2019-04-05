using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Interface;

namespace ServiceLayer.Services
{
    public class CryptoService: ICryptoService
    {
        RNGCryptoServiceProvider rng;

        public CryptoService()
        {
            rng = new RNGCryptoServiceProvider();
        }

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
            Byte[] b = new byte[64 / 2];
            rng.GetBytes(b);
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

        public SigningCredentials GenerateJWTSignature()
        {
            byte[] symmetricKey = new byte[256 / 8];
            rng.GetBytes(symmetricKey);
            var charKey = Encoding.UTF8.GetString(symmetricKey);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(charKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            return credentials;
        }
    }
}
