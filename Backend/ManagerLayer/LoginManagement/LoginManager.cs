using System;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;

namespace Gucci.ManagerLayer.LoginManagement
{
    public class LoginManager
    {
        private readonly string AppLaunchSecretKey;
        private ICryptoService _cryptoService;
        private IUserService _userService;
        private IJWTService _jwtService;
        private UserClaimsService _userClaimService;

        public LoginManager()
        {
            // "D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE" For testing
            AppLaunchSecretKey = "D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE";
            //Environment.GetEnvironmentVariable("AppLaunchSecretKey", EnvironmentVariableTarget.User);
            _cryptoService = new CryptoService(AppLaunchSecretKey);
            _userService = new UserService();
            _jwtService = new JWTService();
            _userClaimService = new UserClaimsService();
        }

        public LoginManager(string SSOSecretKey)
        {
            AppLaunchSecretKey = SSOSecretKey;
            _cryptoService = new CryptoService(AppLaunchSecretKey);
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
                    User createdUser = new User();
                    createdUser.UserId = _userService.GetNextUserID();
                    _userService.InsertUser(createdUser); // Check for user acivation on home page
                    _userClaimService.AddDefaultClaims(createdUser);
                    return _jwtService.CreateToken(request.email, createdUser.UserId);
                }
            }
            return "-1";
        }
    }
}
