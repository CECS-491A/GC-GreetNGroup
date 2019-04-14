using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Interface;

namespace ServiceLayer.Services
{
    public class CryptoService: ICryptoService
    {
        private readonly string AppLaunchSecretKey = "2E4747FAEB007F487E9B5894B9E4C53700AB3B3C964F1E40C3ED125FFF26DD83";
        //Environment.GetEnvironmentVariable("AppLaunchSecretKey", EnvironmentVariableTarget.User);
        //"D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE"
        //"2E4747FAEB007F487E9B5894B9E4C53700AB3B3C964F1E40C3ED125FFF26DD83"

        RNGCryptoServiceProvider rng;

        public CryptoService()
        {
            rng = new RNGCryptoServiceProvider();
        }

        public string HashHMAC(string message)
        {
            byte[] key = Encoding.ASCII.GetBytes(AppLaunchSecretKey);
            var encoding = new ASCIIEncoding();
            byte[] convertedMessage = encoding.GetBytes(message);


            var hasher = new HMACSHA256(key);
            byte[] hash = hasher.ComputeHash(convertedMessage);
            string hashString = Convert.ToBase64String(hash);

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
