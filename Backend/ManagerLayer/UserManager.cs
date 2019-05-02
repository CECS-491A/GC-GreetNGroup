using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using System;

namespace ManagerLayer.UserManagement
{
    public class UserManager
    {
        private readonly string AppLaunchSecretKey;
        private IUserService _userService;
        private ICryptoService _cryptoService;
        private ILoggerService _gngLoggerService;

        public UserManager()
        {
            AppLaunchSecretKey = Environment.GetEnvironmentVariable("AppLaunchSecretKey", EnvironmentVariableTarget.User);
            _userService = new UserService();
            _cryptoService = new CryptoService(AppLaunchSecretKey);
            _gngLoggerService = new LoggerService();
        }

        public UserManager(string SSOLaunchKey)
        {
            AppLaunchSecretKey = SSOLaunchKey;
            _userService = new UserService();
            _cryptoService = new CryptoService(AppLaunchSecretKey);
        }

        public bool DeleteUserSSO(SSOUserRequest request)
        {
            string message = request.ssoUserId + ";" +
                             request.email + ";" +
                             request.timestamp + ";";
            var hashedMessage = _cryptoService.HashHMAC(message); // Hash the request
            // Check if signature is valid
            if (hashedMessage == request.signature)
            {
                return false;
            }

            // Check if user exists
            if (!_userService.IsUsernameFound(request.email))
            {
                return false;
            }

            try
            {
                return _userService.DeleteUser(_userService.GetUserByUsername(request.email));
            }
            catch (Exception ex)
            {
                _gngLoggerService.LogGNGInternalErrors(ex.ToString());
                return false;
            }
        }
    }
}
