using System.Net.Http;
using System.Web.Http;
using Gucci.ManagerLayer.UADManagement;
using Gucci.ManagerLayer.LogManagement;

namespace WebApi.Controllers
{
    public class UADController : ApiController
    {
        private LogManager gngLogManager = new LogManager();
        private const string url = "https://www.greetngroup.com/analysisdashboard";

        [HttpGet]
        [Route("api/UAD/LoginVSRegistered/{month}/{year}")]
        public IHttpActionResult GetLoginvsRegistered(string month, int year)
        {
            UADManager _uadManager = new UADManager();
            try
            {
                var result = _uadManager.GetLoginComparedToRegistered(month, year);
                return Ok(result);
            }
            catch (HttpRequestException error)
            {
                //gngLogManager.LogBadRequest("", "", url, e.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/AverageSessionDuration/{month}/{year}")]
        public IHttpActionResult GetAverageSessionDuration(string month, int year)
        {
            UADManager _uadManager = new UADManager();
            try
            {
                var result = _uadManager.GetAverageSessionDuration(month, year);
                return Ok(result);
            }
            catch (HttpRequestException error)
            {
                //gngLogManager.LogBadRequest("", "", url, error.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/GetTop5MostUsedFeature/{month}/{year}")]
        public IHttpActionResult GetTop5MostUsedFeature(string month, int year)
        {
            UADManager _uadManager = new UADManager();
            try
            {
                var result = _uadManager.GetTop5MostUsedFeature(month, year);
                return Ok(result);
            }
            catch (HttpRequestException error)
            {
                //gngLogManager.LogBadRequest("", "", url, error.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/Top5AveragePageSession/{month}/{year}")]
        public IHttpActionResult GetTop5AveragePageSession(string month, int year)
        {
            UADManager _uadManager = new UADManager();
            try
            {
                var result = _uadManager.GetTop5AveragePageSession(month, year);
                return Ok(result);
            }
            catch (HttpRequestException error)
            {
                //gngLogManager.LogBadRequest("", "", url, error.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/LoggedInMonthly/{month}/{year}")]
        public IHttpActionResult GetLoggedInMonthly(string month, int year)
        {
            UADManager _uadManager = new UADManager();
            try
            {
                var result = _uadManager.GetLoggedInMonthly(month, year);
                return Ok(result);
            }
            catch (HttpRequestException error)
            {
                //gngLogManager.LogBadRequest("", "", url, error.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/AverageSessionMonthly/{month}/{year}")]
        public IHttpActionResult GetAverageSessionMonthly(string month, int year)
        {
            UADManager _uadManager = new UADManager();
            try
            {
                var result = _uadManager.GetAverageSessionMonthly(month, year);
                return Ok(result);
            }
            catch (HttpRequestException error)
            {
                //gngLogManager.LogBadRequest("", "", url, error.ToString());
                return BadRequest();
            }
        }

    }
}