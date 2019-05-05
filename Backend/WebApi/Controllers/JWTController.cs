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
        public class TokenRequest
        {
            public string token { get; set; }
        }
        [HttpPost]
        [Route("api/jwt/isvalidtoken")]
        public IHttpActionResult IsJWTTokenValid([FromBody] TokenRequest request)
        {
            try
            {
                var _jwtService = new JWTService();
                var isJwtValid = _jwtService.IsTokenValid(request.token);
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
