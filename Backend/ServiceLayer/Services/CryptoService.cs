using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Interface;
using DataAccessLayer.Context;
using DataAccessLayer.Tables;
using System.Collections.Generic;

namespace ServiceLayer.Services
{
    public class CryptoService: ICryptoService
    {
        private readonly string AppLaunchSecretKey = Environment.GetEnvironmentVariable("AppLaunchSecretKey", EnvironmentVariableTarget.User);

        RNGCryptoServiceProvider rng;
        IGNGLoggerService _gngLoggerService;

        public CryptoService()
        {
            rng = new RNGCryptoServiceProvider();
            _gngLoggerService = new GNGLoggerService();
        }

        public string HashHMAC(string message)
        {
            byte[] key = Encoding.ASCII.GetBytes(AppLaunchSecretKey);
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
