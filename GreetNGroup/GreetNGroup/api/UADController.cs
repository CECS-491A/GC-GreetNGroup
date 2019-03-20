using GreetNGroup.UAD;
using System.Net.Http;
using System.Web.Http;

namespace GreetNGroup.api
{
    
    public class UADController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetEventById(int id)
        {
            string a = "Nope";
            try
            {
                if(id == 1)
                {
                    a = UserAnalysisDashboard.LoginVSRegistered("March");
                }
                if (id == 2)
                {
                    a = UserAnalysisDashboard.LoggedInMonthly(); ;
                }
                if (id == 3)
                {
                    a = UserAnalysisDashboard.Top5MostUsedFeature("March") ;
                }
                if (id == 4)
                {
                    a = UserAnalysisDashboard.AverageSessionDuration("March");
                }
                if (id == 5)
                {
                    a = UserAnalysisDashboard.AverageSessionMonthly("March");
                }
                if (id == 6)
                {
                    a = UserAnalysisDashboard.Top5AveragePageSession("March");
                }

                return Ok(a);
            }
            catch (HttpRequestException e)
            {
                // Add logging
                return BadRequest();
            }
        }

        
    }
}