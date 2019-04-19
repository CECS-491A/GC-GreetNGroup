using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Tables;
using ServiceLayer.Interface;

namespace ServiceLayer.Services
{
    public class EventService
    {
        private IGNGLoggerService _gngLoggerService;
        private Dictionary<string, int> tagIds;
        private int eventId;
        public EventService()
        {
            _gngLoggerService = new GNGLoggerService();
            tagIds = GenerateEventTagIds();
            Int32.TryParse(Environment.GetEnvironmentVariable("EventId", EnvironmentVariableTarget.User), out eventId);
        }

        /*
         * The functions within this service make use of the database context
         * and similarly attempt to catch
         *      ObjectDisposedException
         * to ensure the context is still valid and we want to catch the error
         * where it has been made
         *
         */

        /// <summary>
        /// The following region inserts an event/event details into the event database
        /// </summary>
        #region Insert Event Information

        public bool InsertMadeEvent(Event e)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    ctx.Events.Add(e);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        public Event InsertEvent(int userId, DateTime startDate, string eventName, 
            string address, string city, string state, string zip, List<string> eventTags, string eventDescription)
        {
            Event userEvent = null;
            if (IsUserAtMaxEventCreation(userId) == false)
            {
                try
                {
                    string eventLocation = ParseAddress(address, city, state, zip);

                    using (var ctx = new GreetNGroupContext())
                    {
                        userEvent = new Event(userId, eventId, startDate, eventName, eventLocation, eventDescription);

                        ctx.Events.Add(userEvent);
                        ctx.SaveChanges();
                        if(InsertEventTags(eventTags, userEvent.EventId) == false)
                        {
                            userEvent = null;
                        }
                        eventId++;
                        Environment.SetEnvironmentVariable("EventId", eventId.ToString(), EnvironmentVariableTarget.User);
                    }
                    return userEvent;
                }
                catch (ObjectDisposedException od)
                {
                    _gngLoggerService.LogGNGInternalErrors(od.ToString());
                    return userEvent;
                }
            }
            else
            {
                return userEvent;
            }
            
        }

