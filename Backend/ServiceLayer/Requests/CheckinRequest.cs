using System.ComponentModel.DataAnnotations;

namespace Gucci.ServiceLayer.Requests
{
    public class CheckinRequest
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public string CheckinCode { get; set; }
        [Required]
        public string JWT { get; set; }
    }
}
