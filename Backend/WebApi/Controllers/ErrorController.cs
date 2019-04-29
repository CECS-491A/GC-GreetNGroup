using System.Net.Http;
using System.Web.Http;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Services;
using Gucci.ManagerLayer.LogManagement;
using System;

namespace WebApi.Controllers
{
    public class ErrorController : ApiController
    {
        private ILoggerService _gngLoggerService = new LoggerService();

        [HttpPost]
        [Route("api/error/contactsystemadmin")]
        public IHttpActionResult CallSystemAdmin(string userId, string url, string ip)
        {
            IErrorHandlerService _errorHandlerService = new ErrorHandlerService();
            LogManager gngLogManager = new LogManager();

            try
            {
                if (_errorHandlerService.IsErrorCounterAtMax() == true)
                {
                    var errorMessage = _errorHandlerService.ContactSystemAdmin();
                    return Ok(errorMessage);
                }
                else
                {
                    return Ok();
                }
            }
            catch(Exception e)
            {
                _gngLoggerService.LogBadRequest(userId, ip, url, e.ToString());
                return BadRequest();
            }

        }
    }
}