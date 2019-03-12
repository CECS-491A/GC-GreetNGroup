using System;
using System.Linq;

namespace GreetNGroup.DataAccess.Queries
{
    public class DbCheck
    {
        /// <summary>
        /// Finds User based on userId -- uId
        /// returns a bool on whether or not it has been found
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public static bool IsUsernameFound(int uId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Finds any username matching the parameter
                    var user = ctx.Users.Any(s => s.UserId == uId);
                    return user != false;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }
        /// <summary>
        /// Override method that finds a username based on username parameter and 
        /// returns a bool based on whether it exists in the database or not
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool IsUsernameFound(string username)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    return ctx.Users.Any(u => u.UserName.Equals(username));
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        /// <summary>
        /// Checks if the password user has input matches the password that was
        /// registered with that username and returns a bool based on if it matches
        /// or not
        /// </summary>
        /// <param name="username">Username input provided by user</param>
        /// <param name="password">Hashed password input</param>
        /// <returns></returns>
        public static bool DoesPasswordMatch(string username, string password)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.Where(u => u.UserName.Equals(username));
                    return user.Any(p => p.Password.Equals(password));
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        /// <summary>
        /// Finds if claim exists within the db using claimId
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        public static bool IsClaimInTable(int claimId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    return ctx.Claims.Any(u => u.ClaimId.Equals(claimId));
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        /// <summary>
        /// Checks if a claim exists on current userId
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="claimId"></param>
        /// <returns></returns>
        public static bool IsClaimOnUser(int uId, int claimId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var userClaims = ctx.UserClaims.Where(u => u.UId.Equals(uId));
                    return userClaims.Any(c => c.ClaimId.Equals(claimId));
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        /// <summary>
        /// Override method that takes a username instead of uID and claimID
        /// </summary>
        /// <param name="username"></param>
        /// <param name="claimName"></param>
        /// <returns></returns>
        public static bool IsClaimOnUser(string username, string claimName)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var userClaims = ctx.UserClaims.Where(u => u.User.UserName.Equals(username));
                    return userClaims.Any(c => c.Claim.ClaimName.Equals(claimName));
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        /// <summary>
        /// Checks to see if an event exists by eventId
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public static bool IsEventIdFound(int eventId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    return ctx.Events.Any(c => c.EventId.Equals(eventId));
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

    }
}