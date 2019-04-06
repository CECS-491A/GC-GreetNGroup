using ManagerLayer.LoginManagement;
using ServiceLayer.Requests;
using System;
using System.Net;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class LaunchController : ApiController
    {
        [HttpPost]
        [Route("api/launch")]
        public IHttpActionResult Launch([FromBody] LoginRequest request)
        {
            string redirectURL = "https://greetngroup.com/login?" +
                                  "email=" + request.email +
                                  "&signature=" + request.signature +
                                  "&ssoUserId=" + request.ssoUserId +
                                  "&timestamp=" + request.timestamp;
            return Content(HttpStatusCode.Redirect, redirectURL);
        }
    }
}
