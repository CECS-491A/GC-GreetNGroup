using ManagerLayer.LoginManagement;
using ManagerLayer.ProfileManagement;
using ServiceLayer.Requests;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("api/user/{userID}")]
        public IHttpActionResult Get(string userID)
        {
            try
            {
                ProfileManager pm = new ProfileManager();
                int result = pm.GetUserController(userID);
                if (result == 1)
                {
                    return Content(HttpStatusCode.OK, pm.GetUserController(userID));
                }
                else if (result == -1)
                {
                    return Content(HttpStatusCode.NotFound, "User does not exist");
                }
                else if (result == -2)
                {
                    return Content(HttpStatusCode.ServiceUnavailable, "Unable to search for user");
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Service Unavailable");
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }

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
            catch (Exception) //Catch all errors
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }

        [HttpGet]
        [Route("api/user/update/getuser")]
        public IHttpActionResult GetUserToUpdate(string jwtToken)
        {
            ProfileManager pm = new ProfileManager();
            int response = pm.GetUserToUpdateController(jwtToken);
            if(response == 1)
            {
                return Content(HttpStatusCode.OK, pm.GetUserToUpdate(jwtToken));
            }else if(response == -1)
            {
                return Content(HttpStatusCode.Unauthorized, "Session is invalid");
            }
            else if(response == -2)
            {
                return Content(HttpStatusCode.BadRequest, "Unable to update user");
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }

        [HttpPost]
        [Route("api/user/update")]
        public IHttpActionResult Update([FromBody] UpdateProfileRequest request)
        {
            ProfileManager pm = new ProfileManager();
            int response = pm.UpdateUserProfile(request);
            if (response == 1)
            {
                return Content(HttpStatusCode.OK, "Profile has been updated");
            }
            else if (response == -1)
            {
                return Content(HttpStatusCode.Unauthorized, "Session is invalid");
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }
    }
}
