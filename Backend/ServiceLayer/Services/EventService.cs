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
        ICryptoService _cryptoService;

        public EventService()
        {
            _cryptoService = new CryptoService();
        }

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
                // log
                return false;
            }
        }

        public bool InsertEvent(string userId, int eventId, DateTime startDate, string eventName, 
            string address, string city, string state, string zip, List<string> eventTags, string eventType)
        {
            bool isSuccessfulAdd = false;
            int sequentialId = RetrieveUsersSequentialId(userId);
            if (IsUserAtMaxEventCreation(sequentialId) == false && sequentialId != -1)
            {
                try
                {
                    string eventLocation = ParseAddress(address, city, state, zip);

                    using (var ctx = new GreetNGroupContext())
                    {
                        var userEvent = new Event(sequentialId, eventId, startDate, eventName, eventLocation);

                        ctx.Events.Add(userEvent);

                        ctx.SaveChanges();
                        isSuccessfulAdd = true;
                    }
                    return isSuccessfulAdd;
                }
                catch (ObjectDisposedException od)
                {
                    // log
                    return isSuccessfulAdd;
                }
            }
            else
            {
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
                // log
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
                // log
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
                // log
                return isSuccessfullyUpdated;
            }
        }

        #endregion
        
        /// <summary>
        /// The following region handles Event information deletion
        /// </summary>
        #region Delete Event Information

        public bool DeleteEvent(string eId)
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
                // log
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
                // log
                return false;
            }
        }

        #endregion

        /// <summary>
        /// The following region retrieves event information from the database
        /// </summary>
        #region Event Information Retrieval

        public Event GetEventById(int eventId)
        {
            Event e = null;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    e = ctx.Events.FirstOrDefault(c => c.EventId.Equals(eventId));
                    return e;
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
                return e;
            }
        }

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
                // log
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
                //log
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
        /// Method RetrieveUsersSequentialId finds the users sequential id based on their
        /// hashed id. Since SHA256 is a one-way encryption algorithm as of 2019.4.5, it is currently
        /// not possible to decrypt such a hash and the only way to find the sequential id is to
        /// compare hashes, which will take a hit in terms of runtime.
        /// </summary>
        /// <param name="hashedId">User's hashed id</param>
        /// <returns>Returns integer value which is the user's sequential id</returns>
        public int RetrieveUsersSequentialId(string hashedId)
        {
            int usersId = -1;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var userList = new List<User>();
                    var usersInDB = ctx.Users;

                    foreach (User u in usersInDB)
                    {
                        var uId = u.UserId.ToString();
                        var hashedUId = _cryptoService.HashSha256(uId);
                        if (hashedId.Equals(hashedUId))
                        {
                            usersId = u.UserId;
                        }
                    }

                }
                return usersId;
            }
            catch (ObjectDisposedException od)
            {
                //log
                return usersId;
            }

        }

        /// <summary>
        /// Method EventIDGenerator references the answer given in
        /// <https://stackoverflow.com/questions/15009423/way-to-generate-a-unique-number-that-does-not-repeat-in-a-reasonable-time>
        /// We are using the answer provided by Patrick & Aaron Anodide because it will generate a unique ID
        /// every time the method is called so long as it is not called multiple times at the exact same
        /// tick. The answer provided allows the use of 26 alphabet characters and numbers 0-9 in the ID generator.
        /// Since ticks are represented 64-bit number, the bytes must be converted to a Base 64 string
        /// as not doing so will result in a lengthy ID. Since the original answer resulted in IDs with
        /// some unique characters, the answer is slightly modified in this implementation to remove those
        /// characters from the ID.
        /// </summary>
        /// <returns>Event id in string form</returns>
        public string EventIDGenerator()
        {
            //Characters that appear in the byte sequence
            char[] replace = new char[] { '+', '-', '/', '=' };
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string id = Convert.ToBase64String(bytes);

            foreach (char c in replace)
            {
                id = id.Replace(c.ToString(), String.Empty);
            }
            return id;
        }

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
        #endregion
    }



}
