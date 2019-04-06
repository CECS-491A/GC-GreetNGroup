using System.Web.Http;

namespace WebApi.Controllers
{
    public class HelloWorldController : ApiController
    {
        [HttpGet]
        [Route("api/helloworld/hello")]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}