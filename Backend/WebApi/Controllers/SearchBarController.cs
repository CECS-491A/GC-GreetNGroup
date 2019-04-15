using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;
using ManagerLayer.GNGLogManagement;

namespace WebApi.Controllers
{
    public class SearchBarController : ApiController
    {
        GNGLogManager gngLogManager = new GNGLogManager();

        /// <summary>
        /// Returns a list of events based on partial matching of the user input
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/searchEvent/{name}")]
        public IHttpActionResult GetEventByName(string name, int userId, string ip, string url)
        {
            try
            {
                if (name.Length < 0) Ok();
                var eventService = new EventService();
                var e = eventService.GetEventListByName(name);
                gngLogManager.LogGNGSearchAction(userId.ToString(), name, ip);
                return Ok(e);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest(userId.ToString(), ip, url, e.ToString());
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
        public IHttpActionResult GetUserByEmail(string username, int userId, string ip, string url)
        {
            try
            {
                if (username.Length < 0) Ok();
                var userService = new UserService();
                var e = userService.GetUserByUsername(username);
                gngLogManager.LogGNGSearchAction(userId.ToString(), username, ip);
                return Ok(e);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest(userId.ToString(), ip, url, e.ToString());
                return BadRequest();
            }
        }
    }
}
