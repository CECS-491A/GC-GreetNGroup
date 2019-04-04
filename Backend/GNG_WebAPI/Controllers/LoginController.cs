using ManagerLayer.LoginManagement;
using ServiceLayer.Requests;
using System;
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
                LoginManager lm = new LoginManager();
                int response = lm.Login(request);
                if(response == 1)
                {

                }else if(response == -1)
                {

                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "Service Unavailable");
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }
    }
}
