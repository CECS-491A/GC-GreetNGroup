using System;
using AllowAnonymousAttribute = System.Web.Http.AllowAnonymousAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Linq;
using GreetNGroup.DataAccess.Queries;
using System.Web.Http;

namespace GreetNGroup.JWT
{
    [Route("api/JWT")]
    public class JWTController : ApiController
    {

        /// <summary>
        /// This is where a user requests a JWT based on the login information they put
        /// and checks if the user has input the proper login information, otherwise they are
        /// not granted a JWT.
        /// </summary>
        /// <param name="userInputUsername">username user has input</param>
        /// <param name="userInputPassword">password user has input</param>
        /// <returns>True for successful JWT generation, false otherwise</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/JWT/grant")]
        public JwtSecurityToken RequestToken(string userInputUsername, string userInputPassword)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            if(DbCheck.IsUsernameFound(userInputUsername) == true &&
                DbCheck.DoesPasswordMatch(userInputUsername, userInputPassword) 
                == true)
            {
                var usersClaims = DbRetrieve.GetUsersClaims(userInputUsername);
                var hashedUID = DbRetrieve.GetUsersHashedUID(userInputUsername);
                var claimsList = new List<System.Security.Claims.Claim>();
                //Takes the claims the user has and puts it in a list of Claims objects
                foreach (GreetNGroup.DataAccess.Tables.Claim c in usersClaims)
                {
                    claimsList.Add(new System.Security.Claims.Claim(c.ClaimName, hashedUID));
                }

                //Generate the symmetric key which will be used for the signature portion of the JWT
                byte[] symmetricKey = new byte[256];
                rng.GetBytes(symmetricKey);
                var charKey = Encoding.UTF8.GetString(symmetricKey);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(charKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var jwt = new JwtSecurityToken(
                    issuer: "greetngroup.com",
                    audience: "greetngroup.com",
                    claims: claimsList,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials);

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.WriteToken(jwt);
                return jwt;

            }
            //User does not exist or user input wrong login information
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method to check if a user has the appropriate claims to access/perform an
        /// action on the app. If they have the claim, return true, otherwise, return
        /// false.
        /// </summary>
        /// <param name="jwt">JWT of the user</param>
        /// <param name="claimToCheck">Required claim needed to perform/access</param>
        /// <returns>True or false depending on if the user has the claim</returns>
        [HttpPost]
        [Route("api/JWT/check")]
        public bool CheckClaimsInToken(JwtSecurityToken jwt, List<string> claimsToCheck)
        {
            var usersCurrClaims = jwt.Claims;
            bool pass = false;

            var usersCurrClaimsNames = new List<string>();
            foreach (System.Security.Claims.Claim claim in usersCurrClaims)
            {
                usersCurrClaimsNames.Add(claim.Type);
            }

            var claimsCheck = claimsToCheck.Except(usersCurrClaimsNames);
            pass = !claimsCheck.Any();

            return pass;
            
        }
    }
}