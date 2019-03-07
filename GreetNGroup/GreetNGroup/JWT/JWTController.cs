using System;
using System.Security.Claims;
using System.Web.Http;
using AllowAnonymousAttribute = System.Web.Http.AllowAnonymousAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using GreetNGroup.Data_Access;

namespace GreetNGroup.JWT
{
    public class JWTController
    {

        public string FetchUserID(string userInputUsername)
        {
            string userID = DataBaseQueries.CheckIfUserExists(userInputUsername);
            return userID;
        }

        public string FetchUserPassword(string userID)
        {
            string userPassword = DataBaseQueries.CurrentPassword(userID);
            return userPassword;
        }

        public List<string> FetchUsersClaims(string userID)
        {
            List<string> userClaims = DataBaseQueries.ListUserClaims(userID);
            return userClaims;
        }


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
        public bool RequestToken(string userInputUsername, string userInputPassword)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            string userID = FetchUserID(userInputUsername);
            string userPassword = "";

            if(userID != null)
            {
                userPassword = FetchUserPassword(userID);
                if (userInputPassword.Equals(userPassword))
                {
                    var usersClaims = FetchUsersClaims(userID);
                    var claimsList = new List<Claim>();
                    //Takes the claims the user has and puts it in a list of Claims objects
                    foreach(string c in usersClaims)
                    {
                        claimsList.Add(new Claim(c, userInputUsername));
                    }

                    //Generate the symmetric key which will be used for the signature portion of the JWT
                    byte[] symmetricKey = new byte[256 / 8];
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
                    return true;
                }
                //Password does not match so cannot grant a token
                else
                {
                    return false;
                }
            }
            //User does not exist therefore cannot grant token
            else
            {
                return false;
            }
        }
    }
}