using System;
using System.Collections.Generic;
using System.Linq;
using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;

namespace Gucci.ServiceLayer.Services
{
    public class EventFinderService
    {
        private ILoggerService _gngLoggerService;

        // Filters list of given event information by removing passed dates
        public List<Event> FilterOutPastEvents(List<Event> unfiltered)
        {
            var filtered = new List<Event>();
            var currentTime = new DateTime().ToLocalTime();
            foreach (var c in unfiltered)
            {
                if (DateTime.Compare(c.StartDate, currentTime) >= 0)
                {
                    filtered.Add(c);
                }
            }

            return filtered;
        }

        public List<Event> FindAllEvents()
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var eventList = ctx.Events.ToList();
                    eventList = FilterOutPastEvents(eventList);
                    return eventList;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                throw;
            }
            catch (Exception e) // This is a catch all for error occuring in db
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Return lists of events based on list of tags
        public List<Event> FindEventByEventTags(List<string> eventTags)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Turn string names of tags into tag id references
                    var tagIds = new List<int>();
                    foreach (var tagName in eventTags)
                    {
                        tagIds.Add(ctx.Tags.First(tag => tag.TagName.Equals(tagName)).TagId);
                    }

                    // Return list of events and their tags simplified into divisions of [eventId, list of tags they contain]
                    var simplifiedPairs = ctx.EventTags.GroupBy(events => events.EventId).Select(events => new
                    {
                        EventId = events.Key,
                        Tags = events.Select(tags => tags.TagId).ToList()
                    });

                    // Retrieve events where tags are found
                    var filteredEventList = new List<Event>();
                    foreach (var pair in simplifiedPairs)
                    {
                        // If an event contains any of the tags within the tagId list, then it will be added to the result list
                        if (tagIds.Except(pair.Tags).Count() < tagIds.Count)
                        {
                            filteredEventList.Add(ctx.Events.FirstOrDefault(pairs => pairs.EventId.Equals(pair.EventId)));
                        }
                    }

                    return filteredEventList;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                throw;
            }
            catch (Exception e) // This is a catch all for error occuring in db
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Returns complete list of events sorted by those existing within a date range
        public List<Event> FindEventsByDateRange(string sDate, string eDate)
        {
            var resultList = new List<Event>();
            var startDate = DateTime.Parse(sDate);
            var endDate = DateTime.Parse(eDate);

            // If the search start date exists after the end date
            if (startDate.CompareTo(endDate) > 0) return resultList;

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Return events where the startDate of the event is within the range of startDate and endDate
                    resultList = ctx.Events.Where(e => e.StartDate.CompareTo(startDate) >= 0 && e.StartDate.CompareTo(endDate) <= 0).ToList();
                    // Sorts result by StartDate
                    resultList.Sort((event1, event2) => DateTime.Compare(event1.StartDate, event2.StartDate));

                    // Removes events past current date
                    resultList = FilterOutPastEvents(resultList);
                    return resultList;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                throw;
            }
            catch (Exception e) // Catch all for error occuring in db
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Return a sorted list when given a list, of events falling in the range of startDate and endDate
        public List<Event> CullEventListByDateRange(List<Event> eventList, string startDate, string endDate)
        {
            var resultList = new List<Event>();
            var sDate = DateTime.Parse(startDate);
            var eDate = DateTime.Parse(endDate);

            // If the search start date exists after the end date
            if (sDate.CompareTo(eDate) > 0) return resultList;

            // Return a list of events from the given list that are within the range of startDate and endDate
            resultList = eventList.Where(events =>
                events.StartDate.CompareTo(sDate) >= 0 && events.StartDate.CompareTo(eDate) <= 0).ToList();

            resultList.Sort((event1, event2) => DateTime.Compare(event1.StartDate, event2.StartDate));

            return resultList;
        }

        // Finds events byn the state they are hosted within
        public List<Event> FindEventsByState(string state)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Finds events where state location exists within event location of event
                    var resultList = ctx.Events.Where(events => events.EventLocation.Contains(state)).ToList();

                    // Sorts result by StartDate
                    resultList.Sort((event1, event2) => DateTime.Compare(event1.StartDate, event2.StartDate));

                    // Removes events past current date
                    resultList = FilterOutPastEvents(resultList);
                    return resultList;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                throw;
            }
            catch (Exception e) // Catch all for error occuring in db
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Return a sorted list when given a list, of events falling in the range of startDate and endDate
        public List<Event> CullEventListByState(List<Event> eventList, string state)
        {
            var resultList = new List<Event>();

            if (eventList != null)
            {
                // Return a list of events from the given list that are within the state requested
                resultList = eventList.Where(events => events.EventLocation.Contains(state)).ToList();
            }

            // Sort list by date
            resultList.Sort((event1, event2) => DateTime.Compare(event1.StartDate, event2.StartDate));

            return resultList;
        }
    }
}
