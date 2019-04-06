using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Tables;
using ServiceLayer.Interface;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {
        private ICryptoService _cryptoService;

        public UserService()
        {
            _cryptoService = new CryptoService();
        }

        /// <summary>
        /// The following region handles inserts into the user table of the database
        /// </summary>
        #region Insert User Information

        // For existing user object passed in to add into individual app
        public bool CreateUser(User user)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    ctx.Users.Add(user);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        #endregion

        /// <summary>
        /// The following region handles updating user information within the database
        /// </summary>
        #region Update User Information
        
        public bool UpdateUser(User updatedUser)
        {
            try
            {
                using(var ctx = new GreetNGroupContext())
                {
                    User retrievedUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(updatedUser.UserId));
                    if(retrievedUser != null)
                    {
                        retrievedUser = updatedUser;
                        ctx.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        #endregion

        /// <summary>
        /// The following region handles deletion of data from the user table in the database
        /// </summary>
        #region Delete User Information

        public bool DeleteUser(User userToDelete)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.FirstOrDefault(c => c.UserId.Equals(userToDelete.UserId));

                    if (user != null)
                    {
                        ctx.Users.Remove(user);
                        ctx.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        #endregion

        /// <summary>
        /// The following region checks information of users within the database
        /// </summary>
        #region User Information Check

        public bool IsUsernameFound(string username)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    return ctx.Users.Any(u => u.UserName.Equals(username));
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        public bool IsUsernameFoundById(int uId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Finds any username matching the parameter
                    var user = ctx.Users.Any(s => s.UserId == uId);
                    return user != false;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        #endregion

        /// <summary>
        /// The following region retrieves user information from the database
        /// </summary>
        #region User Information Retrieval

        public string GetUsersHashedUID(string username)
        {
            var hashedUid = "";

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.Where(u => u.UserName.Equals(username));
                    string userID = user.Select(id => id.UserId).ToString();
                    hashedUid = _cryptoService.HashSha256(userID);
                }

                return hashedUid;
            }
            catch (ObjectDisposedException od)
            {
                // log
                return hashedUid;
            }
        }

        public int GetUsersRegistered()
        {
            int count = 0;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    count = ctx.Users.Count();
                }

                return count;
            }
            catch (ObjectDisposedException od)
            {
                // log
                return count;
            }
        }

        public User GetUser(int userID)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var retrievedUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(userID));

                    if (retrievedUser != null)
                    {
                        return retrievedUser;
                    }
                    return null;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return null;
            }
        }
        
        //TODO: Add method to get the user from db using the username, return user object

        #endregion

        public int GetNextUserID()
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.OrderByDescending(p => p.UserId).FirstOrDefault();
                    return user.UserId + 1;
                }
            }
            catch
            {
                //log
                return 1;
            }
        }

    }
}
