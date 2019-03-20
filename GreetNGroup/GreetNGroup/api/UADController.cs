using System.Web.Http;

namespace GreetNGroup.api
{
    [Route("api/UAD")]
    public class UADController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            return "This is the analysis dashboard you have clearance to view";
        }
    }
}