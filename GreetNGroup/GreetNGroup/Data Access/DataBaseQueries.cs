using System.Collections.Generic;
using System.Linq;

namespace GreetNGroup.Data_Access
{
    public static class DataBaseQueries
    {
        public static bool IsCurrentUsername(string username)
        {
            using (var ctx = new GreetNGroupContext())
            {
            }
        }
        #region Claim Queries

        /// <summary>
        /// Function that pulls claim information of the user to store inside
        /// the token
        /// </summary>
        /// <param name="userId"> Id reference to the user </param>
        /// <returns></returns>
        public static List<string> ListUserClaims(string userId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var claimsTable = new List<ClaimsTable>();
                var claims = new List<string>();
                
                ClaimsTable currTable;
                
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