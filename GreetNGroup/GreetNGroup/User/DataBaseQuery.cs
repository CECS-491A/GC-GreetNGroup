using GreetNGroup.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreetNGroup.User
{
    public static class DataBaseQuery
    {
        /// <summary>
        /// Check to see of the username already exist
        /// </summary>
        /// <param name="Username">The checked username</param>
        /// <param name="Users">List if users</param>
        /// <returns>Whether the name is a dulicate or not</returns>
        public static Boolean isUserNameDuplicate(String Username, List<UserAccount> Users)
        {
            Boolean isDupe = false;
            for (var i = 0; i < Users.Count; i++)
            {
                if (Users[i].Username.Equals(Username))
                {
                    isDupe = true;
                }
            }

            return isDupe;

        }
        /// <summary>
        /// Inserts the user into the list
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="Users"></param>
        /// <returns>The new list of users</returns>
        public static List<UserAccount> insertUser(UserAccount newUser, List<UserAccount> Users)
        {
            Users.Add(newUser);
            return Users;
        }
        /// <summary>
        /// Deletes user from the list
        /// </summary>
        /// <param name="dUser">Deleted account</param>
        /// <param name="Users">List of users</param>
        /// <returns>New list of users</returns>
        public static List<UserAccount> deleteUser(UserAccount dUser, List<UserAccount> Users)
        {
            for(int i =0; i < Users.Count; i++)
            {
                if(dUser.UserID.Equals(Users[i].UserID) == true)
                {
                    Users.RemoveAt(i);
                }
            }
            return Users;
        }

        
    }
}