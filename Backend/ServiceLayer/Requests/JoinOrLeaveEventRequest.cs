using System.ComponentModel.DataAnnotations;

namespace Gucci.ServiceLayer.Requests
{
    public class JoinOrLeaveEventRequest
    {
        [Required]
        public string jwtToken { get; set; }
        [Required]
        public int eventID { get; set; }
    }
}
