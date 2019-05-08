using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gucci.ManagerLayer.UADManagement;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;

namespace WebApi.Controllers
{
    public class UADController : ApiController
    {
        private ILoggerService _gngLoggerService = new LoggerService();
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
            catch (HttpRequestException error) // Catch Logger Errors
            {
                _gngLoggerService.LogBadRequest("", "", url, error.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/LoginSuccessFail/{month}/{year}")]
        public IHttpActionResult GetLoginSuccessFail(string month, int year)
        {
            UADManager _uadManager = new UADManager();
            try
            {
                var result = _uadManager.GetLoginSuccessFail(month, year);
                return Ok(result);
            }
            catch (HttpRequestException error) // Catch Logger Errors
            {
                _gngLoggerService.LogBadRequest("", "", url, error.ToString());
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
                _gngLoggerService.LogBadRequest("", "", url, error.ToString());
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/UAD/Top5MostUsedFeature/{month}/{year}")]
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
                _gngLoggerService.LogBadRequest("", "", url, error.ToString());
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
                _gngLoggerService.LogBadRequest("", "", url, error.ToString());
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
                _gngLoggerService.LogBadRequest("", "", url, error.ToString());
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
                _gngLoggerService.LogBadRequest("", "", url, error.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/logclicks")]
        public IHttpActionResult TrackUserActivity([FromBody] LogActivityRequest request)
        {
            var userService = new UserService();
            var jwtService = new JWTService();

            try
            {
                if (request.Jwt == null || request.Jwt.Equals(""))
                {
                    userService.LogClicksMade(request.StartPoint, request.EndPoint,
                    "N/A", request.Ip);

                    return Content(HttpStatusCode.Accepted, true);
                }
                else
                {
                    userService.LogClicksMade(request.StartPoint, request.EndPoint,
                    jwtService.GetUserIDFromToken(request.Jwt).ToString(), request.Ip);

                    return Content(HttpStatusCode.Accepted, true);
                }

            }
            catch (Exception e)
            {
                if (request.Jwt == null)
                {
                    _gngLoggerService.LogBadRequest("N/A", request.Ip, request.StartPoint, e.ToString());
                }
                else
                {
                    _gngLoggerService.LogBadRequest(jwtService.GetUserIDFromToken(request.Jwt).ToString(), request.Ip, request.StartPoint, e.ToString());
                }
                return Content(HttpStatusCode.BadRequest, "Failed to log user activity");
            }
        }
    }
}