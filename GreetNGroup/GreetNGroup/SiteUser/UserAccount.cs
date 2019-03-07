using System;
using System.Collections.Generic;
using GreetNGroup.UserManage;
using GreetNGroup.Validation;
/*
Basic user account class with all fields needed for a registered account     
*/
namespace GreetNGroup.SiteUser
{
    public class UserAccount : IUserManager
    {
        #region User Management
        /// <summary>
        /// Creats a new USer Account given the attributes and if the Username has not been taken.
        /// </summary>
        /// <param name="jwt">Jason web token with claims</param>
        /// <param name="userName">New user Name</param>
        /// <param name="city">New City Location</param>
        /// <param name="state">New State Location</param>
        /// <param name="country">New Country Location</param>
        /// <param name="DOB">New user's Date of birth</param>
        public void AddAccount(string jwt, string userName, string city, string state, string country, DateTime DOB)
        {
            //Check the current user's claim first
            ValidationManager.CheckAddToken(jwt, userName, city, state, country, DOB);

        }
        /// <summary>
        /// Checks claims of the users and returns if the user can delete and the account be deleted
        /// <param name="jwt">Jason web token with claims</param>
        /// <param name="userName">New user Name</param>
        /// <param name="UserID">User Account that will be deletd</param>
        public void DeleteAccount(string jwt, int userID)
        {
            ValidationManager.CheckDeleteToken(jwt, userID);
        }

        /// <summary>
        /// Updates account information
        /// </summary>
        /// <param name="jwt">Jason web token with claims</param>
        /// <param name="UserID">Account that is being updated</param>
        /// <param name="changedAttributes">The attributes the account will change into</param>
        public void UpdateAccount(string jwt, int userID, List<string> changedAttributes)
        {
            ValidationManager.CheckEditToken(jwt, userID, changedAttributes);
        }

        #endregion
    }
}