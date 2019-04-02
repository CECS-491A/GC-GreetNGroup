using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNG_WebApi.Requests
{
    public class LoginRequest
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
