using System.Net.Http;
using System.Web.Http;
using ServiceLayer.Interface;
using ServiceLayer.Services;
using ManagerLayer.GNGLogManagement;

namespace WebApi.Controllers
{
    public class ErrorController : ApiController
    {
        [HttpPost]
        [Route("api/error/contactsystemadmin")]
        public IHttpActionResult CallSystemAdmin(string userId, string url, string ip)
        {
            IErrorHandlerService _errorHandlerService = new ErrorHandlerService();
            GNGLogManager gngLogManager = new GNGLogManager();

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
                gngLogManager.LogBadRequest(userId, ip, url, e.ToString());
                return BadRequest();
            }

        }
    }
}