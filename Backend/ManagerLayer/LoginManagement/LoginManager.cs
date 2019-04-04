using System;
using System.Text;
using ServiceLayer.Requests;
using ServiceLayer.Services;
using ManagerLayer.UserManagement;

namespace ManagerLayer.LoginManagement
{
    public class LoginManager
    {
        private string sharedSecretKey = Environment.GetEnvironmentVariable("sharedSecretKey", EnvironmentVariableTarget.User);
        private ICryptoService _cryptoService;
        private IUserService _userService;
        private IJWTService _JWTService;
        public LoginManager()
        {
            _cryptoService = new CryptoService();
            _JWTService = new JWTService();
            _userService = new UserService();
        }


        public int Login(LoginRequest request)
        {
            //TODO: Make the concatenation more extensible
            //foreach property in request

            string message = request.ssoUserId + ";" +
                             request.email + ";" +
                             request.timestamp + ";";


            var hashedMessage = _cryptoService.HashHMAC(Encoding.ASCII.GetBytes(sharedSecretKey), message);
            if(hashedMessage == request.signature)
            {
                //Check if user exists
                if (_userService.IsExistingGNGUser(request.email))
                {

                }
                else
                {
                    //If user doesn't exist, create a placeholder user that is not activated
                    //Check for user acivation on home page
                    //_userManager.InsertUser();
                }
            }
            return -1;
        }
    }
}
