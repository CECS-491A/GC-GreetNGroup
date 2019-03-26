using System.Web.Http;

namespace GNG_WebApi.Controllers
{
    public class HelloWorldController : ApiController
    {
        [Route("hello")]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}