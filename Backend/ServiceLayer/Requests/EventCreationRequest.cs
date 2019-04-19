using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.Requests
{
    public class EventCreationRequest
    {
        [Required]
        public int userId { get; set; }
        [Required]
        public DateTime startDate { get; set; }
        [Required]
        public string eventName { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string state { get; set; }
        [Required]
        public string zip { get; set; }
        [Required]
        public List<string> eventTags { get; set; }
        public string eventDescription { get; set; }
        [Required]
        public string ip { get; set; }
        [Required]
        public string url { get; set; }

    }
}
