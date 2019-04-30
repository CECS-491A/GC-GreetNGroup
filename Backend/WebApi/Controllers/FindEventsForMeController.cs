using System;
using System.Collections.Generic;
using System.Web.Http;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;

namespace Gucci.WebApi.Controllers
{
    public class FindEventsForMeController : ApiController
    {
        [HttpPost]
        [Route("api/FindEventsForMe/")]
        public IHttpActionResult GetEvents([FromBody] FindEventsForMeRequest request)
        {
            var eventFinder = new EventFinderService();
            var tempList = new List<Event>();

            var filteredEventList = new List<Event>();
            // make call to function with 4 params     findEvents(useTags, useDates, useStates, filter)
            // use bools to determine strategy
            // strategy will grab from filter as needed

            // null blank inputs will be caught in the frontend
            // a secondary check is created in here for off cases

            // Return list of events unfiltered
            if (!request.UseTags && !request.UseDates && !request.UseLocation)
            {
                filteredEventList = eventFinder.FindAllEvents();
            }
            // Return list of events filtered by tags
            else if (request.UseTags && !request.UseDates && !request.UseLocation)
            {
                filteredEventList = eventFinder.FindEventByEventTags(request.Tags);
            }
            // Filtered by Dates
            else if (!request.UseTags && request.UseDates && !request.UseLocation)
            {
                filteredEventList = eventFinder.FindEventsByDateRange(request.StartDate, request.EndDate);
            }
            // Filtered by Location
            else if (!request.UseTags && !request.UseDates && request.UseLocation)
            {
                filteredEventList = eventFinder.FindEventsByState(request.State);
            }
            // Filtered by Tags and Dates
            else if (request.UseTags && request.UseDates && !request.UseLocation)
            {
                tempList = eventFinder.FindEventByEventTags(request.Tags);
                filteredEventList = eventFinder.CullEventListByDateRange(tempList, 
                    request.StartDate, request.EndDate);
            }
            // Filtered by Tags and Location
            else if (request.UseTags && !request.UseDates && request.UseLocation)
            {
                tempList = eventFinder.FindEventByEventTags(request.Tags);
                filteredEventList = eventFinder.CullEventListByState(tempList, request.State);
            }
            // Filtered by Dates and Location
            else if (!request.UseTags && request.UseDates && request.UseLocation)
            {
                tempList = eventFinder.FindEventsByDateRange(request.StartDate, request.EndDate);
                filteredEventList = eventFinder.CullEventListByState(tempList, request.State);
            }
            // Filtered by Tags, Dates, and Location
            else if (request.UseTags && request.UseDates && request.UseLocation)
            {
                tempList = eventFinder.FindEventByEventTags(request.Tags);
                tempList = eventFinder.CullEventListByState(tempList, request.State);
                filteredEventList = eventFinder.CullEventListByDateRange(tempList, request.StartDate, request.EndDate);
            }

            // Return results
            return Ok(filteredEventList);
        }
    }
}
