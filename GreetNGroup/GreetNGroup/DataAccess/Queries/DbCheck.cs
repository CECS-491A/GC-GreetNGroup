using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public static bool IsUsernameFound(string uId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                 // Finds any username matching the parameter
                 var user = ctx.Users.Any(s => s.UserId == uId);
                 return user != false;
            }
        }

        /// <summary>
        /// Checks if a claim exists on current userId
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="claimId"></param>
        /// <returns></returns>
        public static bool IsClaimOnUser(string uId, int claimId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var userClaims = ctx.UserClaims.Where(u => u.UId.Equals(uId));
                return userClaims.Any(c => c.ClaimId.Equals(claimId));
            }
        }

        /// <summary>
        /// Checks to see if an event exists by eventId
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public static bool IsEventIdFound(string eventId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                return ctx.Events.Any(c => c.EventId.Equals(eventId));
            }
        }
    }
}