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
        public List<Event> CullEventListByDateRange(List<Event> eventList, DateTime startDate, DateTime endDate)
        {
            var resultList = new List<Event>();

            // If the search start date exists after the end date
            if (startDate.CompareTo(endDate) > 0) return resultList;

            // Return a list of events from the given list that are within the range of startDate and endDate
            resultList = eventList.Where(events =>
                events.StartDate.CompareTo(startDate) >= 0 && events.StartDate.CompareTo(endDate) <= 0).ToList();

            resultList.Sort((event1, event2) => DateTime.Compare(event1.StartDate, event2.StartDate));

            return resultList;
        }
    }
}
