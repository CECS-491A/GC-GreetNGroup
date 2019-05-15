using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gucci.ManagerLayer.ProfileManagement;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using Gucci.ServiceLayer.Interface;
using ManagerLayer.UserManagement;

namespace WebApi.Controllers
{
    public class ProfileController : ApiController
    {
        private JWTService _jwtService = new JWTService();
        private UserManager userMan = new UserManager();
        UserProfileManager pm = new UserProfileManager();
        private ILoggerService _gngLoggerService = new LoggerService();

        // Method to get the user profile given their ID
        [HttpGet]
        [Route("api/profile/getprofile/{userID}")]
        public HttpResponseMessage Get(string userID)
        {
            UserProfileManager profileMan = new UserProfileManager();
            var retrievedUser = profileMan.GetUser(userID);
            return retrievedUser;
        }

        // Method to update the user
        [HttpPost]
        [Route("api/profile/update")]
        public HttpResponseMessage Update([FromBody] UpdateProfileRequest request)
        {
            UserProfileManager profileMan = new UserProfileManager();
            var response = profileMan.UpdateUserProfile(request);
            return response;
        }

        // Method to check if the user profile is activated
        [HttpPost]
        [Route("api/profile/isprofileactivated")]
        public HttpResponseMessage IsProfileActivated([FromBody] TokenRequest request)
        {
            var profileMan = new UserProfileManager();
            var isProfileActivated = profileMan.IsProfileActivated(request.token);
            return isProfileActivated;
        }

        [HttpGet]
        [Route("api/getuserid")]
        public IHttpActionResult GetCurrentUserID([FromUri]string jwt)
        {
            try
            {
                // Retrieves info for GET
                var userID = _jwtService.GetUserIDFromToken(jwt);
                return Ok(userID);
            }
            catch (Exception e)
            {
                // _gngLoggerService.LogBadRequest(userID.ToString(), "N/A", "https://www.greetngroup.com/user/" + userID, e.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/profile/{userID}/rate")]
        public IHttpActionResult Rate(string userID, [FromBody]RateRequest request)
        {
            try
            {
                var isTokenValid = _jwtService.IsTokenValid(request.jwtToken);
                if (!isTokenValid)
                {
                    return Content(HttpStatusCode.BadRequest, "Unvalid Token");
                }
                var result = pm.RateUser(request, userID);
                if (result == 1)
                {
                    return Content(HttpStatusCode.OK, "Rating successful");
                }
                else if (result == 2)
                {
                    return Content(HttpStatusCode.OK, "Updated Rating");
                }
                else if (result == -1)
                {
                    return Content(HttpStatusCode.BadRequest, "Raing Failed");
                }
                else if (result == -2)
                {
                    return Content(HttpStatusCode.BadRequest, "Not Authorized Rate Users");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Something went wrong");
                }
            }
            catch (Exception e) //Catch all errors
            {
                _gngLoggerService.LogBadRequest(userID, "N/A", "https://www.greetngroup.com/user/" + userID + "/rate", e.ToString());
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }
    }
}
