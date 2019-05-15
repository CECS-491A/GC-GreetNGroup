using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using Gucci.ServiceLayer.Interface;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ManagerLayer.UserManagement;
using Gucci.ManagerLayer;

namespace WebApi.Controllers
{

    public class UserController : ApiController
    {
        private ILoggerService _gngLoggerService = new LoggerService();
        private UserService userService = new UserService();
        private IJWTService _jwtService = new JWTService();

        // Winn 
        // Method to get the email of a user given the JWTToken
        // HTTPPost Necessary for reading the token
        // Odd bug where token was null if not using this request class
        [HttpPost]
        [Route("api/user/getemail")]
        public HttpResponseMessage GetEmail([FromBody] JWTTokenRequest request)
        {
            try
            {
                UserManager userMan = new UserManager();
                var result = userMan.GetEmail(request.token);
                return result;
            }
            catch (Exception e)
            {
                _gngLoggerService.LogBadRequest(_jwtService.GetUserIDFromToken(request.token).ToString(), "N/A", "https://www.greetngroup.com/user/", e.ToString());
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unable to retrieve email")
                };
                return httpResponseFail;
            }
        }
        // Winn
        [HttpPost]
        [Route("api/user/deleteuser")]
        public HttpResponseMessage DeleteUser([FromBody] TokenRequest request)
        {
            JWTService _jwtService = new JWTService();
            var isTokenValid = _jwtService.IsTokenValid(request.token);
            if (!isTokenValid)
            {
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Unable to delete user, token is invalid")
                };
                return httpResponseFail;
            }
            var retrievedEmailFromToken = _jwtService.GetUsernameFromToken(request.token);

            UserManager userMan = new UserManager();
            var response = userMan.DeleteUser(retrievedEmailFromToken);
            return response;
        }

        [HttpGet]
        [Route("api/user/username/{userID}")]
        public IHttpActionResult GetUserByID(int userID)
        {
            try
            {
                // Retrieves info for GET
                var user = userService.GetUserById(userID);
                var fullName = user.FirstName + " " + user.LastName;
                return Ok(fullName);
            }
            catch (HttpRequestException e)
            {
                _gngLoggerService.LogBadRequest(userID.ToString(), "N/A", "https://www.greetngroup.com/user/" + userID, e.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/user/logout")]
        public HttpResponseMessage Logout([FromBody] TokenRequest request)
        {
            var sessionMan = new SessionManager();
            var response = sessionMan.LogoutUsingGreetNGroup(request.token);
            return response;
        }

        /*
        [HttpPost]
        [Route("api/user/{userID}/rate")]
        public IHttpActionResult Rate(string userID, [FromBody]RateRequest request)
        {
            try
            {
                ProfileManager pm = new ProfileManager();
                int result = pm.RateUser(request, userID);
                if (result == 1)
                {
                    return Content(HttpStatusCode.OK, "Rating successful");
                }
                else if (result == -1)
                {
                    return Content(HttpStatusCode.BadRequest, "Service Unavailable");
                }
                else if (result == -2)
                {
                    return Content(HttpStatusCode.BadRequest, "Cannot rate user agian");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Service Unavailable");
                }
            }
            catch (Exception e) //Catch all errors
            {
                _gngLoggerService.LogBadRequest(userID, "N/A", "https://www.greetngroup.com/user/" + userID + "/rate", e.ToString());
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
                /*
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Unable to retrieve user data, user not found")
                };
                return httpResponseFail;
                
            }
        }
        */

        /*
        [HttpPost]
        [Route("api/user/{userID}/rate")]
        public HttpResponseMessage Rate(string userID, [FromBody]RateRequest request)
        {
            //
            return
        }
        */
    }
}
