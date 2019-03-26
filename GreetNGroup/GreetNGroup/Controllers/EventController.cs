using System.Net.Http;
using System.Web.Http;
using GreetNGroup.DataAccess.Queries;

namespace GreetNGroup.Controllers
{
    public class EventController : ApiController
    {
        /// <summary>
        /// Returns value that has been requested for retrieval in Ok response
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [Route("Event")]
        public IHttpActionResult GetEventById(int id)
        {
            try
            {
                var e = DbRetrieve.GetEventById(id);
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
