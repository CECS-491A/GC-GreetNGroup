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
        private string baseRedirectURL = "https://greetngroup.com/login?token=";

        public SessionManager()
        {
            _signatureService = new SignatureService();
            _userService = new UserService();
            _jwtService = new JWTService();
            _userClaimService = new UserClaimsService();
        }

        public HttpResponseMessage Login(ApiController controller, SSOUserRequest request)
        {
            try
            {
                // Check if signature is valid
                var response = _signatureService.IsValidClientRequest(request.ssoUserId, request.email, request.timestamp, request.signature);
                if (!response)
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
                    User createdUser = new User
                    {
                        UserId = _userService.GetNextUserID()
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
                    Content = new StringContent(e.ToString())
                };
                return httpResponse;
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
