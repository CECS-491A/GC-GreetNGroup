using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Requests
{
    public class RateRequest
    {
        [Required]
        public string RaterID { get; set; }
        [Required]
        public string rating { get; set; }
    }
}
