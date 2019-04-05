using ManagerLayer.LoginManagement;
using ServiceLayer.Requests;
using System;
using System.Net;
using System.Web.Http;

namespace GNG_WebApi.Controllers
{
    public class ProfileController : ApiController
    {
        [HttpGet]
        [Route("api/profile/{userID}")]
        public IHttpActionResult Get(string userID)
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
