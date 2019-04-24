using System;
using System.Collections.Generic;

namespace ServiceLayer.Requests
{
    class FindEventRequest
    {
        public List<string> EventTags { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string State { get; set; }
    }
}
