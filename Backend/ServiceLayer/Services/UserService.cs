using System;
using System.Collections.Generic;
using System.Linq;
using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;
using Gucci.DataAccessLayer.DataTransferObject;
using Gucci.ServiceLayer.Model;
using Gucci.DataAccessLayer.Models;
using System.Data.Entity;

namespace Gucci.ServiceLayer.Services
{
    public class UserService : IUserService
    {
        private ILoggerService _gngLoggerService;
        private Configurations configurations;

        public UserService()
        {
            _gngLoggerService = new LoggerService();
            configurations = new Configurations();
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
                using (var ctx = new GreetNGroupContext())
                {
                    ctx.Entry(updatedUser).State = EntityState.Modified;
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


        /// <summary>
        /// Method IsUserAtMaxEventCreation queries the database to check the creation
        /// count of the user attempting to create an event. If the user has reached 5
        /// or more events created, the method returns false and the user cannot create
        /// any more events.
        /// </summary>
        /// <param name="userId">Hashed user id of the user attempting to create an event</param>
        /// <returns>Return a bool value depending on if the user has reached the creation
        /// count threshold or not</returns>
        public bool IsUserAtMaxEventCreation(int userId)
        {
            var isAtMax = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.FirstOrDefault(u => u.UserId.Equals(userId));
                    var creationCount = user.EventCreationCount;

                    if (creationCount >= 5)
                    {
                        isAtMax = true;
                    }
                }
                return isAtMax;
            }
            catch (Exception od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isAtMax;
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

        // Returns user found via partial match of username
        public List<DefaultUserSearchDto> GetDefaultUserInfoListByUsername(string username)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Searches user table in terms of the DefaultUserSearchDto to minimize
                    // columns returned 
                    var user = ctx.Users.Where(u => u.UserName.Contains(username))
                        .Select(u => new DefaultUserSearchDto()
                        {
                            Username = u.UserName
                        }).ToList();

                    return user;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return null;
            }
        }

        // Returns user found via EventId
        public List<DefaultUserSearchDto> GetDefaultUserInfoById(int userId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Searches user table in terms of the DefaultUserSearchDto to minimize
                    // columns returned 
                    var user = ctx.Users.Where(c => c.UserId.Equals(userId))
                        .Select(u => new DefaultUserSearchDto()
                        {
                            Username = u.UserName
                        }).ToList();

                    return user;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return null;
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

                    return user;
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

        #region Logging Functions

        /// <summary>
        /// Method LogEntryToWebsite logs when a user first enters GreetNGroup. The log
        /// will keep track of the url that the user landed on as an entrypoint. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">user ID (empty if not a registered user)</param>
        /// <param name="urlEntered">URL entry point</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if log was successfully made</returns>
        public bool LogEntryToWebsite(string usersID, string urlEntered, string ip)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            _gngLoggerService.CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "EntryToWebsite",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "User " + usersID + " entered at " + urlEntered
            };
            var logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogClicksMade logs a user navigating around GreetNGroup based on the
        /// url they started at and the url they ended at inside GreetNGroup. If the log
        /// failed to be made, it will increment the errorCounter.
        /// </summary>
        /// <param name="startPoint">Starting URL</param>
        /// <param name="endPoint">Ending URL</param>
        /// <param name="usersID">user ID (empty if user does not exist)</param>
        /// <param name="ip">IP address of the user/guest</param>
        /// <returns>Return true or false if the log was made successfully</returns>
        public bool LogClicksMade(string startPoint, string endPoint, string usersID, string ip)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            _gngLoggerService.CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "ClickEvent",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = startPoint + " to " + endPoint
            };
            var logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;

        }

        /// <summary>
        /// Method LogExitFromWebsite logs when a user exits GreetNGroup and goes off to a 
        /// URL outside of GreetNGroup. The log tracks the URL the user was last on before 
        /// exiting GreetNGroup. If the log was failed to be made, it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">user ID (blank if user is not registered)</param>
        /// <param name="urlOfExit">Last URL the user visited inside GreetNGroup</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if the log was successfully made</returns>
        public bool LogExitFromWebsite(string usersID, string urlOfExit, string ip)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            _gngLoggerService.CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "ExitFromWebsite",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "User " + usersID + " exited website from " + urlOfExit
            };
            var logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogAccountDeletion logs when a user deletes their GreetNGroup account.
        /// </summary>
        /// <param name="usersID">user ID</param>
        /// <param name="ip">IP address</param>
        /// <returns>Returns true or false if log was successfully made</returns>
        public bool LogAccountDeletion(string usersID, string ip)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            _gngLoggerService.CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "AccountDeletion",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "User " + usersID + " deleted account"
            };
            var logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        #endregion
    }
}
