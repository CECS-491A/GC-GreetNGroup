using ManagerLayer.LoginManagement;
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
                //TODO update in sprint 5 to get user profile information
                return Content(HttpStatusCode.OK, "user");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "Service Unavailable");
            }
        }
    }
}
