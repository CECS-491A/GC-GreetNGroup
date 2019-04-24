using Gucci.ManagerLayer.ProfileManagement;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gucci.ManagerLayer.LogManagement;

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
        private LogManager gngLogManager = new LogManager();

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
                */
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
