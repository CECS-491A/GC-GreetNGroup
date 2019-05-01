using Gucci.ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class JWTController : ApiController
    {
        [HttpGet]
        [Route("api/jwt/isvalidtoken")]
        public IHttpActionResult IsJWTTokenValid([FromUri] string jwtToken)
        {
            try
            {
                var _jwtService = new JWTService();
                var isJwtValid = _jwtService.IsJWTSignatureTampered(jwtToken);
                return Content(HttpStatusCode.OK, true);
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, false);
            }
            
        }
    }
}
