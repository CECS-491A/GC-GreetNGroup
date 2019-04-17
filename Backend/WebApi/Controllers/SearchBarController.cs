using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;
using ManagerLayer.GNGLogManagement;

namespace WebApi.Controllers
{
    public class SearchBarController : ApiController
    {
        GNGLogManager gngLogManager = new GNGLogManager();

        // For FromBody field, supplies current user Ip, and url
        public class SearchBarRequest
        {
            [Required] public string Ip { get; set; }
            [Required] public string Url { get; set; }
        }

        /// <summary>
        /// Returns a list of events based on partial matching of the user input
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/searchEvent/{name}")]
        public IHttpActionResult GetEventByName(string name, int userId, [FromBody]SearchBarRequest req)
        {
            try
            {
                if (name.Length < 0) Ok();
                var eventService = new EventService();
                var e = eventService.GetEventListByName(name);
                gngLogManager.LogGNGSearchAction(userId.ToString(), name, req.Ip);
                return Ok(e);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest(userId.ToString(), req.Ip, req.Url, e.ToString());
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
        public IHttpActionResult GetUserByEmail(string username, int userId, [FromBody]SearchBarRequest req)
        {
            try
            {
                if (username.Length < 0) Ok();
                var userService = new UserService();
                var e = userService.GetUserByUsername(username);
                gngLogManager.LogGNGSearchAction(userId.ToString(), username, req.Ip);
                return Ok(e);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest(userId.ToString(), req.Ip, req.Url, e.ToString());
                return BadRequest();
            }
        }
    }
}
