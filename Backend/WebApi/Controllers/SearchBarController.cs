using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Gucci.ManagerLayer.LogManagement;
using Gucci.ManagerLayer.SearchManager;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Services;

namespace WebApi.Controllers
{
    public class SearchBarController : ApiController
    {
        private ILoggerService _gngLoggerService = new LoggerService();
        private const string url = "https://greetngroup.com/search";

        /// <summary>
        /// Returns a list of events based on partial matching of the user input
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/searchEvent")]
        public IHttpActionResult GetEventByName([FromUri]string name)
        {
            var searchManager = new SearchManager();
            try
            {
                // Prevents no input search
                if (name.Length < 0) Ok();
                
                // Retrieves info for GET
                var e = searchManager.GetEventListByName(name);

                // logs action -- does not care about ip or userId
                _gngLoggerService.LogGNGSearchAction("N/A", name, "N/A");

                return Ok(e);
            }
            catch (Exception e)
            {
                // logs error -- does not care about ip or userId
                _gngLoggerService.LogBadRequest("N/A", "N/A", url, e.ToString());
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns a user if the username matches an existing user in the database
        /// Username is synonymous to email in this application
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/searchUser")]
        public IHttpActionResult GetUserByEmail([FromUri]string username)
        {
            var searchManager = new SearchManager();
            try
            {
                // Prevents no input search
                if (username.Length < 0) Ok();
                
                // Retrieves info for GET
                var e = searchManager.GetUserByUsername(username);
                
                // logs action -- does not care about ip or userId
                _gngLoggerService.LogGNGSearchAction("N/A", username, "N/A");

                return Ok(e);
            }
            catch (Exception e)
            {
                // logs error -- does not care about ip or userId
                _gngLoggerService.LogBadRequest("N/A", "N/A", url, e.ToString());
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns a user if the userId matches an existing userId in the database
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/searchUserId/{uId}")]
        public IHttpActionResult GetUserById(int uId)
        {
            var searchManager = new SearchManager();
            try
            {
                // Retrieves info for GET
                var e = searchManager.GetUserByUserId(uId).FirstOrDefault();
                if (e == null) return Ok("No UserName Found");

                return Ok(e);
            }
            catch (Exception e)
            {
                // logs error -- does not care about ip or userId
                _gngLoggerService.LogBadRequest("N/A", "N/A", url, e.ToString());
                return BadRequest();
            }
        }
    }
}
