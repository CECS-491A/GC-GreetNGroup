using System;
using System.Text;
using System.Security.Cryptography;

namespace GreetNGroup.Passwords
{
    /// <summary>
    /// Converts UTF8 passwords to SHA1 hashes to send to HaveIBeenPwned API
    /// to check if password has been "pwned"
    /// </summary>
    public class UTF8ToSHA1
    {
        //SHA1Managed object contains methods to convert plaintext to SHA1 hash
        private SHA1Managed sha1Password = new SHA1Managed();

        /// <summary>
        /// Converts string input (passwords) into a SHA1 hash using the methods provided
        /// by the SHA1Managed class
        /// </summary>
        /// <param name="input">the string to be converted to a SHA1 hash</param>
        /// <returns>The SHA1 hash in string form. Returns null if "" or null input.</returns>
        public string ConvertToHash(string input)
        {

            try
            {
                //If password is null or "", return null and throw NullReferenceException
                if (string.IsNullOrEmpty(input) == true)
                {
                    return null;
                    throw new NullReferenceException();
                }
                using (sha1Password)
                {
                    //Computes hash based on UTF8 encoding
                    var passwordHash = sha1Password.ComputeHash(Encoding.UTF8.GetBytes(input));
                    var hashToString = new StringBuilder(passwordHash.Length * 2);
                    foreach (byte b in passwordHash)
                    {
                        //Hash contains capital letters
                        hashToString.Append(b.ToString("X2"));
                    }
                    //SHA1Managed parent is IDisposable interface and can be disposed to optimize runtime
                    sha1Password.Dispose();
                    return hashToString.ToString();
                }
            }
            catch (ArgumentNullException)
            {
                //This is where logging should go
                return null;
            }

        }
    }
}