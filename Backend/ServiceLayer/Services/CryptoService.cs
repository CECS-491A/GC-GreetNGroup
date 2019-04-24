using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Gucci.ServiceLayer.Interface;
using DataAccessLayer.Context;
using DataAccessLayer.Tables;
using System.Collections.Generic;

namespace Gucci.ServiceLayer.Services
{
    public class CryptoService: ICryptoService
    {
        private readonly string AppLaunchSecretKey = "D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE";
        //Environment.GetEnvironmentVariable("AppLaunchSecretKey", EnvironmentVariableTarget.User);
        //"D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE"
        //"49A4BBA24DB9F476AAC1A007A13EC05C930FE0AABF024C2B94085E27C0CD214F"

        RNGCryptoServiceProvider rng;
        ILoggerService _gngLoggerService;

        public CryptoService()
        {
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

        public SigningCredentials GenerateJWTSignature(string symmetricKey)
        {
            return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricKey)),
                SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
