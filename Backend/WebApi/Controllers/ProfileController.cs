using ManagerLayer.LoginManagement;
using ServiceLayer.Requests;
using System;
using System.Net;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ProfileController : ApiController
    {
        [HttpGet]
        [Route("api/profile/{userID}")]
        public IHttpActionResult Get(string userID)
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
