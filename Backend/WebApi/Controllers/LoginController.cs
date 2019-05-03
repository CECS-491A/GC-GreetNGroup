using Gucci.ManagerLayer;
using Gucci.ServiceLayer.Requests;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("api/login")]
        public HttpResponseMessage Login([FromBody] SSOUserRequest request)
        {
            SessionManager sessionMan = new SessionManager();
            var response = sessionMan.Login(this, request);
            return response;
        }
    }
}
