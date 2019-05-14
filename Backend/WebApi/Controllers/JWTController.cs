using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using System.Net;
using System.Web.Http;
using System;

namespace WebApi.Controllers
{
    public class JWTController : ApiController
    {
        [HttpPost]
        [Route("api/jwt/isvalidtoken/{jwtToken}")]
        public IHttpActionResult IsJWTTokenValid([FromUri] string jwtToken)
        {
            try
            {
                var _jwtService = new JWTService();
                var isJwtValid = _jwtService.IsTokenValid(jwtToken);
                if (!isJwtValid)
                {
                    return Content(HttpStatusCode.BadRequest, false);
                }
                return Content(HttpStatusCode.OK, true);
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, false);
            }
        }

        [HttpPost]
        [Route("api/jwt/check")]
        public IHttpActionResult CheckUsersClaims([FromBody] ClaimCheckRequest request)
        {
            var _jwtService = new JWTService();
            var _gngLoggerService = new LoggerService();

            var claimsToCheckResult = _jwtService.CheckUserClaims(request.JWT, request.ClaimsToCheck);
            var expirationCheckResult = _jwtService.IsTokenExpired(request.JWT);

            try
            {
                if (claimsToCheckResult.Equals("Authorized") &&
                    expirationCheckResult.Equals("NotExpired"))
                {
                    return Content(HttpStatusCode.OK, "Authorized to view content");
                }
                else if(claimsToCheckResult.Equals("Authorized") &&
                    expirationCheckResult.Equals("Expired"))
                {
                    return Content(HttpStatusCode.Forbidden, "There was a problem in checking your session, please " +
                        "try again");
                }
                else if (claimsToCheckResult.Equals("Unauthorized"))
                {
                    return Content(HttpStatusCode.Forbidden, "You are unauthorized to view this content. If this " +
                        "was a mistake, please contact an admin");
                }
                else
                {
                    return Content(HttpStatusCode.Forbidden, "There was an problem in checking your session, please re-login and try again");
                }
            }
            catch (Exception e)
            {
                _gngLoggerService.LogBadRequest(_jwtService.GetUserIDFromToken(request.JWT).ToString(),
                    request.Ip, request.UrlToEnter, e.ToString());
                return Content(HttpStatusCode.BadRequest, "Service is unavailable");
            }
        }
    }
}
