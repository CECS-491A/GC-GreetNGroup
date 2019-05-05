using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using ServiceLayer.Services;
using System;
using System.Net;
using System.Net.Http;

namespace ManagerLayer.UserManagement
{
    public class UserManager
    {
        private IUserService _userService;
        private SignatureService _signatureService;
        private JWTService _jwtService;

        public UserManager()
        {
            _userService = new UserService();
            _signatureService = new SignatureService();
            _jwtService = new JWTService();
        }

        public bool DoesUserExists(int userID)
        {
            return _userService.IsUsernameFoundById(userID);
        }

        public HttpResponseMessage GetEmail(string jwtToken)
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
                // Check if user exists
                var isExistingUser = _userService.IsUsernameFound(email);
                if (!isExistingUser)
                {
                    var httpResponseFail = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("User not found in system")
                    };
                    return httpResponseFail;
                }

                var isUserDeleted = _userService.DeleteUser(_userService.GetUserByUsername(email));
                if (isUserDeleted)
                {
                    var httpResponseSuccess = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("User was deleted from GreetNGroup")
                    };
                    return httpResponseSuccess;
                }

                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to delete user at this time")
                };
                return httpResponse;
            }
            catch (Exception)
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to delete user at this time")
                };
                return httpResponse;
            }
        }
    }
}
