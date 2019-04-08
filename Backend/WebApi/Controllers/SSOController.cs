using ManagerLayer.LoginManagement;
using ManagerLayer.UserManagement;
using ServiceLayer.Requests;
using System;
using System.Net;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class SSOController : ApiController
    {
        [HttpPost]
        [Route("api/SSO/launch")]
        public IHttpActionResult Launch([FromBody] SSOUserRequest request)
        {
            string redirectURL = "https://greetngroup.com/login?" +
                                  "email=" + request.email +
                                  "&signature=" + request.signature +
                                  "&ssoUserId=" + request.ssoUserId +
                                  "&timestamp=" + request.timestamp;
            return Content(HttpStatusCode.Redirect, redirectURL);
        }

        [HttpPost]
        [Route("api/SSO/delete")]
        public IHttpActionResult Delete([FromBody] SSOUserRequest request)
        {
            try
            {
                UserManager um = new UserManager();
                if (um.DeleteUserSSO(request))
                {
                    return Content(HttpStatusCode.OK, "User has been deleted");
                }
                return Content(HttpStatusCode.BadRequest, "Invalid signature");
            }
            catch
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }
    }
}
