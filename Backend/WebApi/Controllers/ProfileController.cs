using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gucci.ManagerLayer.ProfileManagement;
using Gucci.ServiceLayer.Requests;

namespace WebApi.Controllers
{
    public class ProfileController : ApiController
    {

        // Method to get the user profile given their ID
        [HttpGet]
        [Route("api/profile/{userID}")]
        public HttpResponseMessage Get(string userID)
        {
            try
            {
                UserProfileManager profileMan = new UserProfileManager();
                var result = profileMan.GetUser(userID);
                return result;
            }
            catch (Exception e)
            {
                //gngLogManager.LogBadRequest("", "", "", e.ToString());
                //return Content(HttpStatusCode.BadRequest, "Service Unavailable");
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Service Unavailable")
                };
                return httpResponseFail;
            }
        }

        // Method to update the user
        [HttpPost]
        [Route("api/profile/update")]
        public HttpResponseMessage Update([FromBody] UpdateProfileRequest request)
        {
            try
            {
                UserProfileManager profileMan = new UserProfileManager();
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


        [HttpGet]
        [Route("api/profile/isprofileactivated")]
        public HttpResponseMessage IsProfileActivated([FromUri] string jwtToken)
        {
            try
            {
                var profileMan = new UserProfileManager();
                var response = profileMan.IsProfileActivated(jwtToken);
                return response;
            }
            catch
            {
                var httpResponse = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent("Unable to check if user is activated at this time.")
                };
                return httpResponse;
            }
        }
    }
}
