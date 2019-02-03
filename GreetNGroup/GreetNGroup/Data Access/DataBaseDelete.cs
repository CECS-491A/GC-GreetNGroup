using System;
using System.Collections.Generic;
using System.Linq;
using GreetNGroup.Validation;

namespace GreetNGroup.Data_Access
{
    public class DataBaseDelete
    {
        /// <summary>
        /// Function to directly delete a user
        /// </summary>
        /// <param name="userID">User ID</param>
        public static void DeleteUser(string userID)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    //Retrieve user claims
                    var Userclaims = ctx.UserClaims.Where(s => s.UserId == userID);
                    //Retrieves user 
                    var user = ctx.UserTables.Single(s => s.UserId == userID);
                    //Remove claims first because UID is primary key
                    ctx.UserClaims.RemoveRange(Userclaims);
                    //Delete user next
                    ctx.UserTables.Remove(user);
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //Log exception e
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Function to delete a user after passing a conditional check
        /// </summary>
        /// <param name="userID"></param>
        public static void TryDeleteUser(string userID)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Checks if the account of userID has claims
                    var userClaims = ctx.UserClaims.Count(s => s.UserId == userID);
                    if (userClaims > 0)
                    {
                        // Stores Claims into a list
                        var claimsList = DataBaseQueries.ListUserClaims(userID);
                        // Checks if the account can be deleted
                        
                    }
                }
            }
            catch (Exception e)
            {
                
            }
        }
        
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
                        List<string> checkClaims = DataBaseQueries.ListUserClaims(userID);
                        
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
                //Log Exception
                Console.WriteLine(e);
            }
        }
    }
}