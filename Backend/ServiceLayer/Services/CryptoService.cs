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
        RNGCryptoServiceProvider rng;
        IGNGLoggerService _gngLoggerService;

        public CryptoService()
        {
            rng = new RNGCryptoServiceProvider();
            _gngLoggerService = new GNGLoggerService();
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

        /// <summary>
        /// Method RetrieveUsersSequentialId finds the users sequential id based on their
        /// hashed id. Since SHA256 is a one-way encryption algorithm as of 2019.4.5, it is currently
        /// not possible to decrypt such a hash and the only way to find the sequential id is to
        /// compare hashes, which will take a hit in terms of runtime.
        /// </summary>
        /// <param name="hashedId">User's hashed id</param>
        /// <returns>Returns integer value which is the user's sequential id</returns>
        public int RetrieveUsersSequentialId(string hashedId)
        {
            int usersId = -1;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var userList = new List<User>();
                    var usersInDB = ctx.Users;

                    foreach (User u in usersInDB)
                    {
                        var uId = u.UserId.ToString();
                        var hashedUId = HashSha256(uId);
                        if (hashedId.Equals(hashedUId))
                        {
                            usersId = u.UserId;
                        }
                    }

                }
                return usersId;
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return usersId;
            }

        }
    }
}
