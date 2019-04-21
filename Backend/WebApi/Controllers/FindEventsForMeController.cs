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
        public class EventSearchFilters
        {
            public string Tags { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string State { get; set; }
        }

        /*
        [Route("api/FindEventsForMe/{useTags}/{useDates}/{useStates}/")]
        public IHttpActionResult GetEvents([FromUri] EventSearchFilters filter)
        {
            // make call to function with 4 params     findEvents(useTags, useDates, useStates, filter)
            // use bools to determine strategy
            // strategy will grab from filter as needed
        }
        }
        */
    }
}
