using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;

namespace WebApi.Controllers
{
    public class SearchBarController : ApiController
    {
        /// <summary>
        /// Returns a list of events based on partial matching of the user input
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/searchEvent/{name}")]
        public IHttpActionResult GetEventByName(string name)
        {
            try
            {
                if (name.Length < 0) Ok();
                var eventService = new EventService();
                var e = eventService.GetEventListByName(name);
                return Ok(e);
            }
            catch (HttpRequestException e)
            {
                // Add logging
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns a user if the username matches an existing user in the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/searchUser/{username}")]
        public IHttpActionResult GetUserByEmail(string username)
        {
            try
            {
                if (username.Length < 0) Ok();
                var userService = new UserService();
                var e = userService.GetUserByUsername(username);
                return Ok(e);
            }
            catch (HttpRequestException e)
            {
                // Add logging
                return BadRequest();
            }
        }
    }
}
