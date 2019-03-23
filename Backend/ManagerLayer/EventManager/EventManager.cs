using System;

namespace ManagerLayer.EventManager
{
    public class EventManager
    {
        /// <summary>
        /// The following region inserts an event/event details into the event database
        /// </summary>
        #region Insert Event Information

        public static void InsertEvent(int userId, int eventId, DateTime startDate, string eventName)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var userEvent = new Event(userId, eventId, startDate, eventName);

                    ctx.Events.Add(userEvent);

                    ctx.SaveChanges();
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
            }
        }

        #endregion

        /// <summary>
        /// The following region handles update of Event specific information
        /// </summary>
        #region Update Event Information

        public static void UpdateEventStartDate(int eId, DateTime startDate)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    Event curEvent = ctx.Events.FirstOrDefault(c => c.EventId.Equals(eId));
                    if (curEvent != null)
                        curEvent.StartDate = startDate;
                    ctx.SaveChanges();
                }
            }
            catch (ObjectDisposedException od)
            {
                // log
            }
        }

        #endregion
        
        /// <summary>
        /// The following region handles Event information deletion
        /// </summary>
        #region Delete Event Information
        #endregion
        
        /// <summary>
        /// The following region retrieves confirmation of information within the event database
        /// </summary>
        #region Event Information Check

        public static bool IsEventIdFound(int eventId)
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
    }
}
