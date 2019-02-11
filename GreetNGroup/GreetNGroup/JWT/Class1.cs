using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Mvc;
using System.Web.Http;
using AllowAnonymousAttribute = System.Web.Http.AllowAnonymousAttribute;
using GreetNGroup.Tokens;
using GreetNGroup.Data_Access;

namespace GreetNGroup.JWT
{
    public class JWTController
    {
        /// <summary>
        /// This is where a user 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RequestToken([FromBody] Token token)
        {
            if(token.UserId == DataBaseQueries.
        }
    }
}