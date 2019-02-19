using System;
using System.Collections.Generic;
using System.Linq;
using GreetNGroup.Models;
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
                    var userClaims = ctx.UserClaims.Where(s => s.UserId == userID);
                    //Retrieves user 
                    var user = ctx.UserTables.Single(s => s.UserId == userID);
                    //Remove claims first because UID is primary key
                    ctx.UserClaims.RemoveRange(userClaims);
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
                        // Only non admins can be edited by admins
                        var adminRights = DataBaseCheck.FindClaim(userID, "AdminRights");

                        if (!adminRights)
                        {
                            DeleteUser(userID);
                        }
                        else
                        {
                            throw new System.ArgumentException("Account cannot be deleted");
                        }   
                    }
                    else
                    {
                        throw new System.ArgumentException("userID doesn't exist");
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