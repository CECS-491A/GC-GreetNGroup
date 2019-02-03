using GreetNGroup.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using GreetNGroup.Claim_Controls;
using GreetNGroup.Tokens;

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
        
        
        /*
         * CheckDeleteClaim and CheckEditClaim will be changed to purely make checks for specific
         * claims needed to do their function, DataBaseDelete will handle the actual function
         */
        
        

        
        /// <summary>
        /// Checks if the account can be edited
        /// </summary>
        /// <param name="UserID">Edited Accounts user id</param>
        /// <param name="attributeContents">List of attributes that will replace current attributes</param>
        public static void CheckEditClaim(string userID, List<string> attributeContents)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    //Checks if the account exist/has any claims
                    var user = ctx.UserTables.Count(s => s.UserId == userID);
                    if(user != 0)
                    {
                        //Retrive user claims
                        var userClaims = ctx.UserClaims.Count(s => s.UserId == userID);
                        //Turns claims into a list
                        var claims = DataBaseQueries.ListUserClaims(userID);
                        //Check if account can be edited
                        bool canEdit = ValidationManager.checkAccountEditable(claims);
                        if (canEdit)
                        {
                            
                            // Will have to edit here
                            
                            
                            //UpdateUser(UserID, attributeContents);
                        }
                        else
                        {
                            throw new System.ArgumentException("Account cannot be updated", "Claim");
                        }
                    }
                    else
                    {
                        throw new System.ArgumentException("Account does not exist", "Database");
                    }                   
                }
            }
            catch (Exception e)
            {
                //Log Exception
                Console.WriteLine(e);
            }
        }    
    }
}