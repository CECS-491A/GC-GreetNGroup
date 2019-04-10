using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Services;
using ManagerLayer.GNGLogManagement;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    public class EventController : ApiController
    {
        GNGLogManager gngLogManager = new GNGLogManager();

        /// <summary>
        /// Returns value that has been requested for retrieval in Ok response
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/event/{id}")]
        public IHttpActionResult GetEventById(int id, string userId, string ip, string url)
        {
            try
            {
                var eventService = new EventService();
                var e = eventService.GetEventById(id);
                return Ok(e);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest(userId, ip, url, e.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/event/createevent")]
        public IHttpActionResult CreateNewEvent(string userId, DateTime startDate, string eventName,
            string address, string city, string state, string zip, List<string> eventTags, string eventDescription,
            string ip, string url)
        {
            try
            {
                EventService eventService = new EventService();
                var newEvent = eventService.InsertEvent(userId, startDate, eventName, address, city, state, zip, eventTags, eventDescription);
                var eventId = newEvent.EventId;
                gngLogManager.LogGNGEventsCreated(userId, eventId, ip);
                return Ok(newEvent);
            }
            catch(HttpRequestException e)
            {
                gngLogManager.LogBadRequest(userId, ip, url, e.ToString());
                return BadRequest();
            }

        }
    }
}
