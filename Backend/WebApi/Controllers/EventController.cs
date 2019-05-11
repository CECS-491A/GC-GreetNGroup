using System.Net.Http;
using System.Web.Http;
using Gucci.ServiceLayer.Services;
using Gucci.ServiceLayer.Requests;
using System.Net;
using Gucci.ManagerLayer.SearchManager;
using Gucci.ServiceLayer.Interface;
using System;

namespace WebApi.Controllers
{
    public class EventController : ApiController
    {
        private EventService eventService = new EventService();
        private ILoggerService _gngLoggerService = new LoggerService();
        private EventTagService eventTagService = new EventTagService();
        private IJWTService _jwtService = new JWTService();
        private UserService _userService = new UserService();

        /// <summary>
        /// Returns value that has been requested for retrieval in Ok response
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/event/{id}")]
        public IHttpActionResult GetEventById(int eventId, int userId, string ip, string url)
        {
            try
            {
                var e = eventService.GetEventById(eventId);
                //gngLogManager.LogGNGSearchAction(userId.ToString(), id.ToString(), ip);
                return Ok(e);
            }
            catch (Exception e)
            {
                _gngLoggerService.LogBadRequest(userId.ToString(), ip, url, e.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/event/createEvent")]
        public IHttpActionResult CreateNewEvent([FromBody] EventRequest request)
        {
            var userId = _jwtService.GetUserIDFromToken(request.JWT);
            try
            {
                if(_userService.IsUserAtMaxEventCreation(userId) == false)
                {
                    var newEvent = eventService.InsertEvent(userId, request.StartDate, request.EventName,
                    request.Address, request.City, request.State, request.Zip,
                    request.EventTags, request.EventDescription, request.Ip, request.Url);
                    var eventId = newEvent.EventId;
                    if (newEvent != null)
                    {
                        return Content(HttpStatusCode.OK, "Your event was successfully created. Redirecting" +
                            " you to the homepage");
                    }
                    else
                    {
                        return Content(HttpStatusCode.Conflict, "Event Creation was unsuccessful. Redirecting " +
                            "you to the homepage. Please try again.");
                    }
                }
                else
                {
                    return Content(HttpStatusCode.Forbidden, "You have reached your maximum event creation count." +
                        " If you want to create more, please wait until your other events expire");
                }
                
                
            }
            catch(Exception e)
            {
                _gngLoggerService.LogBadRequest(userId.ToString(), request.Ip, request.Url, e.ToString());
                return Content(HttpStatusCode.BadRequest, "Service unavailable");
            }

        }

        [HttpPost]
        [Route("api/event/updateEvent")]
        public IHttpActionResult UpdateEvent([FromBody] EventRequest request)
        {
            var userId = _jwtService.GetUserIDFromToken(request.JWT);
            try
            {
                var eventToUpdate = eventService.GetEventById(request.EventId);
                
                var isSuccessfulUpdate = eventService.UpdateEvent(request.EventId, userId, request.StartDate, request.EventName,
                    request.Address, request.City, request.State, request.Zip, request.EventTags, request.EventDescription, request.Url,
                    request.Ip);
                if(isSuccessfulUpdate == true)
                {
                    return Content(HttpStatusCode.OK, "The event has been updated. Redirecting you to " +
                        "the homepage.");
                }
                else
                {
                    return Content(HttpStatusCode.Conflict, "The update was unsuccessful");
                }
            }
            catch(Exception e)
            {
                _gngLoggerService.LogBadRequest(userId.ToString(), request.Ip, request.Url, e.ToString());
                return BadRequest();
            }
        }

        /*
        [HttpGet]
        [Route("api/event/{eventid}/delete")]
        public IHttpActionResult DeleteEvent([FromBody] int eventId, int userId, string ip)
        {
            try
            {
                var isSuccessfulDelete = eventService.DeleteEvent(eventId, userId, ip);
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
                _gngLoggerService.LogBadRequest(userId.ToString(), ip, "https://www.greetngroup.com/event/" +
                    eventId + "/delete", e.ToString());
                return BadRequest();
            }
        }
        */

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
                if (eventService.IsEventExpired(eventFound.EventId))
                {
                    eventService.SetEventToExpired(eventFound.EventId);
                    return Content(HttpStatusCode.OK, eventFound);
                }

                return Ok(eventFound);
            }
            catch (Exception e)
            {
                // logs error -- does not care about ip or userId
                _gngLoggerService.LogBadRequest("N/A", "N/A", "https://greetngroup.com/search", e.ToString());
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
            catch (Exception e)
            {
                // logs error -- does not care about ip or userId
                _gngLoggerService.LogBadRequest("N/A", "N/A", "https://greetngroup.com/search", e.ToString());
                return BadRequest();
            }
        }
    }
}
