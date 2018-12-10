// this is the implementation provided by the
// cryptographic service provider

using System;
using System.Security.Cryptography;
namespace GreetNGroup.Tokens
{
    public static class KeyGen
    {
        /// <summary>
        /// This function is used to create a random key of size keyLength
        /// the key will be used to has the given token
        /// </summary>
        /// <param name="keyLength"></param>
        /// <returns></returns>
        public static string GenerateRandomCryptoKey(int keyLength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}