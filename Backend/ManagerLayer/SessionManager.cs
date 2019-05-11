using System;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;
using ServiceLayer.Services;
using Gucci.DataAccessLayer.Context;
using DataAccessLayer.Tables;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;

namespace Gucci.ManagerLayer
{
    public class SessionManager
    {
        private SignatureService _signatureService;
        private IUserService _userService;
        private IJWTService _jwtService;
        private UserClaimsService _userClaimService;
        private readonly string baseRedirectURL = "https://greetngroup.com/login?token=";

        public SessionManager()
        {
            //Environment.GetEnvironmentVariable("AppLaunchSecretKey", EnvironmentVariableTarget.Machine)
            //8934DC8043EE545D7759F2089267A5EDF1B424DC5E100A85E85B65E5C5C9E72C
            _signatureService = new SignatureService("5E5DDBD9B984E4C95BBFF621DF91ABC9A5318DAEC0A3B231B4C1BC8FE0851610");
            _userService = new UserService();
            _jwtService = new JWTService();
            _userClaimService = new UserClaimsService();
        }

        public SessionManager(string secretKey)
        {
            _signatureService = new SignatureService(secretKey);
            _userService = new UserService();
            _jwtService = new JWTService();
            _userClaimService = new UserClaimsService();
        }

        public HttpResponseMessage Login(ApiController controller, SSOUserRequest request)
        {
            try
            {
                // Check if signature is valid
                var isSignatureValid = _signatureService.IsValidClientRequest(request.ssoUserId, request.email, request.timestamp, request.signature);
                if (!isSignatureValid)
                {
                    var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Invalid Session")
                    };
                    return httpResponse;
                }
                // Check if user exists
                if (_userService.IsUsernameFound(request.email))
                {
                    var generatedToken = _jwtService.CreateToken(request.email, _userService.GetUserUid(request.email));
                    var redirectURL = baseRedirectURL + generatedToken;
                    var redirect = controller.Request.CreateResponse(HttpStatusCode.SeeOther);
                    redirect.Content = new StringContent(redirectURL);
                    redirect.Headers.Location = new Uri(redirectURL);

                    return redirect;
                }
                else
                {
                    // If user doesn't have account in greetngroup, create account
                    User createdUser = new User
                    {
                        UserId = _userService.GetNextUserID(),
                        UserName = request.email
                    };
                    _userService.InsertUser(createdUser);
                    _userClaimService.AddDefaultClaims(createdUser);
                    var generatedToken = _jwtService.CreateToken(request.email, _userService.GetUserUid(request.email));
                    var redirectURL = baseRedirectURL + generatedToken;
                    var redirect = controller.Request.CreateResponse(HttpStatusCode.SeeOther);
                    redirect.Content = new StringContent(redirectURL);
                    redirect.Headers.Location = new Uri(redirectURL);

                    return redirect;
                }
            }
            catch (Exception e)
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to login at this time")
                };
                return httpResponse;
            }
        }

        public HttpResponseMessage Logout(SSOUserRequest request)
        {
            try
            {
                // Check if signature is valid
                var isSignatureValid = _signatureService.IsValidClientRequest(request.ssoUserId, request.email, request.timestamp, request.signature);
                if (!isSignatureValid)
                {
                    var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Invalid Session")
                    };
                    return httpResponse;
                }
                if (!_userService.IsUsernameFound(request.email))
                {
                    var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("User Does Not Exist")
                    };
                    return httpResponse;
                }

                using(var ctx = new GreetNGroupContext())
                {
                    var userToLogout = ctx.Users.Where(u => u.UserName == request.email).FirstOrDefault<User>();
                    var JWTTokenToRemove = ctx.JWTTokens.Where(j => j.UserId == userToLogout.UserId).FirstOrDefault<JWTToken>();
                    if(JWTTokenToRemove != null)
                    {
                        _jwtService.DeleteTokenFromDB(JWTTokenToRemove.Token);
                        ctx.SaveChanges();
                        var httpResponseSuccess = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent("User has logged out of GreetNGroup")
                        };
                        return httpResponseSuccess;
                    }
                    var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Unable to log out at this time")
                    };
                    return httpResponse;
                }
            }
            catch (Exception)
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to log out at this time")
                };
                return httpResponse;
            }
        }
    }
}
