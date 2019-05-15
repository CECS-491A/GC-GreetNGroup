using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class TestingUtils
    {
        private UserService _userService = new UserService();

        public User CreateUser()
        {
            var newUser = new User();
            newUser.UserId = _userService.GetNextUserID();
            newUser.UserName = "test@gmail.com";
            return newUser;
        }

        public User InsertUserInDB(User userToAdd)
        {
            if (_userService.InsertUser(userToAdd))
            {
                return userToAdd;
            }

            return null;
        }

        public bool DeleteUserFromDB(User userToDelete)
        {
            return _userService.DeleteUser(userToDelete);
        }
    }
}
