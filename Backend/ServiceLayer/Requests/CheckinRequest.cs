using System.ComponentModel.DataAnnotations;

namespace Gucci.ServiceLayer.Requests
{
    public class CheckinRequest
    {
        public int EventId { get; set; }
        public string CheckinCode { get; set; }
        public string JWT { get; set; }
    }
}
