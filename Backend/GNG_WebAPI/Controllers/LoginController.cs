using GNG_WebApi.Requests;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;

namespace GNG_WebApi.Controllers
{
    public class LoginController : ApiController
    {
        
        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login([FromBody] LoginRequest request)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }
    }
}
