using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;
using ManagerLayer.GNGLogManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    public class EventController : ApiController
    {
        public class EventCreationRequest
        {
            [Required]
            public int userId { get; set; }
            [Required]
            public DateTime startDate { get; set; }
            [Required]
            public string eventName { get; set; }
            [Required]
            public string address { get; set; }
            [Required]
            public string city { get; set; }
            [Required]
            public string state { get; set; }
            [Required]
            public string zip { get; set; }
            [Required]
            public List<string> eventTags { get; set; }
            public string eventDescription { get; set; }
            [Required]
            public string ip { get; set; }
            [Required]
            public string url { get; set; }

        }

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
    }
}
