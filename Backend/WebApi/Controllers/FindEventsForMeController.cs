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
            public string Outdoors { get; set; }
            public string Indoors { get; set; }
            public string Music { get; set; }
            public string Games { get; set; }
            public string Fitness { get; set; }
            public string Art { get; set; }
            public string Sports { get; set; }
            public string Educational { get; set; }
            public string Food { get; set; }
            public string Discussion { get; set; }
            public string Miscellaneous { get; set; }
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
