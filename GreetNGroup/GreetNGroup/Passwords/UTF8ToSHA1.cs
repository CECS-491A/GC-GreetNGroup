using System;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace GreetNGroup.Passwords
{
    /// <summary>
    /// Converts UTF8 passwords to SHA1 hashes to send to HaveIBeenPwned API
    /// to check if password has been "pwned"
    /// </summary>
    public class UTF8ToSHA1
    {

        /// <summary>
        /// Converts string input (passwords) into a SHA1 hash using the methods provided
        /// by the SHA1CryptoServiceProvider class
        /// </summary>
        /// <param name="password">the string to be converted to a SHA1 hash</param>
        /// <returns>The SHA1 hash in string form. Returns null if "" or null input.</returns>
        public string ConvertToHash(string password)
        {
            //SHA1CryptoServiceProvider object contains methods to convert plaintext to SHA1 hash
            SHA1CryptoServiceProvider sha1Password = new SHA1CryptoServiceProvider();

            string hashAsString = null;
            try
            {
                //If password is null or "", return null and throw NullReferenceException
                if (string.IsNullOrEmpty(password) == true)
                {
                    return null;
                    throw new NullReferenceException();
                }
                using (sha1Password)
                {
                    //Computes hash based on UTF8 encoding
                    var passwordHash = sha1Password.ComputeHash(Encoding.UTF8.GetBytes(password));
                    var hashToString = new StringBuilder(passwordHash.Length * 2);
                    foreach (byte b in passwordHash)
                    {
                        //Make it so hash contains capitalized characters
                        hashToString.Append(b.ToString("X2"));
                    }
                    //SHA1CryptoServiceProvider extends IDisposable and can be disposed for optimizing runtime
                    sha1Password.Dispose();
                    hashAsString = hashToString.ToString();
                }
            }
            catch (ArgumentNullException)
            {
                //Write the nullreferenceexception onto a trace log
                Trace.Listeners.Add(new TextWriterTraceListener("passwordlog.log"));
                Trace.AutoFlush = true;
                Trace.WriteLine("Cannot convert an empty/null password");
            }
            return hashAsString;

        }
    }
}