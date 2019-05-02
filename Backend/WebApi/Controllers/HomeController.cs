using Gucci.ManagerLayer.LoginManagement;
using Gucci.ManagerLayer.ProfileManagement;
using Gucci.ServiceLayer.Requests;
using System;
using System.Net;
using System.Web.Http;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Services;

namespace WebApi.Controllers
{
    public class HomeController : ApiController
    {
        private ILoggerService _gngLoggerService = new LoggerService();
        private UserService userService = new UserService();
        private IJWTService jwtService = new JWTService();

        [HttpGet]
        [Route("api/profile")]
        public IHttpActionResult Get([FromBody]string jwtToken)
        {
            try
            {
                ProfileManager pm = new ProfileManager();
                //TODO update in sprint 5 to get user profile information
                if (pm.CheckProfileActivated(jwtToken))
                {
                    return Content(HttpStatusCode.OK, true);
                }
                return Content(HttpStatusCode.Redirect, "User profile needs to be completed");
            }
            catch (Exception ex)
            {
                //TODO: Update so that ip & url is included in FromBody for logging purposes
                _gngLoggerService.LogBadRequest(jwtService.GetUserIDFromToken(jwtToken).ToString(), 
                    "N/A", "https://www.greetngroup.com/", ex.ToString());
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }

        [HttpPost]
        [Route("api/logclicks")]
        public IHttpActionResult TrackUserActivity([FromBody] LogActivityRequest request)
        {
            try
            {
                if(request.Jwt == null)
                {
                    userService.LogClicksMade(request.StartPoint, request.EndPoint,
                    "N/A", request.Ip);

                    return Content(HttpStatusCode.Accepted, true);
                }
                else
                {
                    userService.LogClicksMade(request.StartPoint, request.EndPoint,
                    jwtService.GetUserIDFromToken(request.Jwt).ToString(), request.Ip);

                    return Content(HttpStatusCode.Accepted, true);
                }
                
            }
            catch(Exception e)
            {
                if(request.Jwt == null)
                {
                    _gngLoggerService.LogBadRequest("N/A", request.Ip, request.StartPoint, e.ToString());
                }
                else
                {
                    _gngLoggerService.LogBadRequest(jwtService.GetUserIDFromToken(request.Jwt).ToString(), request.Ip, request.StartPoint, e.ToString());
                }
                return Content(HttpStatusCode.BadRequest, "Failed to log user activity");
            }
        }

        [HttpPost]
        [Route("api/JWT/check")]
        public IHttpActionResult CheckUsersClaims([FromBody] ClaimCheckRequest request) {
            try
            {
                if(jwtService.CheckUserClaims(request.JWT, request.ClaimsToCheck) == true)
                {
                    return Content(HttpStatusCode.Accepted, true);
                }
                else
                {
                    return Content(HttpStatusCode.Forbidden, false);
                }
            }
            catch(Exception e)
            {
                _gngLoggerService.LogBadRequest(jwtService.GetUserIDFromToken(request.JWT).ToString(), 
                    request.Ip, request.UrlToEnter, e.ToString());
                return Content(HttpStatusCode.BadRequest, "Service is unavailable");
            }
        }
    }
}
