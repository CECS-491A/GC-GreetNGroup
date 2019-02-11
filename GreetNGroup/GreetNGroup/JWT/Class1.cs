using System;
using System.Security.Claims;
using System.Web.Http;
using AllowAnonymousAttribute = System.Web.Http.AllowAnonymousAttribute;
using GreetNGroup.Tokens;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;

namespace GreetNGroup.JWT
{
    public class JWTController
    {
        string userInputUsername;
        string userInputPassword;
        /// <summary>
        /// This is where a user 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public string RequestToken([FromBody] Token token)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            if(token.userName.Equals(userInputUsername) &&
                token.password.Equals(userInputPassword))
            {
                var usersClaims = token.Claims;

                var claims = new List<Claim>();
                foreach(string c in usersClaims)
                {
                    claims.Add(new Claim(c, token.userName));
                }

                byte[] symmetricKey = new byte[256 / 8];
                rng.GetBytes(symmetricKey);
                var charKey = Encoding.UTF8.GetString(symmetricKey);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(charKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var jwt = new JwtSecurityToken(
                    issuer: "greetngroup.com",
                    audience: "greetngroup.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials);

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                return tokenHandler.WriteToken(jwt);
            }
            else
            {
                return "Invalid login";
            }
        }
    }
}