using System.Net.Http;
using System.Web.Http;
using ManagerLayer.UADManagement;
using ManagerLayer.GNGLogManagement;
namespace WebApi.Controllers
{
    public class UADController : ApiController
    {

        private GNGLogManager gngLogManager = new GNGLogManager();
        private const string url = "https://www.greetngroup.com/analysisdashboard";

        [HttpGet]
        [Route("api/UAD/LoginVSRegistered/{month}")]
        public IHttpActionResult GetLoginvsRegistered(string month)
        {
            try
            {
                UADManager _uadManager = new UADManager();
                var result = _uadManager.GetLoginVSRegistered(month);
                return Ok(result);
            }
            catch(HttpRequestException e)
            {
                gngLogManager.LogBadRequest("", "", url, e.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/AverageSessionDuration/{month}")]
        public IHttpActionResult GetAverageSessionDuration(string month)
        {
            try
            {
                UADManager _uadManager = new UADManager();
                var result = _uadManager.GetAverageSessionDuration(month);
                return Ok(result);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest("", "", url, e.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/GetTop5MostUsedFeature/{month}")]
        public IHttpActionResult GetTop5MostUsedFeature(string month)
        {
            try
            {
                UADManager _uadManager = new UADManager();
                var result = _uadManager.GetTop5MostUsedFeature(month);
                return Ok(result);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest("", "", url, e.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/Top5AveragePageSession/{month}")]
        public IHttpActionResult GetTop5AveragePageSession(string month)
        {
            try
            {
                UADManager _uadManager = new UADManager();
                var result = _uadManager.GetTop5AveragePageSession(month);
                return Ok(result);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest("", "", url, e.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/LoggedInMonthly/{month}")]
        public IHttpActionResult GetLoggedInMonthly(string month)
        {
            try
            {
                UADManager _uadManager = new UADManager();
                var result = _uadManager.GetLoggedInMonthly(month);
                return Ok(result);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest("", "", url, e.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/AverageSessionMonthly/{month}")]
        public IHttpActionResult GetAverageSessionMonthly(string month)
        {
            try
            {
                UADManager _uadManager = new UADManager();
                var result = _uadManager.GetAverageSessionMonthly(month);
                return Ok(result);
            }
            catch (HttpRequestException e)
            {
                gngLogManager.LogBadRequest("", "", url, e.ToString());
                return BadRequest();
            }
        }


    }
}