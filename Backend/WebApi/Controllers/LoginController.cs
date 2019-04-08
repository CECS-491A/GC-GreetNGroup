using ManagerLayer.LoginManagement;
using ServiceLayer.Requests;
using System;
using System.Net;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login([FromBody] SSOUserRequest request)
        {
            try
            {
                LoginManager lm = new LoginManager();
                string response = lm.Login(request);
                if (response.Equals("-1"))
                {
                    return Content(HttpStatusCode.BadRequest, "Invalid session");
                }
                else
                {
                    return Content(HttpStatusCode.Redirect, response);
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }
    }
}
