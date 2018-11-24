using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace GreetNGroup.Passwords
{
    //TODO: add handling of passwords that don't occur at all


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

        public static async Task<bool> IsPasswordPwned(string pass)
        {
            bool identicalHashExists = false;
            HttpContent passwordHashPrefixes = null;

            string firstFiveChars = GetFirst5HashChars(pass);
            string hashSuffix = GetHashSuffix(pass);
            string path = "https://api.pwnedpasswords.com/range/" + firstFiveChars;


            Console.WriteLine("asdf");
            try
            {
                var response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    passwordHashPrefixes = response.Content;

                    Console.WriteLine("Before using Contentreader");
                    //Content reader holds all the returned hashes
                    using (StreamReader contentReader = new StreamReader(await passwordHashPrefixes.ReadAsStreamAsync()))
                    {
                        Console.WriteLine("Using Contentreader");
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
                                Console.WriteLine(identicalHashExists);
                                return identicalHashExists;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                //This is where logging should go
            }


            return identicalHashExists;
        }

        public static async Task<int> passwordOccurences(string passwordToCheck)
        {
            HttpContent passwordHashPrefixes = null;

            string firstFiveChars = GetFirst5HashChars(passwordToCheck);
            string hashSuffix = GetHashSuffix(passwordToCheck);
            string path = "https://api.pwnedpasswords.com/range/" + firstFiveChars;


            Console.WriteLine("asdf");
            try
            {
                var response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    passwordHashPrefixes = response.Content;

                    Console.WriteLine("Before using Contentreader");
                    //Content reader holds all the returned hashes
                    using (StreamReader contentReader = new StreamReader(await passwordHashPrefixes.ReadAsStreamAsync()))
                    {
                        Console.WriteLine("Using Contentreader");
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
                }

            }
            catch (Exception e)
            {
                //This is where logging should go
            }
            return 0;
        }
    }
}
