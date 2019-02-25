using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GreetNGroup.api
{
    [EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*"), Route("api/hello")]
    public class HelloController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }
    }
}
