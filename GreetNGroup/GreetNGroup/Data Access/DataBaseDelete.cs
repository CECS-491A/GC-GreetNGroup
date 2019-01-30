using System;
using System.Linq;

namespace GreetNGroup.Data_Access
{
    public class DataBaseDelete
    {
        /// <summary>
        /// Deletes a user in the database given the following UID
        /// </summary>
        /// <param name="UserID">User ID</param>
        public static void DeleteUser(string UserID)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    //Retrieve user claims
                    var Userclaims = ctx.UserClaims.Where(s => s.UserId == UserID);
                    //Retrieves user 
                    var user = ctx.UserTables.Single(s => s.UserId == UserID);
                    //Remove claims first because UID is primary key
                    ctx.UserClaims.RemoveRange(Userclaims);
                    //Delete user next
                    ctx.UserTables.Remove(user);
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //Log excepetion e
                Console.WriteLine(e);
            }
        }
    }
}