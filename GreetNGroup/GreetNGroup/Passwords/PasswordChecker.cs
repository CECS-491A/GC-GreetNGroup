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
        private const int MIN_PASSWORD_OCCURRENCES = 1;
        
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
            var identicalHashExists = false;
            var passwordOccurrenceCount = await PasswordOccurrences(passwordToCheck);
            
            if(passwordOccurrenceCount >= MIN_PASSWORD_OCCURRENCES)
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
        public async Task<HttpResponseMessage> GetResponseCode(string URL)
        {
            //HttpClient connects to Troy Hunt's HaveIBeenPwned API to retrieve possibly pwned passwords
            var client = new HttpClient();
            //HttpResponseMessage will be used to hold the response code the API returns
            HttpResponseMessage responseMessage = null;

            //HttpResponseMessage is set to the response given when the client attempts
            //a GET request from the API url
            responseMessage = await client.GetAsync(URL);
            
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

            if(string.IsNullOrEmpty(passwordToCheck))
            {
                return -1;
                throw new ArgumentNullException();
            }

            //Get the hash of the password and get the prefix and suffix of that password
            var hashedPassword = sha1.ConvertToHash(passwordToCheck);
            var hashSuffix = hashedPassword.Substring(5);
            var firstFiveChars = hashedPassword.Substring(0, 5);
            //Concat url to the prefix to retrieve passwords that match the prefix
            var path = "https://api.pwnedpasswords.com/range/" + firstFiveChars;

            try
            {
                var response = await GetResponseCode(path);

                if (response.IsSuccessStatusCode)
                {
                    //Get the password suffixes that matched the given prefix as HttpContent
                    retrievedPasswordHashes = response.Content;

                    //If the retrievedPasswordHashes is still null, throw exception
                    if(retrievedPasswordHashes == null)
                    {
                        return -1;
                        throw new NullReferenceException();
                    }

                    //Content reader holds all the returned hashes for reading
                    using (var contentReader = new StreamReader(await retrievedPasswordHashes.ReadAsStreamAsync()))
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
                //If response is not 200, throw exception
                else
                {
                    return -1;
                    throw new Exception();
                }
            }
            catch (NullReferenceException)
            {
                //Write the nullreferenceexception onto a trace log
                Trace.Listeners.Add(new TextWriterTraceListener("passwordlog.log"));
                Trace.AutoFlush = true;
                Trace.WriteLine("Cannot read an empty httpcontent");
            }
            catch (ArgumentNullException)
            {
                //Write the argumentnullexception onto a trace log
                Trace.Listeners.Add(new TextWriterTraceListener("passwordlog.log"));
                Trace.AutoFlush = true;
                Trace.WriteLine("Empty or null password invalid");
            }
            catch (Exception)
            {
                //Write the exception onto a trace log
                Trace.Listeners.Add(new TextWriterTraceListener("passwordlog.log"));
                Trace.AutoFlush = true;
                Trace.WriteLine("Unsuccessful API GET request");
            }
            return 0;
        }
    }
}
