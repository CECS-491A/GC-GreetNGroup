using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gucci.ServiceLayer.Services
{
    
    public class AttendeesService
    {
        private ILoggerService _gngLoggerService;
        private EventService _eventService;
        private UserService _userService;
        public AttendeesService ()
        {
            _gngLoggerService = new LoggerService();
            _eventService = new EventService();
            _userService = new UserService();
        }

        #region Insert into Table
        /// <summary>
        /// Inserts an attendee to the attandee table
        /// </summary>
        /// <param name="attendee">the current attendee</param>
        /// <returns>boolean if the attendee was successfully added or not</returns>
        public bool InsertAttendee(int eventId, int userId)
        {
            try
            {
                Attendance attendee = new Attendance(eventId, userId, false, "fill") ;
                using (var ctx = new GreetNGroupContext())
                {
                    ctx.Attendees.Add(attendee);
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
        #endregion

        #region Delete from Table
        /// <summary>
        /// Removes attendee from table
        /// </summary>
        /// <param name="eventId">event id </param>
        /// <param name="userId">user id</param>
        /// <returns>Removes the specififed attendee from the table</returns>
        public bool DeleteAttendee(int eventId, int userId)
        {
            var isSuccessfullyDeleted = false;

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    Attendance curAttendee = ctx.Attendees.FirstOrDefault(c => c.EventId.Equals(eventId) && c.UserId.Equals(userId));
                    if (curAttendee != null)
                        ctx.Attendees.Remove(curAttendee);
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

        #region Validation
        /// <summary>
        /// Checks to see if user and event exist
        /// </summary>
        /// <param name="eventId">event id </param>
        /// <param name="userId">user id</param>
        /// <returns>a boolean if the event and user exist at the same time</returns>
        public bool DoesEventandUserExist(int eventId, int userId)
        {
            var isValid = false;
            var isEventValid = _eventService.IsEventIdFound(eventId);
            var isUserValid = _userService.IsUsernameFoundById(userId);
            if(isEventValid == true && isUserValid == true)
            {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// Checks to see if attendee aready exist in the table
        /// </summary>
        /// <param name="eventId">event id </param>
        /// <param name="userId">user id</param>
        /// <returns>a boolean if the attendee exist in the list or not</returns>
        public bool DoesAttendeeExist(int eventId, int userId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    return ctx.Attendees.Any(c => c.EventId.Equals(eventId) && c.UserId.Equals(userId));
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
        /// Checks to see if attendee aready exist in the table
        /// </summary>
        /// <param name="eventId">event id </param>
        /// <param name="userId">user id</param>
        /// <returns>a boolean if the attendee exist in the list or not</returns>
        public List<string> GetAttendees(int eventId)
        {
            var attendees = new List<Attendance>();
            var user = new List<User>();
            var names = new List<string>();
            var ctx = new GreetNGroupContext();
            try
            {

                attendees = ctx.Attendees.Where(c => c.EventId.Equals(eventId)).ToList();
                if(attendees.Count != 0)
                {
                    for(int i = 0; i < attendees.Count; i++)
                    {
                       
                        user.Add(_userService.GetUserById(attendees[i].UserId));
                  
                    }
                   
                    for(int i = 0; i < user.Count; i++)
                    {
                        names.Add(user[i].FirstName + " " + user[i].LastName);
                    }
                    return names;
                }
                else
                {
                    return names;
                }

            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return names;
            }
        }
    }
}
