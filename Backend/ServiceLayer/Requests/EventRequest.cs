using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gucci.ServiceLayer.Requests
{
    public class EventRequest
    {
        public int EventId { get; set; }
        [Required]
        public string JWT { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public string EventName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public List<string> EventTags { get; set; }
        public string EventDescription { get; set; }
        [Required]
        public string Ip { get; set; }
        [Required]
        public string Url { get; set; }

    }
}
