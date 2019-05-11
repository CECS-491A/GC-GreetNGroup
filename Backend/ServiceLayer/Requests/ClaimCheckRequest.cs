using System.Collections.Generic;

namespace Gucci.ServiceLayer.Requests
{
    public class ClaimCheckRequest
    {
        public string JWT { get; set; }
        public List<string> ClaimsToCheck { get; set; }
        public string Ip { get; set; }
        public string UrlToEnter { get; set; }
    }
}
