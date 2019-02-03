using System;
using System.Collections.Generic;
using System.Linq;

namespace GreetNGroup.Data_Access
{
    public static class DataBaseUpdate
    {
        /// <summary>
        /// Checks if the account can be edited
        /// </summary>
        /// <param name="userID">Edited Accounts user id</param>
        /// <param name="attributeContents">List of attributes that will replace current attributes</param>
        public static void TryUpdateUser(string userID, List<string> attributeContents)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    //Checks if the account exist/has any claims
                    var user = ctx.UserTables.Count(s => s.UserId == userID);
                    if(user != 0)
                    {
                        //Retrieve user claims
                        var userClaims = ctx.UserClaims.Count(s => s.UserId == userID);
                        //Turns claims into a list
                        var claims = DataBaseQueries.ListUserClaims(userID);
                        // Only non admins can be edited by admins
                        var adminRights = DataBaseCheck.FindClaim(userID, "AdminRights");
                        
                        if (!adminRights)
                        {
                            UpdateUser(userID, attributeContents);
                        }
                        else
                        {
                            throw new System.ArgumentException("Account cannot be updated");
                        }
                    }
                    else
                    {
                        throw new System.ArgumentException("Account does not exist");
                    }                   
                }
            }
            catch (Exception e)
            {
                //Log Exception
                Console.WriteLine(e);
            }
        } 
        
        /// <summary>
        /// Updates the user with new content
        /// </summary>
        /// <param name="userID">Edited User's user ID</param>
        /// <param name="attributeContents">List that will replace old user information</param>
        private static void UpdateUser(string userID, List<string> attributeContents)
        {
            var currentAttributes = new List<string>();
            var ctx = new GreetNGroupContext();

            //Try statement to fill the variables with user's current attributes
            try
            {
                using (ctx)
                {
                    var userToUpdate = ctx.UserTables.Single(s => s.UserId == userID);
                    currentAttributes.Add(userToUpdate.FirstName);
                    currentAttributes.Add(userToUpdate.LastName);
                    currentAttributes.Add(userToUpdate.UserName);
                    currentAttributes.Add(userToUpdate.City);
                    currentAttributes.Add(userToUpdate.State);
                    currentAttributes.Add(userToUpdate.Country);
                    currentAttributes.Add(userToUpdate.DoB.ToString());
                    currentAttributes.Add(userToUpdate.SecurityQuestion);
                    currentAttributes.Add(userToUpdate.SecurityAnswer);
                    currentAttributes.Add(userToUpdate.isActivated.ToString());
                    //Instead of naming all the columns in the table
                    //Use some method to get all the column data 
                    //in a row and add it to the list to allow for new columns to be added
                    //and column names aren't hardcoded
                }
            }
            catch (Exception e)
            {
                //log
            }
            //For loop to update the attributes with new values, if there are values to update it to
            for (int i = 0; i < attributeContents.Count; i++)
            {
                if (!attributeContents[i].Equals("."))
                {
                    currentAttributes[i] = attributeContents[i];
                }
            }
            //Try statement update the user in the database
            try
            {
                using (ctx)
                {
                    var userToUpdate = ctx.UserTables.Single(s => s.UserId == userID);
                    userToUpdate.FirstName = currentAttributes[0];
                    userToUpdate.LastName = currentAttributes[1];
                    userToUpdate.UserName = currentAttributes[2];
                    userToUpdate.City = currentAttributes[3];
                    userToUpdate.State = currentAttributes[4];
                    userToUpdate.Country = currentAttributes[5];
                    userToUpdate.DoB = Convert.ToDateTime(currentAttributes[6]);
                    userToUpdate.SecurityQuestion = currentAttributes[7];
                    userToUpdate.SecurityAnswer = currentAttributes[8];
                    userToUpdate.isActivated = currentAttributes[9].Equals("true");
                    ctx.SaveChanges();
                    //Instead of calling each index, use a for loop with some method to 
                    //take all list values and put into the row 
                }
            }
            catch(Exception e)
            {
                //log
            }
        } 
    }
}