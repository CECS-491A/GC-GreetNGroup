using System;
using System.Collections.Generic;

namespace Gucci.ServiceLayer.Requests
{
    public class FindEventsForMeRequest
    {
        public bool UseTags { get; set; }
        public bool UseDates { get; set; }
        public bool UseLocation { get; set; }
        public List<string> Tags { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string State { get; set; }
    }
}
