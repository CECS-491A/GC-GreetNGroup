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
        public static void AddClaimsToUsers(string claimId, string userId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var claim = new UserClaim() { UserId = userId, ClaimId = claimId };
                ctx.UserClaims.Add(claim);
                ctx.SaveChanges();
            }
        }

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
        
        //
        // all of the following is not using entity framework --plain sql server code -- not agnostic
        //
        
        // String to connect to database
        //private static string _connectionString = "Server=greetngroupdb.cj74stlentvn.us-west-1.rds.amazonaws.com;" +
        //                                          "Database=;User Id=gucci;Password=password123!;";

        /// <summary>
        /// To generate SELECT statements to send to the database and return data
        ///
        /// generic "SELECT x FROM x WHERE x"
        /// </summary>
        /// <param name="selectArg"></param> what to SELECT from
        /// <param name="fromArg"></param> what to FROM from
        /// <param name="whereArg"></param>
        /// <returns></returns>
        //public string QuerySelectWhere(string selectArg, string fromArg, string whereArg)
        //{
            //string q = "SELECT " + selectArg + " FROM " + fromArg + " WHERE " + whereArg ";";
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
                //using (SqlCommand cmd = new SqlCommand(q), connection)
                //{
                    //using (SqlDataReader reader = command.ExecuteReader())
                    //{
                        //while (reader.Read())
                        //{
                            //return reader[selectArg];
                        //}
                    //}
                //}
            //}
        //}
    }
}