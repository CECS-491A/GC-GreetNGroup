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
        public static bool FindUsername(string uId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                 // Finds any username matching the parameter
                 var user = ctx.Users.Any(s => s.UserId == uId);
                 return user != false;
            }
        }

        public static bool FindClaimFromUser(string uId, int claimId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var userClaims = ctx.UserClaims.Where(u => u.UId.Equals(uId));
                return userClaims.Any(c => c.ClaimId.Equals(claimId));
            }
        }
    }
}