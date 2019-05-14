using System.Linq;
using Gucci.DataAccessLayer.Context;

namespace Gucci.ServiceLayer.Services
{
    public class CheckInService
    {
        // Check attendees list of current event for current user
        public bool CheckAttendanceList(int eventId, int userId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                // Find if username exists within corresponding event attendee table
                var retrievedAttendee = ctx.Attendees.Where(a => a.EventId == eventId && a.UserId == userId);
                if(retrievedAttendee != null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
