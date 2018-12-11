using System;
using System.Collections.Generic;

namespace GreetNGroup.UserManage
{
    /// <summary>
    /// User management class that allows the management of accounts on the website
    /// </summary>
    public interface IUserManager
    {
        void AddAccount(String userName, String city, String state, String country, DateTime DOB);
        void DeleteAccount(string UID);
        void UpdateAccount(string UserID, List<string> changedAttributes);
    }
}