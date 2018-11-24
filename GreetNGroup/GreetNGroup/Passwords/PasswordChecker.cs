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
        private static HttpClient client = new HttpClient();

        public PasswordChecker()
        {

        }

        /// <summary>
        /// Method to get the hash of the password and return the first 5 characters 
        /// </summary>
        /// <param name="pass">The password to be hashed</param>
        /// <returns>First 5 characters of the hash</returns>
        public static string GetFirst5HashChars(string pass)
        {
            UTF8ToSHA1 sha1 = new UTF8ToSHA1();
            string hashedPassword = sha1.ConvertToHash(pass);
            string firstFiveChars = hashedPassword.Substring(0, 5);
            return firstFiveChars;
        }
        /// <summary>
        /// Method to get the last 35 characters of the password hash
        /// </summary>
        /// <param name="pass">The password to be hashed</param>
        /// <returns>Last 35 characters of the hash</returns>
        public static string GetHashSuffix(string pass)
        {
            UTF8ToSHA1 sha1 = new UTF8ToSHA1();
            string hashedPassword = sha1.ConvertToHash(pass);
            string passwordSuffix = hashedPassword.Substring(5);
            return passwordSuffix;
        }

        /// <summary>
        /// Method to get a list of all hashes that match the first 5 characters of the original hash
        /// </summary>
        /// <param name="pass">The password to be hashed</param>
        /// <returns>HTTPContent of the page</returns>
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

            //Content reader holds all the returned hashes
            using (contentReader)
            {
                //While loop reads each hash line by line
                while (!contentReader.EndOfStream)
                {
                    //Variable passwordInfo holds the hash as it's read
                    var passwordInfo = await contentReader.ReadLineAsync();
                    //Split the hash using semicolon to get the hash suffix in the first index and count in the second index
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

        //TODO:
        //Delete above IsPasswordPwnedmethod
        public static async Task<bool> IsPasswordPwned(string pass)
        {
            bool identicalHashExists = false;
            HttpContent passwordHashPrefixes = null;
            string firstFiveChars = GetFirst5HashChars(pass);
            string hashSuffix = GetHashSuffix(pass);

            string path = "https://api.pwnedpasswords.com/range/" + firstFiveChars;
            StreamReader contentReader = new StreamReader(await passwordHashPrefixes.ReadAsStreamAsync());

            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    passwordHashPrefixes = response.Content;
                }
            }
            catch (Exception e)
            {
                //This is where logging should go
            }

            //Content reader holds all the returned hashes
            using (contentReader)
            {
                //While loop reads each hash line by line
                while (!contentReader.EndOfStream)
                {
                    //Variable passwordInfo holds the hash as it's read
                    var passwordInfo = await contentReader.ReadLineAsync();
                    //Split the hash using semicolon to get the hash suffix in the first index and count in the second index
                    var splitToCount = passwordInfo.Split(':');
                    if (splitToCount.Length == 2 && splitToCount[0].Equals(hashSuffix))
                    {
                        identicalHashExists = true;
                        return identicalHashExists;
                    }
                }
            }
            return identicalHashExists;
        }
    }
}
