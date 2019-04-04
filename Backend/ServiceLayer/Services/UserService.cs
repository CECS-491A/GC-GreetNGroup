using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Tables;

namespace ServiceLayer.Services
{
    public class UserService: IUserService
    {
        /// <summary>
        /// Method IsExistingGNGUser checks to see if the user has done the GreetNGroup specific
        /// registration that makes them a valid user of GreetNGroup
        /// </summary>
        /// <param name="username">Username registered in the SSO</param>
        /// <returns>Returns true or false depending if the user exists in the GNG context</returns>
        public bool IsExistingGNGUser(string username)
        {
            bool userExists = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    if (ctx.Users.Any(u => u.UserName.Equals(username)) == true)
                    {
                        userExists = true;
                    }
                }
                return userExists;
            }
            catch (ObjectDisposedException e)
            {
                //log
                return userExists;
            }

        }
    }
}
