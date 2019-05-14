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
        public EventManager()
        {
            _checkInService = new CheckInService();
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

                    var retrievedAttendee = ctx.Attendees.Where(a => a.EventId == eventID)
                                                         .Where(a => a.UserId == userID).FirstOrDefault();

                    if (retrievedAttendee == null)
                    {
                        var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("Unable to check in")
                        };
                        return httpResponseFail;
                    }

                    retrievedAttendee.CheckedIn = true;
                    ctx.SaveChanges();

                    var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("You have successfully checked in!")
                    };
                    return httpResponse;
                }
            }
            catch
            {
                //log
                var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return httpResponse;
            }
        }

        public HttpResponseMessage IsAttendee(int eventId, int userId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    // Find if username exists within corresponding event attendee table
                    var retrievedAttendee = ctx.Attendees.Where(a => a.EventId == eventId)
                                                         .Where(a => a.UserId == userId)
                                                         .FirstOrDefault();
                    if (retrievedAttendee == null)
                    {
                        var httpResponseFail = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("false")
                        };
                        return httpResponseFail;
                    }
                    var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("true")
                    };
                    return httpResponse;
                }
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
