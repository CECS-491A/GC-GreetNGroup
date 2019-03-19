using GreetNGroup.DataAccess.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GreetNGroup.DataAccess.Queries
{
    public class DbRetrieve
    {
        /// <summary>
        /// Retrieves Event through Id reference
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public static Event GetEventById(int eventId)
        {
            Event e = null;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    e = ctx.Events.FirstOrDefault(c => c.EventId.Equals(eventId));
                    return e;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return e;
            }
        }

        /// <summary>
        /// Gets the list of greetngroup claims a user has
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<Claim> GetUsersClaims(string username)
        {
            List<Claim> claimsList = new List<Claim>();

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var usersClaims = ctx.UserClaims.Where(c => c.User.UserName.Equals(username));
                    foreach (UserClaim claim in usersClaims)
                    {
                        claimsList.Add(claim.Claim);
                    }

                    return claimsList;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return claimsList;
            }
        }

        /// <summary>
        /// Finds the user's id and hashes it using the SHA256CryptoServiceProvider methods.
        /// The hashed id is then returned to the caller
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string GetUsersHashedUID(string username)
        {
            var hashedUid = "";

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.Where(u => u.UserName.Equals(username));
                    using (var sha256 = new SHA256CryptoServiceProvider())
                    {
                        //First converts the uID into UTF8 byte encoding before hashing
                        var hashedUIDBytes =
                            sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Select(id => id.UserId).ToString()));
                        var hashToString = new StringBuilder(hashedUIDBytes.Length * 2);
                        foreach (byte b in hashedUIDBytes)
                        {
                            hashToString.Append(b.ToString("X2"));
                        }

                        sha256.Dispose();
                        hashedUid = hashToString.ToString();
                    }
                }

                return hashedUid;
            }
            catch (ObjectDisposedException od)
            {
                // log
                return hashedUid;
            }
        }
    }
}