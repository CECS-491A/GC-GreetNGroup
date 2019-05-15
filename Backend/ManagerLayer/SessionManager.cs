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
            _signatureService = new SignatureService(Environment.GetEnvironmentVariable("AppLaunchSecretKey", EnvironmentVariableTarget.Machine));
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

        public HttpResponseMessage Logout(string email)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var userToLogout = ctx.Users.Where(u => u.UserName == email).FirstOrDefault<User>();
                var JWTTokenToInvalidate = ctx.JWTTokens.Where(j => j.UserId == userToLogout.UserId)
                                                        .OrderByDescending(p => p.Id).First();
                if (JWTTokenToInvalidate != null)
                {
                    var isTokenInvalidated = _jwtService.InvalidateToken(JWTTokenToInvalidate.Token);
                    if (isTokenInvalidated)
                    {
                        var httpResponseSuccess = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent("User has logged out of GreetNGroup")
                        };
                        return httpResponseSuccess;
                    }
                    var httpResponseFail = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Unable to delete token")
                    };
                    return httpResponseFail;
                }
                var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("User Does Not Exist")
                };
                return httpResponse;
            }
        }

        public HttpResponseMessage LogoutUsingSSO(SSOUserRequest request)
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

                return Logout(request.email);
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

        public HttpResponseMessage LogoutUsingGreetNGroup(string jwtToken)
        {
            try
            {
                var email = _jwtService.GetUsernameFromToken(jwtToken);
                return Logout(email);
            }
            catch(Exception e)
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.ToString())
                };
                return httpResponse;
            }
        }
    }
}
