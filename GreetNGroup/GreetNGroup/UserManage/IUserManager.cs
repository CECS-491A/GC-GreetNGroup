
using GreetNGroup.SiteUser;
using System;
using System.Collections.Generic;

namespace GreetNGroup.UserManage
{
    /// <summary>
    /// User management class that allows the management of accounts on the website
    /// </summary>
    public interface IUserManager
    {
        object AddAccount(String userName, String city, String state, String country, String DOB, List<UserAccount> Users);
        Boolean DeleteAccount(UserAccount deleteUser);
        void ChangeEnable(UserAccount user, Boolean enable);
    }
}