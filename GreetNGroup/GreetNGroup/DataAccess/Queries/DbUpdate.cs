using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreetNGroup.DataAccess.Queries
{
    public class DbUpdate
    {
        /// <summary>
        /// Grabs user via userId and updates the password to specified new password
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="password"></param>
        public static void UpdateUserPassword(string uId, string password)
        {
            using (var ctx = new GreetNGroupContext())
            {
                User curUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(uId));
                if (curUser != null)
                    curUser.Password = password;
                ctx.SaveChanges();
            }
        }
    }
}