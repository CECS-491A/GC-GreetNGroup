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
        public class IsProfileActivatedRequest
        {
            public string token { get; set; }
        }
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


        [HttpPost]
        [Route("api/profile/isprofileactivated")]
        public HttpResponseMessage IsProfileActivated([FromBody] IsProfileActivatedRequest request)
        {
            var profileMan = new UserProfileManager();
            var isProfileActivated = profileMan.IsProfileActivated(request.token);
            return isProfileActivated;
        }
    }
}
