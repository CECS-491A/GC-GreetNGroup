using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
