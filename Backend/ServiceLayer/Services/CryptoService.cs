using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Gucci.ServiceLayer.Interface;

namespace Gucci.ServiceLayer.Services
{
    public class CryptoService: ICryptoService
    {
        private readonly string AppLaunchSecretKey;
        
        //"D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE" For testing
        //

        RNGCryptoServiceProvider rng;
        ILoggerService _gngLoggerService;

        public CryptoService(string SSOSecretKey)
        {
            AppLaunchSecretKey = SSOSecretKey;
            rng = new RNGCryptoServiceProvider();
            _gngLoggerService = new LoggerService();
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
    }
}
