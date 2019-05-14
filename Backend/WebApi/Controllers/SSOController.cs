using Gucci.ManagerLayer;
using Gucci.ServiceLayer.Requests;
using ManagerLayer.UserManagement;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class SSOController : ApiController
    {
        [HttpPost]
        [Route("api/sso/login")]
        public HttpResponseMessage Login([FromBody] SSOUserRequest request)
        {
            SessionManager sessionMan = new SessionManager();
            var response = sessionMan.Login(this, request);
            return response;
        }

        [HttpPost]
        [Route("api/sso/logout")]
        public HttpResponseMessage Logout([FromBody] SSOUserRequest request)
        {
            SessionManager sessionMan = new SessionManager();
            var response = sessionMan.LogoutUsingSSO(request);
            return response;
        }

        [HttpPost]
        [Route("api/sso/deleteuser")]
        public HttpResponseMessage DeleteUser([FromBody] SSOUserRequest request)
        {
            UserManager userMan = new UserManager();
            var response = userMan.DeleteUserUsingSSO(request);
            return response;
        }
    }
}
