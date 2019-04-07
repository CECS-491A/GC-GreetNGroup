using System;
using System.Text;
using ServiceLayer.Requests;
using ServiceLayer.Services;
using ManagerLayer.JWTManagement;
using DataAccessLayer.Tables;
using ServiceLayer.Interface;

namespace ManagerLayer.LoginManagement
{
    public class LoginManager
    {
        private readonly string sharedSecretKey = Environment.GetEnvironmentVariable("sharedSecretKey", EnvironmentVariableTarget.User);

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
            //Check if signature is valid
            if (hashedMessage == request.signature)
            {
                //Check if user exists
                if (_userService.IsUsernameFound(request.email))
                {
                    return _JWTManager.GrantToken(request.email).ToString();
                }
                else
                {
                    User createdUser = new User(  //If user doesn't exist, create a placeholder user that is not activated
                        _userService.GetNextUserID(), //UserID
                        null, //First name
                        null, //Last name
                        request.email, //Username
                        null, //City
                        null, //State
                        null, //Country
                        DateTime.MinValue, //Minimum datetime for DOB
                        false //IsActivated
                        );
                    _userService.InsertUser(createdUser); //Check for user acivation on home page
                    return _JWTManager.GrantToken(request.email).ToString();
                }
            }
            return "-1";
        }
    }
}
