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
        [Route("api/profile/isprofileactivated/{jwtToken}")]
        public HttpResponseMessage IsProfileActivated([FromBody] string jwtToken)
        {
            var profileMan = new UserProfileManager();
            var isProfileActivated = profileMan.IsProfileActivated(jwtToken);
            return isProfileActivated;
        }
    }
}
