using System.Collections.Generic;
using System.Linq;

namespace GreetNGroup.Data_Access
{
    public static class DataBaseQueries
    {
        public static string CurrentPassword(string userID)
        {
            string userPassword = "";
            using (var ctx = new GreetNGroupContext())
            {
                string passwordInDB = ctx.UserTables.Where(p => userID.Contains(p.Password)).ToString();
                userPassword = passwordInDB;
            }
            return userPassword;
        }

        public static string CheckIfUserExists(string userName)
        {
            string userID = "";
            using (var ctx = new GreetNGroupContext())
            {
                string userNameInDB = ctx.UserTables.Where(id => userName.Contains(id.UserId)).ToString();
                userID = userNameInDB;
            }
            return userID;
        }

        public static string CurrentUsername(string userID)
        {
            string userName = "";
            using (var ctx = new GreetNGroupContext())
            {
                string userNameInDB = ctx.UserTables.Where(u => userID.Contains(u.UserName)).ToString();
                userName = userNameInDB;
            }
            return userName;
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