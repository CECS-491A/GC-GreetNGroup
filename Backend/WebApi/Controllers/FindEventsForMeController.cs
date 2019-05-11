using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;

namespace Gucci.WebApi.Controllers
{
    public class FindEventsForMeController : ApiController
    {

        // DO NOT HARD CODE YOUR API ROUTES, ADD TO A FILE TO REF
        [HttpPost]
        [Route("api/FindEventsForMe/")]
        public IHttpActionResult GetEvents([FromBody] FindEventsForMeRequest request)
        {
            var eventFinder = new EventFinderService();
            var tempList = new List<Event>();

            var filteredEventList = new List<Event>();
            
            try
            {
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
                    filteredEventList =
                        eventFinder.CullEventListByDateRange(tempList, request.StartDate, request.EndDate);
                }
            }
            catch (ArgumentException argExcept) // Wrong should not be found, but if they are, catch here
            {
                return Content(HttpStatusCode.BadRequest, argExcept);
            }
            catch (Exception e) // Catch all of errors bubbling up from services
            {
                return Content(HttpStatusCode.InternalServerError, e);
            }
            // Return results
            return Ok(filteredEventList);
        }
    }
}
