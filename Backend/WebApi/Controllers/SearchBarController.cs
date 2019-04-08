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
        [Route("api/search/{name}")]
        public IHttpActionResult GetEventById(string name)
        {
            try
            {
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
    }
}
