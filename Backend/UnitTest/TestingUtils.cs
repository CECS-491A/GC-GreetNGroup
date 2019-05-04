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
            var newUser = new User
            {
                UserId = _userService.GetNextUserID()
            };
            return InsertUserInDB(newUser);
        }

        public User InsertUserInDB(User userToAdd)
        {
            _userService.InsertUser(userToAdd);
            return userToAdd;
        }

        public bool DeleteUserFromDB(User userToDelete)
        {
            return _userService.DeleteUser(userToDelete);
        }
    }
}
