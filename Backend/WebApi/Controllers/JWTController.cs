using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using System.Net;
using System.Web.Http;

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
    }
}
