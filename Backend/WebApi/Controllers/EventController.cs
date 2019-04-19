using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;
using ManagerLayer.GNGLogManagement;
using ServiceLayer.Requests;
using ManagerLayer.SearchManager;

namespace WebApi.Controllers
{
    public class EventController : ApiController
    {
        
        GNGLogManager gngLogManager = new GNGLogManager();
        EventService eventService = new EventService();

        /// <summary>
        /// Returns value that has been requested for retrieval in Ok response
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/event/{id}")]
        public IHttpActionResult GetEventById(int id, int userId, string ip, string url)
        {
            try
            {
                var eventService = new EventService();
                var e = eventService.GetEventById(id);
                gngLogManager.LogGNGSearchAction(userId.ToString(), id.ToString(), ip);
                return Ok(e);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest(userId.ToString(), ip, url, e.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/event/createevent")]
        public IHttpActionResult CreateNewEvent([FromBody] EventCreationRequest request)
        {
            try
            {
                var newEvent = eventService.InsertEvent(request.userId, request.startDate, request.eventName,
                    request.address, request.city, request.state, request.zip, 
                    request.eventTags, request.eventDescription);
                var eventId = newEvent.EventId;
                gngLogManager.LogGNGEventsCreated(request.userId.ToString(), eventId, request.ip);
                return Ok(newEvent);
            }
            catch(HttpRequestException e)
            {
                gngLogManager.LogBadRequest(request.userId.ToString(), request.ip, request.url, e.ToString());
                return BadRequest();
            }

        }

        /// <summary>
        /// Returns a list of events based on partial matching of the user input
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/eventInfo")]
        public IHttpActionResult GetEventByName([FromUri]string name)
        {
            var searchManager = new SearchManager();
            try
            {
                // Prevents no input search
                if (name.Length < 0) Ok();

                // Retrieves info for GET
                var e = searchManager.GetEventListByName(name);

                return Ok(e);
            }
            catch (HttpRequestException e)
            {
                // logs error -- does not care about ip or userId
                // gngLogManager.LogBadRequest("", "", "https://greetngroup.com/search", e.ToString());
                return BadRequest();
            }
        }
    }
}
