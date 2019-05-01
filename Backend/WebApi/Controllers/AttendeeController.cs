using Gucci.ServiceLayer.Services;
using Gucci.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class AttendeeController : ApiController
    {
        private AttendeesService attendeeService = new AttendeesService();
        private ILoggerService _gngLoggerService = new LoggerService();

        [HttpGet]
        [Route("api/attendee/{eventid}")]
        public IHttpActionResult GetAttendeesList(int eventId)
        {
            try
            {
                //return first and last names of attendees
                var names = attendeeService.GetAttendees(eventId);
                return Ok(names);
            }
            catch (HttpRequestException e)
            {
                _gngLoggerService.LogBadRequest("N/A", "N/A", "https://www.greetngroup.com/attendee/" + eventId, e.ToString());
                return BadRequest();
            }
        }
    }
}
