using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace GreetNGroup.Passwords
{
    
    /// <summary>
    /// Holds the methods that send the SHA1 to the HaveIBeenPwned API to check if
    /// the password has been "pwned"
    /// </summary>
    public class PasswordChecker
    {
        private static UTF8ToSHA1 sha1 = new UTF8ToSHA1();
        private static HttpClient client = new HttpClient();

        public PasswordChecker()
        {

        }

        public static string GetFirst5HashChars(string pass)
        {
            string hashedPassword = sha1.ConvertToHash(pass);
            string firstFiveChars = hashedPassword.Substring(0, 5);
            return firstFiveChars;
        }

        public static string GetHashSuffix(string pass)
        {
            string hashedPassword = sha1.ConvertToHash(pass);
            string passwordSuffix = hashedPassword.Substring(5);
            return passwordSuffix;
        }

        public static async Task<HttpContent> GetIdenticalHashes(string pass)
        {
            HttpContent passwordHashPrefixes = null;
            string firstFiveChars = GetFirst5HashChars(pass);
            string path = "https://api.pwnedpasswords.com/range/" + firstFiveChars;

            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    passwordHashPrefixes = response.Content;
                }
                return passwordHashPrefixes;
            }
            catch (Exception e)
            {
                //This is where logging should go
            }
            return passwordHashPrefixes;
        }


        public static async Task<int> FindMatchingCount(HttpContent passwordHashes, string pass)
        {
            StreamReader contentReader = new StreamReader(await passwordHashes.ReadAsStreamAsync());
            string hashSuffix = GetHashSuffix(pass);

            using (contentReader)
            {
                while (!contentReader.EndOfStream)
                {
                    var passwordInfo = await contentReader.ReadLineAsync();
                    var splitToCount = passwordInfo.Split(':');
                    if (splitToCount.Length == 2 && splitToCount[0].Equals(hashSuffix))
                    {
                        int.TryParse(splitToCount[1], out int count);
                        return count;
                    }
                }
            }
            return 0;
        }

        public static async Task<bool> IsPasswordPwned(string pass)
        {
            bool isPwned = false;

            HttpContent hashedPasswords = await GetIdenticalHashes(pass);
            int count = await FindMatchingCount(hashedPasswords, pass);
            if (count > 0)
            {
                isPwned = true;
            }
            else
            {
                isPwned = false;
            }
            return isPwned;
        }
    }
}
