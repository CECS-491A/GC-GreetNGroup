using Gucci.ManagerLayer.AttendeeManagement;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace WebApi.Controllers
{
    public class JoinOrLeaveEventController : ApiController
    {
        private AttendeeManager attendeeManager = new AttendeeManager();
        private EventService eventService = new EventService();
        private ILoggerService _gngLoggerService = new LoggerService();
        private JWTService _jWTService = new JWTService();
        [HttpPost]
        [Route("api/joinevent")]
        public IHttpActionResult JoinEvent([FromBody] JoinOrLeaveEventRequest request)
        {
            try
            {
                var result = attendeeManager.JoinEvent(request.eventID, request.jwtToken);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.Conflict, "Event Join Failed");
                }
            }
            catch (HttpRequestException e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/leaveevent")]
        public IHttpActionResult LeaveEvent([FromBody] JoinOrLeaveEventRequest request)
        {
            try
            {
                var result = attendeeManager.LeaveEvent(request.eventID, request.jwtToken);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.Conflict, "Event Join Failed");
                }
            }
            catch (HttpRequestException e)
            {
                return BadRequest();
            }
        }
    }
}