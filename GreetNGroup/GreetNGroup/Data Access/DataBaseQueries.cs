using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Diagnostics;
using System.Linq;
using System.Web;
using GreetNGroup.Claim_Controls;
using GreetNGroup.Data_Access;
using Microsoft.Ajax.Utilities;

namespace GreetNGroup.Data_Access
{
    public static class DataBaseQueries
    {
        #region Claim Queries
        /// <summary>
        /// Function that adds claims to users
        /// </summary>
        /// <param name="claimId"> Id to reference claim in database </param>
        /// <param name="userId"> Id to reference user in database </param>
        public static void AddClaimsToUsers(string claimId, string userId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var claim = new UserClaim() { UserId = userId, ClaimId = claimId };
                ctx.UserClaims.Add(claim);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Function that pulls claim information of the user to store inside
        /// the token
        /// </summary>
        /// <param name="userId"> Id reference to the user </param>
        /// <returns></returns>
        public static List<string> FindClaimsFromUser(string userId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                List<ClaimsTable> claimsTable = new List<ClaimsTable>();
                ClaimsTable currTable;
                List<string> claims = new List<string>();
                
                List<UserClaim> userClaims = ctx.UserClaims.Where(c => userId.Contains(c.UserId)).ToList();

                foreach (var t in userClaims)
                {
                    var currClaim = t.ClaimId;
                    claimsTable.Add(ctx.ClaimsTables.Where(u => currClaim.Equals(u.ClaimId)).ToList()[0]);
                }

                foreach (var t1 in claimsTable)
                {
                    claims.Add(t1.Claim);
                }
                return claims;
            }
        }
        #endregion
    }
}