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
        private readonly string AppLaunchSecretKey = "2E4747FAEB007F487E9B5894B9E4C53700AB3B3C964F1E40C3ED125FFF26DD83";
        //Environment.GetEnvironmentVariable("AppLaunchSecretKey", EnvironmentVariableTarget.User);
        //"D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE"
        //"2E4747FAEB007F487E9B5894B9E4C53700AB3B3C964F1E40C3ED125FFF26DD83"

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
