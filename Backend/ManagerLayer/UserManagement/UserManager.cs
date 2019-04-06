using DataAccessLayer.Tables;
using ServiceLayer.Interface;
using ServiceLayer.Services;
using System;

namespace ManagerLayer.UserManagement
{
    public class UserManager
    {
        private IUserService _userService;

        public UserManager()
        {
            _userService = new UserService();
        }

        public User CreateUser(string firstName, string lastName, string userName, string city, string state, string country, DateTime dob)
        {
            User newUser = new User(
            _userService.GetNextUserID(),
            firstName,
            lastName,
            userName,
            city,
            state,
            country,
            dob,
            true
            );

            if (_userService.CreateUser(newUser))
            {
                return newUser;
            }
            return null;
        }

        public User GetUser(int userID)
        {
            User retrievedUser = _userService.GetUser(userID);
            if (retrievedUser != null)
            {
                return retrievedUser;
            }
            return null;
        }

        public bool DeleteUserByID(int userID)
        {
            User retrievedUser = _userService.GetUser(userID);
            if(retrievedUser != null)
            {
                return _userService.DeleteUser(retrievedUser);
            }
            return false;
        }

        public bool UserExist(int userID)
        {
            return _userService.IsUsernameFoundById(userID);
        }
    }
}
