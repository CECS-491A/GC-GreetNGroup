using System;
using System.Collections.Generic;

namespace GreetNGroup.UserManage
{
    /// <summary>
    /// User management class that allows the management of accounts on the website
    /// </summary>
    public interface IUserManager
    {
        void AddAccount(string jwt, string userName, string city, string state, string country, DateTime DOB);
        void DeleteAccount(string jwt, int uId);
        void UpdateAccount(string jwt, int uId, List<string> changedAttributes);
    }
}