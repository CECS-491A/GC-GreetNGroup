using ManagerLayer.ProfileManagement;
using ServiceLayer.Requests;
using ServiceLayer.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    // Necessary for reading the token
    // Odd bug where token was null if not using this request class
    // TODO: find alternative solution
    public class GetEmailRequest
    {
        public string token { get; set; }
    }

    public class UserController : ApiController
    {
        UserService userService = new UserService();
        [HttpGet]
        [Route("api/user/{userID}")]
        public HttpResponseMessage Get(string userID)
        {
            try
            {
                ProfileManager profileMan = new ProfileManager();
                var result = profileMan.GetUser(userID);
                return result;
            }
            catch (Exception)
            {
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Unable to retrieve user data, user not found")
                };
                return httpResponseFail;
            }
        }

        [HttpPost]
        [Route("api/user/email/getemail")]
        public HttpResponseMessage GetEmail([FromBody]GetEmailRequest request)
        {
            try
            {
                ProfileManager profileMan = new ProfileManager();
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
        [Route("api/user/update/getuser")]
        public HttpResponseMessage GetUserToUpdate(string jwtToken)
        {
            try
            {
                ProfileManager profileMan = new ProfileManager();
                var response = profileMan.GetUserToUpdate(jwtToken);
                return response;
            }
            catch
            {
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Unable to retrieve user")
                };
                return httpResponseFail;
            }
        }

        [HttpPost]
        [Route("api/user/update")]
        public HttpResponseMessage Update([FromBody] UpdateProfileRequest request)
        {
            try
            {
                ProfileManager profileMan = new ProfileManager();
                var response = profileMan.UpdateUserProfile(request);
                return response;
            }
            catch
            {
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Unable to update user")
                };
                return httpResponseFail;
            }
        }

        /*
        [HttpPost]
        [Route("api/user/{userID}/rate")]
        public HttpResponseMessage Rate(string userID, [FromBody]RateRequest request)
        {
            //
            return
        }
        */

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
    }
}
