using Gucci.DataAccessLayer.Models;
using ServiceLayer.Interface;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;

namespace ManagerLayer.AttendeeManagement
{
    class AttendeeManager
    {
        private AttendeesService _attendeeService = new AttendeesService();
        public AttendeeManager()
        {
        }
        /// <summary>
        /// Function that inserts a attendee into the attendance table
        /// </summary>
        /// <param name="eventId">Event id attendee is joining</param>
        /// <param name="userId">User id of the attendee</param>
        /// <returns>A string based on what happended</returns>
        public string JoinEvent(int eventId, int userId)
        {
            var eventJoinedMessage = "Joined Event";

            var isEventandUserValid = _attendeeService.DoesEventandUserExist(eventId, userId);
            if(isEventandUserValid == false)
            {
                return eventJoinedMessage = "Event or User does not exist";
            }
            var doesAttendeeExist = _attendeeService.DoesAttendeeExist(eventId, userId);
            if(doesAttendeeExist == true)
            {
                return eventJoinedMessage = "You have already joined the event";
            }
            var attendeeJoined = _attendeeService.InsertAttendee(eventId, userId);
            if(attendeeJoined == false)
            {
                return eventJoinedMessage = "Uh Oh something went wrong";
            }
            return eventJoinedMessage;
        }
        /// <summary>
        /// Function that deletes a attendee from the attendance table
        /// </summary>
        /// <param name="eventId">Event id attendee is joining</param>
        /// <param name="userId">User id of the attendee</param>
        /// <returns>A string based on what happended</returns>
        public string LeaveEvent(int eventId, int userId)
        {
            var eventLeftMessage = "Left Event";

            var isEventandUserValid = _attendeeService.DoesEventandUserExist(eventId, userId);
            if (isEventandUserValid == false)
            {
                return eventLeftMessage = "Event or User does not exist";
            }
            var doesAttendeeExist = _attendeeService.DoesAttendeeExist(eventId, userId);
            if (doesAttendeeExist == false)
            {
                return eventLeftMessage = "You have not joined the event";
            }
            var attendeeLeft = _attendeeService.DeleteAttendee(eventId, userId);
            if (attendeeLeft == false)
            {
                return eventLeftMessage = "Uh Oh something went wrong";
            }
            return eventLeftMessage;
        }
    }
}
