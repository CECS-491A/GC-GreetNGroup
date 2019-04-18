using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class FindEventsForMeController : ApiController
    {
        // For FromBody field, supplies current user Ip, and url
        public class FindEventsForMeRequest
        {
            [Required] public string Ip { get; set; }
            [Required] public string Url { get; set; }
        }

        /*
        [Route("api/FindEventsForMe/{date}")]
        public IHttpActionResult GetEventsByDate(DateTime date)
        {

        }

        [Route("api/FindEventsForMe/{state}")]
        public IHttpActionResult GetEventsByState(string state)
        {

        }

        [Route("api/FindEventsForMe/{tag}")]
        public IHttpActionResult GetEventsByTag([FromUri] List<string> tag)
        {

        }

        [Route("api/FindEventsForMe/{tag}/{date}")]
        public IHttpActionResult GetEventsByTagAndDate([FromUri] List<string> tag, DateTime date)
        {

        }

        [Route("api/FindEventsForMe/{tag}/{date}/{state}")]
        public IHttpActionResult GetEventsByTagDateAndState([FromUri] List<string> tag, DateTime date, string state)
        {

        }
        */
    }
}
