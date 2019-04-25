using Gucci.DataAccessLayer.Context;
using Gucci.ServiceLayer.Services;
using System.Collections.Generic;
using System.Linq;

namespace Gucci.ManagerLayer.AttendeeManagement
{
    public class AttendeeManager
    {
        private AttendeesService _attendeeService;
        private JWTService _jWTService;
        private readonly List<string> joinEventClaims = new List<string> ();
        GreetNGroupContext ctx = new GreetNGroupContext();
        public AttendeeManager()
        {
            _attendeeService = new AttendeesService();
            _jWTService = new JWTService();
            joinEventClaims.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(9)).ClaimName);
        }
        /// <summary>
        /// Function that inserts a attendee into the attendance table
        /// </summary>
        /// <param name="eventId">Event id attendee is joining</param>
        /// <param name="userId">User id of the attendee</param>
        /// <returns>A string based on what happended</returns>
        public string JoinEvent(int eventId, string jwt)
        {
            var eventJoinedMessage = "Joined Event";
            var userID = _jWTService.GetUserIDFromToken(jwt);
            var hasClaims = _jWTService.CheckUserClaims(jwt, joinEventClaims);
            if(hasClaims)
            {
                var isEventandUserValid = _attendeeService.DoesEventandUserExist(eventId, userID);
                if (isEventandUserValid == false)
                {
                    return eventJoinedMessage = "Event or User does not exist";
                }
                var doesAttendeeExist = _attendeeService.DoesAttendeeExist(eventId, userID);
                if (doesAttendeeExist)
                {
                    return eventJoinedMessage = "You have already joined the event";
                }
                var attendeeJoined = _attendeeService.InsertAttendee(eventId, userID);
                if (attendeeJoined == false)
                {
                    return eventJoinedMessage = "Uh Oh something went wrong";
                }
            }
            else
            {
                eventJoinedMessage = "Sorry you are not allowed to join events";
            }
            return eventJoinedMessage;
        }
        /// <summary>
        /// Function that deletes a attendee from the attendance table
        /// </summary>
        /// <param name="eventId">Event id attendee is joining</param>
        /// <param name="userId">User id of the attendee</param>
        /// <returns>A string based on what happended</returns>
        public string LeaveEvent(int eventId, string jwt)
        {
            var eventLeftMessage = "Left Event";
            var userID = _jWTService.GetUserIDFromToken(jwt);
            var isEventandUserValid = _attendeeService.DoesEventandUserExist(eventId, userID);
            if (isEventandUserValid == false)
            {
                return eventLeftMessage = "Event or User does not exist";
            }
            var doesAttendeeExist = _attendeeService.DoesAttendeeExist(eventId, userID);
            if (doesAttendeeExist == false)
            {
                return eventLeftMessage = "You have not joined the event";
            }
            var attendeeLeft = _attendeeService.DeleteAttendee(eventId, userID);
            if (attendeeLeft == false)
            {
                return eventLeftMessage = "Uh Oh something went wrong";
            }
            return eventLeftMessage;
        }
    }
}
