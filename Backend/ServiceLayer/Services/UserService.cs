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
        private IGNGLoggerService _gngLoggerService;

        public UserService()
        {
            _cryptoService = new CryptoService();
            _gngLoggerService = new GNGLoggerService();
        }

        /*
         * The functions within this service make use of the database context
         * and similarly attempt to catch
         *      ObjectDisposedException
         * to ensure the context is still valid and we want to catch the error
         * where it has been made
         *
         */

        /// <summary>
        /// The following region handles inserts into the user table of the database
        /// </summary>
        #region Insert User Information

        // Inserts given user object into database
        public bool InsertUser(User user)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Catch existing users
                    if (user.UserName != null)
                    {
                        if (ctx.Users.Any(c => c.UserName.Equals(user.UserName)))
                        {
                            return false;
                        }
                    }

                    // Adds user
                    ctx.Users.Add(user);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        #endregion

        /// <summary>
        /// The following region handles updating user information within the database
        /// </summary>
        #region Update User Information
        
        // Updates user by replacing user object in database with new user object with updated fields
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }
        
        // Updates city information on user
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        // Updates state information of user
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        #endregion

        /// <summary>
        /// The following region handles deletion of data from the user table in the database
        /// </summary>
        #region Delete User Information
        
        /*
         * For our application, the DeleteUser function is made to set user values to null
         * apart from UserId and userName which is used as reference 
         */
        public bool DeleteUser(User userToDelete)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    /*
                     * User(int uId, string firstName, string lastName, string userName, string city,
                        string state, string country, DateTime dob, bool isActivated)
                     */
                    var user = ctx.Users.FirstOrDefault(c => c.UserId.Equals(userToDelete.UserId));
                    var blankUser = new User(user.UserId, null, null, "User has been deleted", null, null,
                                            null, DateTime.Now, false);
                    if (user != null)
                    {
                        //ctx.Users.Remove(user);
                        user = blankUser;
                        ctx.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        #endregion

        /// <summary>
        /// The following region checks information of users within the database
        /// </summary>
        #region User Information Check

        // Finds username by string and returns true if found
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        // Finds the username by uId returns true if found
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        #endregion

        /// <summary>
        /// The following region retrieves user information from the database
        /// </summary>
        #region User Information Retrieval

        // Returns userId found through username
        public int GetUserUid(string username)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.FirstOrDefault(u => u.UserName.Equals(username));
                    return user.UserId;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return -1;
            }
        }

        // Returns list of claims given a username string
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return claimsList;
            }
        }

        // Returns count of registered users
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return count;
            }
        }

        // Returns user found via username string
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return null;
            }
        }

        // Returns user based on userId found
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
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return null;
            }
        }

        // Finds the next available userId
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
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return 1;
            }
        }

        #endregion
    }
}
