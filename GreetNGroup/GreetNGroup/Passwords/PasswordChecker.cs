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
        //HttpClient connects to Troy Hunt's HaveIBeenPwned API to retrieve possibly pwned passwords
        private static HttpClient client = new HttpClient();

        /// <summary>
        /// Default constructor for PasswordChecker
        /// </summary>
        public PasswordChecker()
        {

        }

        /// <summary>
        /// Method to get the hash of the password and return the first 5 characters 
        /// </summary>
        /// <param name="password">The password to be hashed</param>
        /// <returns>First 5 characters of the hash</returns>
        public static string GetFirst5HashChars(string password)
        {
            UTF8ToSHA1 sha1 = new UTF8ToSHA1();
            string hashedPassword = sha1.ConvertToHash(password);
            string firstFiveChars = hashedPassword.Substring(0, 5);
            return firstFiveChars;
        }
        /// <summary>
        /// Method to get the last 35 characters of the password hash
        /// </summary>
        /// <param name="password">The password to be hashed</param>
        /// <returns>Last 35 characters of the hash</returns>
        public static string GetHashSuffix(string password)
        {
            UTF8ToSHA1 sha1 = new UTF8ToSHA1();
            string hashedPassword = sha1.ConvertToHash(password);
            string passwordSuffix = hashedPassword.Substring(5);
            return passwordSuffix;
        }

        /// <summary>
        /// Method IsPasswordPwned checks if the password has been compromised by using the
        /// PasswordOccurrences method to retrieve an integer value of how many times a password
        /// has been seen on Troy Hunt's HaveIBeenPwned API
        /// </summary>
        /// <param name="passwordToCheck">The password to be checked</param>
        /// <returns>bool identicalHashExists as true if the password has been seen
        /// more than once or false if the password has not been seen
        /// </returns>
        public static async Task<bool> IsPasswordPwned(string passwordToCheck)
        {
            bool identicalHashExists = false;
            int passwordOccurrenceCount = await PasswordOccurrences(passwordToCheck);
            
            if(passwordOccurrenceCount > 0)
            {
                identicalHashExists = true;
            }
            return identicalHashExists;
        }

        /// <summary>
        /// Method GetResponseCode returns the response message when attempting a request
        /// from the API
        /// </summary>
        /// <param name="path">String that holds the uri for the HttpClient to access</param>
        /// <returns>The response code as an HttpResponseMessage object</returns>
        public static async Task<HttpResponseMessage> GetResponseCode(string path)
        {
            //HttpResponseMessage will be used to hold the response code the API returns
            HttpResponseMessage responseMessage = null;
            try
            {
                var response = await client.GetAsync(path);
                responseMessage = response;
            }
            catch (Exception e)
            {
                //This is where logging should go
            }
            return responseMessage;
        }

        /// <summary>
        /// Method PasswordOccurrences retrieves the amount of occurrences a password has been
        /// seen by sending the first five characters from the hashed password to Troy Hunt's API
        /// and retrieving a list of password hashes that match the prefix. The suffix hash is then
        /// searched for in the list and split to retrieve the amount of times it has been seen.
        /// </summary>
        /// <param name="passwordToCheck">The password to be checked</param>
        /// <returns>Returns integer value of occurrences if any were found. Returns 0 if none were
        /// found.
        /// </returns>
        public static async Task<int> PasswordOccurrences(string passwordToCheck)
        {
            //HttpContent object will be used to hold the password hashes the API returns
            HttpContent retrievedPasswordHashes = null;

            string firstFiveChars = GetFirst5HashChars(passwordToCheck);
            string hashSuffix = GetHashSuffix(passwordToCheck);
            string path = "https://api.pwnedpasswords.com/range/" + firstFiveChars;

            try
            {
                //Gets the response code from an Http request
                var response = await GetResponseCode(path);
                //If response code is 200
                if (response.IsSuccessStatusCode)
                {
                    retrievedPasswordHashes = response.Content;
                    //Content reader holds all the returned hashes for reading
                    using (StreamReader contentReader = new StreamReader(await retrievedPasswordHashes.ReadAsStreamAsync()))
                    {
                        //While reader is not at end of retrieved passwords
                        while (!contentReader.EndOfStream)
                        {
                            //Variable passwordInfo holds the hash suffix as it's read
                            var passwordInfo = await contentReader.ReadLineAsync();
                            //Split the hash using semicolon to get the hash suffix in the first index and count in the second index
                            var splitToCount = passwordInfo.Split(':');
                            if (splitToCount.Length == 2 && splitToCount[0].Equals(hashSuffix))
                            {
                                //Get the value that a password has been seen. 
                                //If it does not encounter a value count or fails to parse, count is 0
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
