using DataAccessLayer.Tables;
using DataAccessLayer.Context;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ServiceLayer.Services
{
    public class JWTService : IJWTService
    {
        public string CreateToken(string username, string hashedUID)
        {
            var usersClaims = RetrieveClaims(username);
            var securityClaimsList = new List<System.Security.Claims.Claim>();

            //Takes the claims the user has and puts it in a list of Security Claims objects
            foreach (DataAccessLayer.Tables.Claim c in usersClaims)
            {
                securityClaimsList.Add(new System.Security.Claims.Claim(c.ClaimName, hashedUID));
            }
            
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            //Generate the symmetric key which will be used for the signature portion of the JWT
            byte[] symmetricKey = new byte[256];
            rng.GetBytes(symmetricKey);
            var charKey = Encoding.UTF8.GetString(symmetricKey);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(charKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: "greetngroup.com",
                audience: "greetngroup.com",
                claims: securityClaimsList,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(jwt);
        }

        //public string UpdateToken(string jwtToken)
        //{

        //}


        //TODO: modify CheckUserClaims to check signature before getting claims
        //TODO: make the symmetric key an env variable

        /// <summary>
        /// Method to check if a user has the appropriate claims to access/perform an
        /// action on the app. If they have the claim, return true, otherwise, return
        /// false.
        /// </summary>
        /// <param name="jwt">JWT of the user</param>
        /// <param name="claimToCheck">Required claim needed to perform/access</param>
        /// <returns>True or false depending on if the user has the claim</returns>
        public bool CheckUserClaims(string userJwtToken, List<string> claimsToCheck)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadToken(userJwtToken) as JwtSecurityToken;
            
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

        /// <summary>
        /// Method RetrieveClaims gets the claims a user has and returns them in list form
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <returns>Returns that user's list of claims</returns>
        public List<DataAccessLayer.Tables.Claim> RetrieveClaims(string username)
        {
            var claimsList = new List<DataAccessLayer.Tables.Claim>();

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var usersClaims = ctx.UserClaims.Where(c => c.User.UserName.Equals(username));
                    foreach (UserClaim claim in usersClaims)
                    {
                        claimsList.Add(claim.Claim);
                    }

                    return claimsList;
                }
            }
            catch (ObjectDisposedException e)
            {
                // log
                return claimsList;
            }

        }

    }
}
