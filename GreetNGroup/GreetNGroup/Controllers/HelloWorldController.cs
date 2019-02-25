using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace GreetNGroup.Controllers
{
    public class HelloWorldController : ApiController
    {
        [Route("/api/hello")]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}