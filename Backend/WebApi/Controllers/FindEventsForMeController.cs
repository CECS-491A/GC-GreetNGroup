using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Requests;

namespace WebApi.Controllers
{
    public class FindEventsForMeController : ApiController
    {
        /*
        [Route("api/FindEventsForMe/{useTags}/{useDates}/{useStates}/")]
        public IHttpActionResult GetEvents([FromBody] EventCreationRequest request, string useTags, string useDates, string useStates)
        {
            // make call to function with 4 params     findEvents(useTags, useDates, useStates, filter)
            // use bools to determine strategy
            // strategy will grab from filter as needed
        }
        */
    }
}
