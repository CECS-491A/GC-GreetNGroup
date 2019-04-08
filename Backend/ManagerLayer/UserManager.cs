using DataAccessLayer.Tables;
using ServiceLayer.Interface;
using ServiceLayer.Requests;
using ServiceLayer.Services;
using System;

namespace ManagerLayer.UserManagement
{
    public class UserManager
    {
        private IUserService _userService;
        private ICryptoService _cryptoService;

        public UserManager()
        {
            _userService = new UserService();
            _cryptoService = new CryptoService();
        }

        public bool DeleteUserSSO(SSOUserRequest request)
        {
            string message = request.ssoUserId + ";" +
                             request.email + ";" +
                             request.timestamp + ";";
            var hashedMessage = _cryptoService.HashHMAC(message);
            //Check if signature is valid
            if (hashedMessage == request.signature)
            {
                //Check if user exists
                if (_userService.IsUsernameFound(request.email))
                {
                    _userService.DeleteUser(_userService.GetUserByUsername(request.email));
                    return true;
                }
                return false;
            }
            return false;     
        }
    }
}
