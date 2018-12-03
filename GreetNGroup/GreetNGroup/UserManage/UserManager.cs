using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GreetNGroup.User;
using GreetNGroup.Account_Fields_Random_Generator;
namespace GreetNGroup.UserManage
{
    /// <summary>
    /// User management class that allows the management of accounts on the website
    /// </summary>
    public static class UserManager
    {
        public static UserAccount addAccount(String userName, String city, String state, String country, String DOB)
        {
            string randomPassword = RandomFieldGenerator.generatePassword();
            UserAccount newAccount = new UserAccount(userName, randomPassword, "", "", city, state, country, DOB,"", "", "");


            return newAccount;

        }
    }
}