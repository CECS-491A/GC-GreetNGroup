using System.Collections.Generic;

namespace Gucci.ServiceLayer.Requests
{
    public class ClaimCheckRequest
    {
        public string JWT { get; }
        public List<string> ClaimsToCheck { get; }
        public string Ip { get; }
        public string UrlToEnter { get; }
    }
}
