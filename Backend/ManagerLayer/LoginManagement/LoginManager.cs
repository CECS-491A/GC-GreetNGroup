using System;
using System.Text;
using ServiceLayer.Requests;
using ServiceLayer.Services;
using ManagerLayer.UserManagement;
using ManagerLayer.JWTManagement;
using DataAccessLayer.Tables;

namespace ManagerLayer.LoginManagement
{
    public class LoginManager
    {
        private string sharedSecretKey = Environment.GetEnvironmentVariable("sharedSecretKey", EnvironmentVariableTarget.User);

        private ICryptoService _cryptoService;
        private IUserService _userService;
        private JWTManager _JWTManager;

        public LoginManager()
        {
            _cryptoService = new CryptoService();
            _JWTManager = new JWTManager();
            _userService = new UserService();
        }

        public string Login(LoginRequest request)
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
                    return _JWTManager.GrantToken(request.email).ToString();
                }
                else
                {
                    //If user doesn't exist, create a placeholder user that is not activated
                    //Check for user acivation on home page

                    User createdUser = new User(
                        _userService.GetNextUserID(), //UserID
                        null, //First name
                        null, //Last name
                        request.email, //Username
                        null, //Password
                        null, //City
                        null, //State
                        null, //Country
                        DateTime.MinValue, //Minimum datetime for DOB
                        null, //SecQ
                        null, //SecA
                        false //IsActivated
                        );
                    _userService.CreateUser(createdUser);
                    return _JWTManager.GrantToken(request.email).ToString();
                }
            }
            return "-1";
        }
    }
}
