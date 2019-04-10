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

        public bool InsertUser(User user)
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

        public bool UpdateUserCity(int uId, string city)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    User curUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(uId));
                    if (curUser != null)
                    {
                        curUser.City = city;
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

        public bool UpdateUserState(int uId, string state)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    User curUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(uId));
                    if (curUser != null)
                    {
                        curUser.State = state;
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
                    return user;
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
                    var userId = user.Select(id => id.UserId).ToString();

                    hashedUid = _cryptoService.HashSha256(userId);
                }

                return hashedUid;
            }
            catch (ObjectDisposedException od)
            {
                // log
                return hashedUid;
            }
        }

        public List<Claim> GetUsersClaims(string username)
        {
            List<Claim> claimsList = new List<Claim>();
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var usersClaims = ctx.UserClaims.Where(c => c.User.UserName.Equals(username));
                    foreach (UserClaim claim in usersClaims)
                    {
                        claimsList.Add(claim.Claim);
                    }

                    return claimsList;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return claimsList;
            }
        }

        public int GetRegisteredUserCount()
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

        public User GetUserByUsername(string username)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.FirstOrDefault(c => c.UserName.Equals(username));

                    return user;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return null;
            }
        }

        public User GetUserById(int userID)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.FirstOrDefault(c => c.UserId.Equals(userID));

                    if (user != null)
                    {
                        return user;
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

        //TODO: Add method to get the user from db using the username, return user object

        #endregion
    }
}