        public bool InsertEventTags(List<string> eventTags, int eventId)
        {
            bool isSuccessfulAdd = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var gngEvent = ctx.Events.FirstOrDefault(e => e.EventId.Equals(eventId));
                    foreach (string tag in eventTags)
                    {
                        if (tagIds.ContainsKey(tag))
                        {
                            var tagToAdd = ctx.Tags.FirstOrDefault(t => t.TagName.Equals(tag));
                            var tagIdNum = tagToAdd.TagId;
                            var eventTag = new EventTag(eventId, tagIdNum);
                            ctx.EventTags.Add(eventTag);
                        }
                    }
                    ctx.SaveChanges();
                    isSuccessfulAdd = true;
                }
                return isSuccessfulAdd;
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isSuccessfulAdd;
            }
            
        }

        #endregion

        /// <summary>
        /// The following region handles update of Event specific information
        /// </summary>
        #region Update Event Information

        public bool UpdateEventStartDate(string eId, DateTime startDate)
        {
            bool isSuccessfullyUpdated = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    Event curEvent = ctx.Events.FirstOrDefault(c => c.EventId.Equals(eId));
                    if (curEvent != null)
                        curEvent.StartDate = startDate;
                    ctx.SaveChanges();
                    isSuccessfullyUpdated = true;
                }
                return isSuccessfullyUpdated;
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isSuccessfullyUpdated;
            }
        }

        public bool UpdateEventName(string eId, string newEventName)
        {
            bool isSuccessfullyUpdated = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    Event curEvent = ctx.Events.FirstOrDefault(c => c.EventId.Equals(eId));
                    if (curEvent != null)
                        curEvent.EventName = newEventName;
                    ctx.SaveChanges();
                    isSuccessfullyUpdated = true;
                }
                return isSuccessfullyUpdated;
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isSuccessfullyUpdated;
            }
        }

        public bool UpdateEventLocation(string eId, string address, string city, string state, string zip)
        {
            bool isSuccessfullyUpdated = false;
            try
            {
                string newLocation = ParseAddress(address, city, state, zip);
                using (var ctx = new GreetNGroupContext())
                {
                    Event curEvent = ctx.Events.FirstOrDefault(c => c.EventId.Equals(eId));
                    if (curEvent != null)
                        curEvent.EventLocation = newLocation;
                    ctx.SaveChanges();
                    isSuccessfullyUpdated = true;
                }
                return isSuccessfullyUpdated;
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isSuccessfullyUpdated;
            }
        }

        #endregion
        
        /// <summary>
        /// The following region handles Event information deletion
        /// </summary>
        #region Delete Event Information

        public bool DeleteEvent(int eId)
        {
            bool isSuccessfullyDeleted = false;

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    Event curEvent = ctx.Events.FirstOrDefault(c => c.EventId.Equals(eId));
                    if (curEvent != null)
                        ctx.Events.Remove(curEvent);
                    ctx.SaveChanges();
                    isSuccessfullyDeleted = true;
                }
                return isSuccessfullyDeleted;
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isSuccessfullyDeleted;
            }

        }
        #endregion
        
        /// <summary>
        /// The following region retrieves confirmation of information within the event database
        /// </summary>
        #region Event Information Check

        public bool IsEventIdFound(int eventId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    return ctx.Events.Any(c => c.EventId.Equals(eventId));
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        #endregion

        /// <summary>
        /// The following region retrieves event information from the database
        /// </summary>
        #region Event Information Retrieval

        public Event GetEventById(int eId)
        {
            Event e = null;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    e = ctx.Events.FirstOrDefault(c => c.EventId.Equals(eId));
                    return e;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return e;
            }
        }

        // Retrieves event in list format given id
        public List<Event> GetEventListById(int eId)
        {
            List<Event> e = null;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    e = ctx.Events.Where(c => c.EventId.Equals(eId)).ToList();
                    return e;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return e;
            }
        }

        // Retrieves event in list format give partial event name/search input
        public List<Event> GetEventListByName(string searchInput)
        {
            List<Event> e = null;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    e = ctx.Events.Where(name => name.EventName.Contains(searchInput)).ToList();
                    return e;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return e;
            }
        }

        /// <summary>
        /// Method IsUserAtMaxEventCreation queries the database to check the creation
        /// count of the user attempting to create an event. If the user has reached 5
        /// or more events created, the method returns false and the user cannot create
        /// any more events.
        /// </summary>
        /// <param name="userId">Hashed user id of the user attempting to create an event</param>
        /// <returns>Return a bool value depending on if the user has reached the creation
        /// count threshold or not</returns>
        public bool IsUserAtMaxEventCreation(int userId)
        {
            bool isAtMax = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    int creationCount;
                    var user = ctx.Users.Where(c => c.UserId.Equals(userId));
                    Int32.TryParse(user.Select(c => c.EventCreationCount).ToString(), out creationCount);

                    if (creationCount >= 5)
                    {
                        isAtMax = true;
                    }
                }
                return isAtMax;
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isAtMax;
            }
        }

        #endregion

        /// <summary>
        /// The following region performs event specific functions in order to properly
        /// perform the CRUD functions for events
        /// </summary>
        #region Event Information Calculations
        
        /// <summary>
        /// Method ParseAddress takes the separate address components of the event form and
        /// concatenates the components into one address to store in the database
        /// </summary>
        /// <param name="address">Address of the event</param>
        /// <param name="city">City of where the event will be held</param>
        /// <param name="state">State of where the event will be held</param>
        /// <param name="zip">The zipcode of the address</param>
        /// <returns>Concatenated address in string form</returns>
        public string ParseAddress(string address, string city, string state, string zip)
        {
            return address + " " + city + ", " + state + " " + zip;
        }

        public Dictionary<string, int> GenerateEventTagIds()
        {
            Dictionary<string, int> eventTagIds = new Dictionary<string, int>();
            eventTagIds.Add("Outdoors", 1);
            eventTagIds.Add("Indoors", 2);
            eventTagIds.Add("Music", 3);
            eventTagIds.Add("Games", 4);
            eventTagIds.Add("Fitness", 5);
            eventTagIds.Add("Art", 6);
            eventTagIds.Add("Sports", 7);
            eventTagIds.Add("Miscellaneous", 8);
            eventTagIds.Add("Educational", 9);
            eventTagIds.Add("Food", 10);
            eventTagIds.Add("Discussion", 11);

            return eventTagIds;
        }

        #endregion
    }



}
