using Gucci.ServiceLayer.Services;
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
                //gngLogManager.LogBadRequest(userId.ToString(), ip, url, e.ToString());
                return BadRequest();
            }
        }
    }
}
