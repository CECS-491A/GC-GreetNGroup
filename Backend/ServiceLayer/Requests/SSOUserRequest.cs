using System.ComponentModel.DataAnnotations;

namespace Gucci.ServiceLayer.Requests
{
    public class SSOUserRequest
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string signature { get; set; }
        [Required]
        public string ssoUserId { get; set; }
        [Required]
        public string timestamp { get; set; }
    }
}
