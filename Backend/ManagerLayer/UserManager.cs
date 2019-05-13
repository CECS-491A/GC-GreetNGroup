using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using ServiceLayer.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ManagerLayer.UserManagement
{
    public class UserManager
    {
        private IUserService _userService;
        private ILoggerService _gngLoggerService;
        private SignatureService _signatureService;
        private JWTService _jwtService;

        public UserManager()
        {
            _userService = new UserService();
            _gngLoggerService = new LoggerService();
            _signatureService = new SignatureService("8934DC8043EE545D7759F2089267A5EDF1B424DC5E100A85E85B65E5C5C9E72C");
            _jwtService = new JWTService();
        }

        public bool DoesUserExists(int userID)
        {
            return _userService.IsUsernameFoundById(userID);
        }

        public HttpResponseMessage GetEmail(string jwtToken)
        {
            try
            {
                var isSignatureTampered = _jwtService.IsJWTSignatureTampered(jwtToken);
                if (isSignatureTampered)
                {
                    var httpResponseFail = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("Session is invalid")
                    };
                    return httpResponseFail;
                }
                var retrievedEmail = _jwtService.GetUsernameFromToken(jwtToken);

                var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(retrievedEmail)
                };
                return httpResponse;
            }
            catch (Exception e)
            {
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to get email at this time.")
                };
                return httpResponseFail;
            }
        }

        public HttpResponseMessage DeleteUserUsingSSO(SSOUserRequest request)
        {
            try
            {
                var isSignatureValid = _signatureService.IsValidClientRequest(request.ssoUserId, request.email, request.timestamp, request.signature);
                if (!isSignatureValid)
                {
                    var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Invalid Session")
                    };
                    return httpResponse;
                }

                var response = DeleteUser(request.email);

                return response;
            }
            catch
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to delete user at this time")
                };
                return httpResponse;
            }
        }

        public HttpResponseMessage DeleteUser(string email)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var retrievedUser = ctx.Users.Where(u => u.UserName == email).FirstOrDefault<User>();

                    if (retrievedUser == null)
                    {
                        var httpResponseFail = new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("User not found in system")
                        };
                        return httpResponseFail;
                    }

                    ctx.JWTTokens.RemoveRange(ctx.JWTTokens.Where(j => j.UserId == retrievedUser.UserId));
                    ctx.UserRatings.RemoveRange(ctx.UserRatings.Where(u => u.RatedId1 == retrievedUser.UserId));
                    ctx.UserRatings.RemoveRange(ctx.UserRatings.Where(u => u.RaterId1 == retrievedUser.UserId));
                    var retrievedClaims = ctx.UserClaims.Where(c => c.UId == retrievedUser.UserId).FirstOrDefault<UserClaim>();
                    ctx.UserClaims.Remove(retrievedClaims);

                    retrievedUser.FirstName = "Deleted";
                    retrievedUser.LastName = "User";
                    retrievedUser.UserName = "DeletedUser@greetngroup.com";
                    retrievedUser.State = "CA";
                    retrievedUser.City = "LB";
                    retrievedUser.Country = "USA";
                    retrievedUser.DoB = DateTime.MinValue;
                    retrievedUser.IsActivated = false;
                    ctx.SaveChanges();

                    var httpResponseSuccess = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("User was deleted from GreetNGroup")
                    };
                    return httpResponseSuccess;
                }
            }
            catch (Exception ex)
            {
                _gngLoggerService.LogGNGInternalErrors(ex.ToString());
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to delete user at this time")
                };
                return httpResponse;
            }
        }
    }
}
