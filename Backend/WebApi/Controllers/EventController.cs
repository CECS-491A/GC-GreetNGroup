using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;
using ManagerLayer.GNGLogManagement;
using ServiceLayer.Requests;
using System.Net;

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
        public IHttpActionResult CreateNewEvent([FromBody] EventRequest request)
        {
            try
            {
                var newEvent = eventService.InsertEvent(request.userId, request.startDate, request.eventName,
                    request.address, request.city, request.state, request.zip, 
                    request.eventTags, request.eventDescription);
                var eventId = newEvent.EventId;
                if(newEvent != null)
                {
                    gngLogManager.LogGNGEventsCreated(request.userId.ToString(), eventId, request.ip);
                    return Ok(newEvent);
                }
                else
                {
                    gngLogManager.LogErrorsEncountered(request.userId.ToString(), HttpStatusCode.Conflict.ToString(),
                        request.url, "The event failed to be created", request.ip);
                    return Content(HttpStatusCode.Conflict, "Event Creation was unsuccessful");
                }
                
            }
            catch(HttpRequestException e)
            {
                gngLogManager.LogBadRequest(request.userId.ToString(), request.ip, request.url, e.ToString());
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
                    request.address, request.city, request.state, request.zip, request.eventTags, request.eventDescription);
                if(isSuccessfulUpdate == true)
                {
                    gngLogManager.LogGNGEventUpdate(request.eventId, request.userId.ToString(), request.ip);
                    return Content(HttpStatusCode.OK, true);
                }
                else
                {
                    gngLogManager.LogErrorsEncountered(request.userId.ToString(), HttpStatusCode.Conflict.ToString(), 
                        request.url, "The event failed to update", request.ip);
                    return Content(HttpStatusCode.Conflict, "The update was unsuccessful");
                }
            }
            catch(HttpResponseException e)
            {
                gngLogManager.LogBadRequest(request.userId.ToString(), request.ip, request.url, e.ToString());
                return BadRequest();
            }
        }
    }
}
