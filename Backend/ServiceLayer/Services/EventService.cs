using System;
using System.Collections.Generic;
using System.Linq;
using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.DataAccessLayer.Models;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Model;
using Gucci.DataAccessLayer.DataTransferObject;

namespace Gucci.ServiceLayer.Services
{
    public class EventService
    {
        private ILoggerService _gngLoggerService;
        private IEventTagService _eventTagService;
        private int eventId;
        private Configurations configurations;

        public EventService()
        {
            configurations = new Configurations();
            _gngLoggerService = new LoggerService();
            _eventTagService = new EventTagService();
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
            string address, string city, string state, string zip, List<string> eventTags, string eventDescription,
            string ip, string url)
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
                        foreach(var tags in eventTags)
                        {
                            if (_eventTagService.InsertEventTag(eventId, tags) == false)
                            {
                                userEvent = null;
                                _gngLoggerService.LogErrorsEncountered(userId.ToString(), "409 Conflict",
                                    url, "The event failed to be created", ip);
                                return userEvent;
                            }
                        }
                        LogGNGEventsCreated(userId.ToString(), eventId, ip);
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

        #endregion

        /// <summary>
        /// The following region handles update of Event specific information
        /// </summary>
        #region Update Event Information
 
        public bool UpdateEvent(int eId, int userId, DateTime startDate, string eventName,
            string address, string city, string state, string zip, List<string> eventTags, string eventDescription,
            string url, string ip)
        {
            var isSuccessfullyUpdated = false;
            var isTagsSuccessfullyUpdated = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var currentEvent = ctx.Events.FirstOrDefault(e => e.EventId.Equals(eId));
                    if (currentEvent != null)
                    {
                        currentEvent.UserId = userId;
                        currentEvent.EventName = eventName;
                        currentEvent.StartDate = startDate;
                        currentEvent.EventLocation = ParseAddress(address, city, state, zip);
                        currentEvent.EventDescription = eventDescription;
                        ctx.SaveChanges();

                        if(eventTags.Count != 0)
                        {
                            var currentEventTags = ctx.EventTags.Where(e => e.EventId.Equals(eId));

                            foreach(var tags in currentEventTags)
                            {
                                if (!eventTags.Contains(tags.Tag.TagName))
                                {
                                    isTagsSuccessfullyUpdated = _eventTagService.DeleteEventTag(eId, tags.Tag.TagName);
                                }
                                else
                                {
                                    eventTags.Remove(tags.Tag.TagName);
                                }
                            }

                            foreach (var tags in eventTags)
                            {
                                isTagsSuccessfullyUpdated = _eventTagService.InsertEventTag(eId, tags);
                            }

                        }
                        if(isTagsSuccessfullyUpdated == true)
                        {
                            isSuccessfullyUpdated = true;
                            LogGNGEventUpdate(eventId, userId.ToString(), ip);
                        }
                    }
                    else
                    {
                        _gngLoggerService.LogErrorsEncountered(userId.ToString(), "409 Conflict", url, "The event failed" +
                            "to be updated", ip);
                        return isSuccessfullyUpdated;
                    }
                }
                return isSuccessfullyUpdated;
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isSuccessfullyUpdated;
            }
        }

        public bool SetEventToExpired(int eId)
        {
            var isSuccessfulEventUpdate = false;

            try
            {
                if (IsEventExpired(eId) == true)
                {
                    using (var ctx = new GreetNGroupContext())
                    {
                        var eventToUpdate = ctx.Events.FirstOrDefault(e => e.EventId.Equals(eId));
                        eventToUpdate.IsEventExpired = true;
                        ctx.SaveChanges();
                    }
                }
                else
                {
                    return isSuccessfulEventUpdate;
                }
                return isSuccessfulEventUpdate;
            }
            catch (Exception e)
            {
                _gngLoggerService.LogGNGInternalErrors(e.ToString());
                return isSuccessfulEventUpdate;
            }
        }

        #endregion
        
        /// <summary>
        /// The following region handles Event information deletion
        /// </summary>
        #region Delete Event Information

        public bool DeleteEvent(int eId, int userId, string ip)
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
                LogGNGEventDeleted(userId.ToString(), eId, ip);
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

        public bool IsEventExpired(int eId)
        {
            var isExpired = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var eventToCheck = ctx.Events.FirstOrDefault(e => e.EventId.Equals(eventId));
                    var eventStartTime = eventToCheck.StartDate;
                    if ((DateTime.UtcNow - eventStartTime).TotalMinutes > configurations.GetMaxEventUptime())
                    {
                        isExpired = true;
                    }
                }
                return isExpired;
            }
            catch (Exception e)
            {
                _gngLoggerService.LogGNGInternalErrors(e.ToString());
                return isExpired;
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

        // Retrieves plain event details in list format give partial event name/search input
        public List<DefaultEventSearchDto> GetPlainEventDetailListByName(string searchInput)
        {
            var e = new List<DefaultEventSearchDto>();
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Grabs events by needed columns and returns data transfer object
                    e = ctx.Events.Where(n => n.EventName.Contains(searchInput))
                        .Select(n => new DefaultEventSearchDto()
                        {
                            Uid = n.UserId,
                            EventName = n.EventName,
                            EventLocation = n.EventLocation,
                            StartDate = n.StartDate
                        }).ToList();
                    return e;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return e;
            }
        }

        // Retrieves plain event details in list format given event id
        public List<DefaultEventSearchDto> GetPlainEventDetailListById(int eId)
        {
            var e = new List<DefaultEventSearchDto>();
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    e = ctx.Events.Where(c => c.EventId.Equals(eId))
                        .Select(c => new DefaultEventSearchDto()
                        {
                            Uid = c.UserId,
                            EventName = c.EventName,
                            EventLocation = c.EventLocation,
                            StartDate = c.StartDate
                        }).ToList();
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
        public Event GetEventByName(string searchInput)
        {
            Event foundEvent = null;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    foundEvent = ctx.Events.FirstOrDefault(c => c.EventName.Equals(searchInput));
                    return foundEvent;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return foundEvent;
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

        // Filters list of given event information by removing passed dates
        public List<DefaultEventSearchDto> FilterOutPastEvents(List<DefaultEventSearchDto> unfiltered)
        {
            var filtered = new List<DefaultEventSearchDto>();
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

        #endregion

        /// <summary>
        /// The following region performs logging specific to event functions
        /// </summary>
        #region Logging Functions

        /// <summary>
        /// Method LogGNGEventsCreated logs the events users made on GreetNGroup. The event ID
        /// and user ID of the host will be tracked. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">user ID</param>
        /// <param name="eventID">Event ID</param>
        /// <param name="ip">IP Address of user</param>
        /// <returns>Return true or false if the log was made successfully</returns>
        public bool LogGNGEventsCreated(string usersID, int eventID, string ip)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            _gngLoggerService.CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "EventCreated",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "Event " + eventID + " created"
            };

            var logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogGNGEventUpdate logs when a user updates their GNG event info
        /// </summary>
        /// <param name="eventId">Event id of the event</param>
        /// <param name="userHostId">User id of the host</param>
        /// <param name="ip">IP address of the host</param>
        /// <returns>Returns a bool based on if the log was successfully made or not</returns>
        public bool LogGNGEventUpdate(int eventId, string userHostId, string ip)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            _gngLoggerService.CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "EventUpdated",
                UserID = userHostId,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "Event " + eventId + " updated"
            };

            var logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogGNGEventDeleted logs when a user deletes their GNG event
        /// </summary>
        /// <param name="hostId">User ID of the host</param>
        /// <param name="eventId">Event ID of the event being deleted</param>
        /// <param name="ip">IP Address of the host</param>
        /// <returns>Returns a bool based on if it was logged successfully</returns>
        public bool LogGNGEventDeleted(string hostId, int eventId, string ip)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            _gngLoggerService.CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "EventDeleted",
                UserID = hostId,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "Event " + eventId + " deleted"
            };

            var logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogGNGEventExpiration logs when an event has passed and can no longer
        /// be joined
        /// </summary>
        /// <param name="hostId">User ID of the host who created the event</param>
        /// <param name="eventId">Event ID of the event that expired</param>
        /// <returns>Returns a bool based on if it was logged successfully or not</returns>
        public bool LogGNGEventExpiration(string hostId, int eventId)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            _gngLoggerService.CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "EventExpired",
                UserID = hostId,
                IpAddress = "N/A",
                DateTime = DateTime.Now.ToString(),
                Description = "Event " + eventId + " expired"
            };

            var logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogGNGSearchAction logs when a user searches for another user or event. The log
        /// tracks the search entry the user made. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">user ID</param>
        /// <param name="searchedItem">Search entry</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if the log was successfully made</returns>
        public bool LogGNGSearchAction(string usersID, string searchedItem, string ip)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            _gngLoggerService.CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "SearchAction",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "User searched for " + searchedItem
            };

            var logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }
        #endregion
    }



}
