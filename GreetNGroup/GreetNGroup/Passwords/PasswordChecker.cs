using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace GreetNGroup.Passwords
{

    /// <summary>
    /// Holds the methods that send the SHA1 to the HaveIBeenPwned API to check if
    /// the password has been "pwned"
    /// </summary>
    public class PasswordChecker
    {
        //A constant integer that allows for easy change in how many occurences are considered for a pwned password
        const int minimumPasswordOccurences = 1;
        
        /// <summary>
        /// Default constructor for PasswordChecker
        /// </summary>
        public PasswordChecker()
        {

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
        public async Task<bool> IsPasswordPwned(string passwordToCheck)
        {
            bool identicalHashExists = false;
            int passwordOccurrenceCount = await PasswordOccurrences(passwordToCheck);
            
            if(passwordOccurrenceCount >= minimumPasswordOccurences)
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
        public async Task<HttpResponseMessage> GetResponseCode(string passwordToCheck)
        {
            UTF8ToSHA1 sha1 = new UTF8ToSHA1();
            //HttpClient connects to Troy Hunt's HaveIBeenPwned API to retrieve possibly pwned passwords
            HttpClient client = new HttpClient();
        //HttpResponseMessage will be used to hold the response code the API returns
        HttpResponseMessage responseMessage = null;
            try
            {
                var hashedPassword = sha1.ConvertToHash(passwordToCheck);
                var firstFiveChars = hashedPassword.Substring(0, 5);
                var path = "https://api.pwnedpasswords.com/range/" + firstFiveChars;
                var response = await client.GetAsync(path);
                responseMessage = response;
                //If not successful then log the event
                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
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
        public async Task<int> PasswordOccurrences(string passwordToCheck)
        {
            UTF8ToSHA1 sha1 = new UTF8ToSHA1();
            HttpContent retrievedPasswordHashes = null;

            var hashedPassword = sha1.ConvertToHash(passwordToCheck);
            var hashSuffix = hashedPassword.Substring(5);
            try
            {
                
                var response = await GetResponseCode(passwordToCheck);

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
            catch (NullReferenceException)
            {
                //Write the nullreferenceexception onto a trace log
                Trace.Listeners.Add(new TextWriterTraceListener("passwordlog.log"));
                Trace.AutoFlush = true;
                Trace.WriteLine("Cannot read an empty httpcontent");
            }
            return 0;
        }
    }
}
