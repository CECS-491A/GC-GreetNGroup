using System;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;

namespace Gucci.ManagerLayer.LoginManagement
{
    public class LoginManager
    {
        private ICryptoService _cryptoService;
        private IUserService _userService;
        private IJWTService _jwtService;
        private UserClaimsService _userClaimService;

        public LoginManager()
        {
            _cryptoService = new CryptoService();
            _userService = new UserService();
            _jwtService = new JWTService();
            _userClaimService = new UserClaimsService();
        }

        public string Login(SSOUserRequest request)
        {
            // TODO: Make the concatenation more extensible
            // foreach property in request

            var message = "ssoUserId=" + request.ssoUserId + ";" +
                             "email=" + request.email + ";" +
                             "timestamp=" + request.timestamp + ";";
            var hashedMessage = _cryptoService.HashHMAC(message);
            // Check if signature is valid
            if (hashedMessage == request.signature)
            {
                // Check if user exists
                if (_userService.IsUsernameFound(request.email))
                {
                    return "https://greetngroup.com/login/" + _jwtService.CreateToken(request.email, _userService.GetUserUid(request.email));
                }
                else
                {
                    User createdUser = new User(  // If user doesn't exist, create a placeholder user that is not activated
                        _userService.GetNextUserID(), // UserID
                        null, // First name
                        null, // Last name
                        request.email, // Username
                        null, // City
                        null, // State
                        null, // Country
                        DateTime.Now, // Minimum datetime for DOB
                        false // IsActivated
                        );
                    _userService.InsertUser(createdUser); // Check for user acivation on home page
                    _userClaimService.AddDefaultClaims(createdUser);
                    return _jwtService.CreateToken(request.email, createdUser.UserId);
                }
            }
            return "-1";
        }
    }
}
