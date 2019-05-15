using System.Collections.Generic;

namespace Gucci.ServiceLayer.Requests
{
    // This class holds information to be passed using the FromBody tag in the controller
    public class FindEventsForMeRequest
    {
        public bool UseTags { get; set; }
        public bool UseDates { get; set; }
        public bool UseLocation { get; set; }
        public List<string> Tags { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string State { get; set; }
    }
}
