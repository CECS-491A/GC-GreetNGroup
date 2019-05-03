using System;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;
using ServiceLayer.Services;
using Gucci.DataAccessLayer.Context;
using DataAccessLayer.Tables;
using System.Linq;

namespace Gucci.ManagerLayer
{
    public class SessionManager
    {
        private SignatureService _signatureService;
        private IUserService _userService;
        private IJWTService _jwtService;
        private UserClaimsService _userClaimService;

        public SessionManager()
        {
            _signatureService = new SignatureService();
            _userService = new UserService();
            _jwtService = new JWTService();
            _userClaimService = new UserClaimsService();
        }

        public string Login(SSOUserRequest request)
        {
            try
            {
                // Check if signature is valid
                if (_signatureService.IsValidClientRequest(request.ssoUserId, request.email, request.timestamp, request.signature))
                {
                    return "-1";
                }
                // Check if user exists
                if (_userService.IsUsernameFound(request.email))
                {
                    return _jwtService.CreateToken(request.email, _userService.GetUserUid(request.email));
                }
                else
                {
                    User createdUser = new User
                    {
                        UserId = _userService.GetNextUserID()
                    };
                    _userService.InsertUser(createdUser);
                    _userClaimService.AddDefaultClaims(createdUser);
                    return _jwtService.CreateToken(request.email, createdUser.UserId);
                }
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        public bool Logout(SSOUserRequest request)
        {
            try
            {
                if (!_signatureService.IsValidClientRequest(request.ssoUserId, request.email, request.timestamp, request.signature))
                {
                    return false;
                }
                if (!_userService.IsUsernameFound(request.email))
                {
                    return false;
                }

                using(var ctx = new GreetNGroupContext())
                {
                    var JWTTokenToRemove = ctx.JWTTokens.Where(j => j.UserName == request.email).FirstOrDefault<JWTToken>();
                    if(JWTTokenToRemove != null)
                    {
                        _jwtService.DeleteTokenFromDB(JWTTokenToRemove.Token);
                        ctx.SaveChanges();
                        return true;
                    }
                    return false;  
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
