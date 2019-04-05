using System;
using System.Linq;
using DataAccessLayer.Context;
using ServiceLayer.Services;

namespace ManagerLayer.JWTManagement
{
    public class JWTManager
    {
        private IJWTService _JWTService;
        private IUserService _userService;
        private ICryptoService _cryptoService;

        public JWTManager()
        {
            _JWTService = new JWTService();
            _userService = new UserService();
            _cryptoService = new CryptoService();
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
                return _JWTService.CreateToken(username, RetrieveHashedUID(username));
            }
            else
            {
                return null;
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
                    string userID = user.Select(id => id.UserId).ToString();
                    hashedUID = _cryptoService.HashSha256(userID);
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
