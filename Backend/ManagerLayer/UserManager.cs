using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using ServiceLayer.Services;
using System;

namespace ManagerLayer.UserManagement
{
    public class UserManager
    {
        private IUserService _userService;
        private SignatureService _signatureService;

        public UserManager()
        {
            _userService = new UserService();
            _signatureService = new SignatureService();
        }

        public bool DoesUserExists(int userID)
        {
            return _userService.IsUsernameFoundById(userID);
        }

        public bool DeleteUserSSO(SSOUserRequest request)
        {
            try
            {
                // Check if signature is valid
                if (!_signatureService.IsValidClientRequest(request.ssoUserId, request.email, request.timestamp, request.signature))
                {
                    return false;
                }

                // Check if user exists
                if (!_userService.IsUsernameFound(request.email))
                {
                    return true;
                }
                return _userService.DeleteUser(_userService.GetUserByUsername(request.email));
            }
            catch
            {
                return false;
            }
        }
    }
}
