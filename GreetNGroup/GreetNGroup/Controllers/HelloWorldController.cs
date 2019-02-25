using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace GreetNGroup.Controllers
{
    public class HelloWorldController : ApiController
    {
        [HttpGet]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}