using System.Net.Http;
using System.Web.Http;
using Gucci.ServiceLayer.Services;
using Gucci.ServiceLayer.Requests;
using System.Net;
using Gucci.ManagerLayer.SearchManager;
using Gucci.ServiceLayer.Interface;

namespace WebApi.Controllers
{
    public class EventController : ApiController
    {
        private EventService eventService = new EventService();
        private ILoggerService _gngLoggerService = new LoggerService();
        private EventTagService eventTagService = new EventTagService();

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
                //gngLogManager.LogGNGSearchAction(userId.ToString(), id.ToString(), ip);
                return Ok(e);
            }
            catch (HttpRequestException e)
            {
                //gngLogManager.LogBadRequest(userId.ToString(), ip, url, e.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/event/createevent")]
        public IHttpActionResult CreateNewEvent([FromBody] EventRequest request)
        {
            try
            {
                var newEvent = eventService.InsertEvent(request.userId, request.startDate, request.eventName,
                    request.address, request.city, request.state, request.zip, 
                    request.eventTags, request.eventDescription, request.ip, request.url);
                var eventId = newEvent.EventId;
                if(newEvent != null)
                {
                    return Ok(newEvent);
                }
                else
                {
                    _gngLoggerService.LogErrorsEncountered(request.userId.ToString(), HttpStatusCode.Conflict.ToString(),
                        request.url, "The event failed to be created", request.ip);
                    return Content(HttpStatusCode.Conflict, "Event Creation was unsuccessful");
                }
                
            }
            catch(HttpRequestException e)
            {
                _gngLoggerService.LogBadRequest(request.userId.ToString(), request.ip, request.url, e.ToString());
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("api/event/{eventId}/updateevent")]
        public IHttpActionResult UpdateEvent([FromBody] EventRequest request)
        {
            try
            {
                var eventToUpdate = eventService.GetEventById(request.eventId);
                var isSuccessfulUpdate = eventService.UpdateEvent(request.eventId, request.userId, request.startDate, request.eventName,
                    request.address, request.city, request.state, request.zip, request.eventTags, request.eventDescription, request.url,
                    request.ip);
                if(isSuccessfulUpdate == true)
                {
                    return Content(HttpStatusCode.OK, true);
                }
                else
                {
                    return Content(HttpStatusCode.Conflict, "The update was unsuccessful");
                }
            }
            catch(HttpResponseException e)
            {
                _gngLoggerService.LogBadRequest(request.userId.ToString(), request.ip, request.url, e.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/event/{eventid}/delete")]
        public IHttpActionResult DeleteEvent([FromBody] int eventId)
        {
            try
            {
                var isSuccessfulDelete = eventService.DeleteEvent(eventId);
                if(isSuccessfulDelete == true)
                {

                    return Content(HttpStatusCode.OK, true);
                }
                else
                {
                    return Content(HttpStatusCode.Conflict, "The deletion was unsuccessful");
                }
            }
            catch(HttpResponseException e)
            {
                _gngLoggerService.LogBadRequest("", "", "", e.ToString());
                return BadRequest();
            }
        }
        
        /// <summary>
        /// Returns a list of events based on partial matching of the user input
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/event/info")]
        public IHttpActionResult GetEventByName([FromUri]string name)
        {
            try
            {
                // Prevents no input search
                if (name.Length < 0) Ok();

                // Retrieves info for GET
                var eventFound = eventService.GetEventByName(name);

                return Ok(eventFound);
            }
            catch (HttpRequestException error)
            {
                // logs error -- does not care about ip or userId
                // gngLogManager.LogBadRequest("", "", "https://greetngroup.com/search", e.ToString());
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns a list of events based on partial matching of the user input
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/event/tags/{eventid}")]
        public IHttpActionResult GetEventTAGS(int eventId)
        {
            try
            {
                // Retrieves info for GET
                var tags = eventTagService.ReturnEventTagsOfEvent(eventId);

                return Ok(tags);
            }
            catch (HttpRequestException error)
            {
                // logs error -- does not care about ip or userId
                // gngLogManager.LogBadRequest("", "", "https://greetngroup.com/search", e.ToString());
                return BadRequest();
            }
        }
    }
}
