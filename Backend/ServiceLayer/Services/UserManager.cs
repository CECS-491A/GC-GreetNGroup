using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Tables;
using ServiceLayer.Interface;

namespace ServiceLayer.Services
{
    public class UserManager
    {
        private ICryptoService _cryptoService;

        public UserManager()
        {
            _cryptoService = new CryptoService();
        }
        
        /// <summary>
        /// The following region handles inserts into the user table of the database
        /// </summary>
        #region Insert User Information

        public void InsertUser(int uId, string firstName, string lastName, string userName, string city,
            string state, string country, DateTime dob, bool isActivated)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = new User(uId, firstName, lastName, userName, city, state, country, dob,
                        isActivated);

                    ctx.Users.Add(user);

                    ctx.SaveChanges();
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
            }
        }

        #endregion

        /// <summary>
        /// The following region handles updating user information within the database
        /// </summary>
        #region Update User Information

        public void UpdateUserCity(int uId, string city)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    User curUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(uId));
                    if (curUser != null)
                        curUser.City = city;
                    ctx.SaveChanges();
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
            }
        }

        public void UpdateUserState(int uId, string state)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    User curUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(uId));
                    if (curUser != null)
                        curUser.State = state;
                    ctx.SaveChanges();
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
            }
        }

        public void UpdateUserCountry(int uId, string country)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    User curUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(uId));
                    if (curUser != null)
                        curUser.Country = country;
                    ctx.SaveChanges();
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
            }
        }

        #endregion

        /// <summary>
        /// The following region handles deletion of data from the user table in the database
        /// </summary>
        #region Delete User Information

        public void DeleteUserById(int userId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.FirstOrDefault(c => c.UserId.Equals(userId));

                    if (user != null) ctx.Users.Remove(user);

                    ctx.SaveChanges();
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
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

        public bool IsClaimOnUser(int uId, int claimId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var userClaims = ctx.UserClaims.Where(u => u.UId.Equals(uId));
                    return userClaims.Any(c => c.ClaimId.Equals(claimId));
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return false;
            }
        }

        #endregion
    }
}
