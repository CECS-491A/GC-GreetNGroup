using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer
{
    public class EventManager
    {
        private CheckInService _checkInService;
        private AttendeesService _attendeeService;
        public EventManager()
        {
            _checkInService = new CheckInService();
            _attendeeService = new AttendeesService();
        }

        public HttpResponseMessage CheckIn(int eventID, int userID, string inputtedCheckInCode)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // perform search for current event and compares EventCheckInCode with input
                    var retrievedEvent = ctx.Events.First(events => events.EventId.Equals(eventID));
                    var verified = retrievedEvent.EventCheckinCode.Equals(inputtedCheckInCode);

                    if (!verified)
                    {
                        var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("Invalid Checkin Code")
                        };
                        return httpResponseFail;
                    }


                    var updatedAttendee = new Attendance(eventID, userID, true);

                    var response = _attendeeService.UpdateAttendance(updatedAttendee);

                    if (response == null)
                    {
                        var httpResponseFail = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent("Unable to check in")
                        };
                        return httpResponseFail;
                    }

                    var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("You have successfully checked in!")
                    };
                    return httpResponse;
                }
            }
            catch(Exception e)
            {
                //log
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.ToString())
                };
                return httpResponse;
            }
        }

        public HttpResponseMessage IsAttendee(int eventId, int userId)
        {
            try
            {
                var IsAttending = _attendeeService.DoesAttendeeExist(eventId, userId);
                if (IsAttending)
                {
                    var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("true")
                    };
                    return httpResponse;
                }
                var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("false")
                };
                return httpResponseFail;
            }
            catch
            {
                //log
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return httpResponse;
            }
        }
    }
}
