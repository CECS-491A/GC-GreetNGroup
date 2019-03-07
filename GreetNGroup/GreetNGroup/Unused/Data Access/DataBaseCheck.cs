using System;
using System.Collections.Generic;
using System.Linq;
using GreetNGroup.Claim_Controls;
using GreetNGroup.Tokens;
using GreetNGroup.Models;

namespace GreetNGroup.Data_Access
{
    public static class DataBaseCheck
    {
        /// <summary>
        /// This function checks a userId's claims with the claim specified in the argument
        /// returns true when the claim exists, else false
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="claimToCheck"></param>
        /// <returns></returns>
        public static bool FindClaim (string userId, string claimToCheck)
        {
            var claimsReq = new List<string> { claimToCheck };
            var currentUserToken = new Token(userId);
            var foundClaim = ClaimsAuthorization.VerifyClaims(currentUserToken, claimsReq);
            
            return foundClaim;
        }
        
        /// <summary>
        /// Finds a username from the database
        /// </summary>
        /// <param name="userName">New Username</param>
        public static bool FindUsername(string userName)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Finds any username matching the parameter
                    var user = ctx.UserTables.Any(s => s.UserName == userName);
                    return user != false;
                }
            }
            catch (Exception e)
            {
                //Log Excepetion
                return true;
            }
        }
    }
}