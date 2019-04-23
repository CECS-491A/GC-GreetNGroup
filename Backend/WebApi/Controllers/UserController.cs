using ManagerLayer.ProfileManagement;
using ServiceLayer.Requests;
using ServiceLayer.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class UserController : ApiController
    {
        UserService userService = new UserService();
        [HttpGet]
        [Route("api/user/{userID}")]
        public HttpResponseMessage Get(string userID)
        {
            try
            {
                ProfileManager pm = new ProfileManager();
                var result = pm.GetUser(userID);
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

        [HttpGet]
        [Route("api/user/update/getuser")]
        public HttpResponseMessage GetUserToUpdate(string jwtToken)
        {
            ProfileManager pm = new ProfileManager();
            var response = pm.GetUserToUpdate(jwtToken);
            return response;
        }

        [HttpPost]
        [Route("api/user/update")]
        public HttpResponseMessage Update([FromBody] UpdateProfileRequest request)
        {
            ProfileManager pm = new ProfileManager();
            var response = pm.UpdateUserProfile(request);
            return response;
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
