using Gucci.ManagerLayer.ProfileManagement;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gucci.ManagerLayer.LogManagement;
using ManagerLayer.UserManagement;

namespace WebApi.Controllers
{
    // Necessary for reading the token
    // Odd bug where token was null if not using this request class
    public class GetEmailRequest
    {
        public string token { get; set; }
    }

    public class UserController : ApiController
    {
        private LogManager gngLogManager = new LogManager();
        UserService userService = new UserService();

        // Method to get the email of a user given the JWTToken
        [HttpPost]
        [Route("api/user/getemail")]
        public HttpResponseMessage GetEmail([FromBody]GetEmailRequest request)
        {
            try
            {
                UserProfileManager profileMan = new UserProfileManager();
                var result = profileMan.GetEmail(request.token);
                return result;
            }
            catch (Exception)
            {
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Unable to retrieve email")
                };
                return httpResponseFail;
            }
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
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/user/logout")]
        public IHttpActionResult Logout([FromBody] TokenRequest request)
        {
            try
            {
                JWTService _jwtService = new JWTService();
                var response = _jwtService.DeleteTokenFromDB(request.token);
                if (response)
                {
                    return Ok("Successfully logged out");
                }
                return InternalServerError();
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
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
                //gngLogManager.LogBadRequest("", "", "", e.ToString());
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
