using ManagerLayer.LoginManagement;
using ManagerLayer.ProfileManagement;
using ServiceLayer.Requests;
using System;
using System.Net;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("api/profile")]
        public IHttpActionResult Get([FromBody]string jwtToken)
        {
            try
            {
                ProfileManager pm = new ProfileManager();
                //TODO update in sprint 5 to get user profile information
                if (pm.CheckProfileActivated(jwtToken))
                {
                    return Content(HttpStatusCode.OK, true);
                }
                return Content(HttpStatusCode.Redirect, "User profile needs to be completed");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }
    }
}
