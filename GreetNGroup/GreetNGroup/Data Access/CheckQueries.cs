using GreetNGroup.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreetNGroup.Data_Access
{
    public static class CheckQueries
    {
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
        /// Checks the claims of the account that is going to be deleted
        /// </summary>
        /// <param name="userID">Delete account user ID</param>
        public static void CheckDeleteClaim(string userID)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    //Checks if the account exist/has any claims
                    var userClaims = ctx.UserClaims.Count(s => s.UserId == userID);
                    if (userClaims > 0)
                    {
                        //turn claims into a list
                        List<string> checkClaims = DataBaseQueries.FindClaimsFromUser(userID);
                        //Checks if the account can be deleted
                        bool canDelete = ValidationManager.checkAccountEditable(checkClaims);
                        
                        if (canDelete == true)
                        {
                            
                            
                            // Will have to edit here
                            
                            
                            //DeleteUser(userID);
                        }
                        else
                        {
                            throw new System.ArgumentException("Account cannot be deleted", "Claim");
                        }                     
                    }
                    else
                    {
                        throw new System.ArgumentException("user ID doesn't exist exist", "Database");
                    }
                }
            }
            catch (Exception e)
            {
                //Log Excepetion
                Console.WriteLine(e);
            }
        }
        
        /// <summary>
        /// Checks if the account can be edited
        /// </summary>
        /// <param name="UserID">Editted Accounts user id</param>
        /// <param name="attributeContents">List of attributes that will replace current attributes</param>
        public static void CheckEditClaim(string UserID, List<string> attributeContents)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    //Checks if the account exist/has any claims
                    var user = ctx.UserTables.Count(s => s.UserId == UserID);
                    if(user != 0)
                    {
                        //Retrive user claims
                        var userClaims = ctx.UserClaims.Count(s => s.UserId == UserID);
                        //Turns claims into a list
                        var claims = DataBaseQueries.FindClaimsFromUser(UserID);
                        //Check if account can be edited
                        Boolean canEdit = ValidationManager.checkAccountEditable(claims);
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
                //Log Excepetion
                Console.WriteLine(e);
            }
        }    
    }
}