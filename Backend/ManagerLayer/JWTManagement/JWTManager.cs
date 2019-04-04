using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DataAccessLayer.Context;
using DataAccessLayer.Tables;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.Services;

namespace ManagerLayer.JWTManagement
{
    public class JWTManager
    {
        private IJWTService _JWTService;
        private IUserService _userService;
        public JWTManager()
        {
            _JWTService = new JWTService();
            _userService = new UserService();
        }

        /// <summary>
        /// Method GrantToken grants a user a JWT that will be used for authorization
        /// purposes when the user is using GreetNGroup. If the user is not registered
        /// with GreetNGroup the method will return null, which will be processed by the
        /// frontend to redirect the user to the GreetNGroup registration page for the user
        /// to input the necessary information to become a registered user for GreetNGroup.
        /// </summary>
        /// <param name="username">Username of user</param>
        /// <returns>Return JWT object</returns>
        public string GrantToken(string username)
        {
            if (_userService.IsExistingGNGUser(username))
            {
                var usersClaims = RetrieveClaims(username);
                var hashedUID = RetrieveHashedUID(username);
                var securityClaimsList = new List<System.Security.Claims.Claim>();

                //Takes the claims the user has and puts it in a list of Security Claims objects
                foreach (DataAccessLayer.Tables.Claim c in usersClaims)
                {
                    securityClaimsList.Add(new System.Security.Claims.Claim(c.ClaimName, hashedUID));
                }

                return _JWTService.CreateToken(username, securityClaimsList);
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Method RetrieveClaims gets the claims a user has and returns them in list form
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <returns>Returns that user's list of claims</returns>
        public List<Claim> RetrieveClaims(string username)
        {
            var claimsList = new List<Claim>();

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
            catch (ObjectDisposedException e)
            {
                // log
                return claimsList;
            }

        }

        /// <summary>
        /// Method RetrieveHashedUID finds the user's UID, hashes it, and returns the
        /// hash as a string for sensitive operations. The UID is hashed using SHA256
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <returns>Returns the hashed UID as a string</returns>
        public string RetrieveHashedUID(string username)
        {
            var hashedUID = "";

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var user = ctx.Users.Where(u => u.UserName.Equals(username));
                    using (var sha256 = new SHA256CryptoServiceProvider())
                    {
                        //First converts the uID into UTF8 byte encoding before hashing
                        var hashedUIDBytes =
                            sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Select(id => id.UserId).ToString()));
                        var hashToString = new StringBuilder(hashedUIDBytes.Length * 2);
                        foreach (byte b in hashedUIDBytes)
                        {
                            hashToString.Append(b.ToString("X2"));
                        }

                        sha256.Dispose();
                        hashedUID = hashToString.ToString();
                    }
                }
                return hashedUID;
            }
            catch (ObjectDisposedException od)
            {
                // log
                return hashedUID;
            }
        }
    }
}
