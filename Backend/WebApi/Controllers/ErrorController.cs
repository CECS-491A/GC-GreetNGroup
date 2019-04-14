using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ServiceLayer.Interface;
using ServiceLayer.Services;

namespace WebApi.Controllers
{
    public class ErrorController : ApiController
    {
        [HttpPost]
        [Route("api/error/contactsystemadmin")]
        public IHttpActionResult CallSystemAdmin()
        {
            IErrorHandlerService _errorHandlerService = new ErrorHandlerService();

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
            catch(HttpRequestException e)
            {
                return BadRequest();
            }

        }
    }
}