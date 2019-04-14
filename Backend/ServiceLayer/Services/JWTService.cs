using DataAccessLayer.Tables;
using DataAccessLayer.Context;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using ServiceLayer.Interface;

namespace ServiceLayer.Services
{
    public class JWTService : IJWTService
    {
        private readonly string symmetricKeyFinal = Environment.GetEnvironmentVariable("symmetricKey", EnvironmentVariableTarget.User);
        private SigningCredentials credentials;
        private ICryptoService _cryptoService;
        private JwtSecurityTokenHandler tokenHandler;

        public JWTService()
        {
            _cryptoService = new CryptoService();
            credentials = _cryptoService.GenerateJWTSignature();
            tokenHandler = new JwtSecurityTokenHandler();
        }

        public string CreateToken(string username, string hashedUID)
        {
            var usersClaims = RetrieveClaims(username);
            var securityClaimsList = new List<System.Security.Claims.Claim>();

            //Takes the claims the user has and puts it in a list of Security Claims objects
            foreach (DataAccessLayer.Tables.Claim c in usersClaims)
            {
                securityClaimsList.Add(new System.Security.Claims.Claim(c.ClaimName, hashedUID));
            }

            var jwt = new JwtSecurityToken(
                issuer: "greetngroup.com",
                audience: "greetngroup.com",
                claims: securityClaimsList,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return tokenHandler.WriteToken(jwt);
        }

        public string ValidateAndUpdateToken(string jwtToken)
        {
            if (!IsJWTSignatureTampered(jwtToken))
            {
                return RefreshToken(jwtToken);
            }
            return "";
        }

        public string RefreshToken(string jwtToken)
        {
            var oldJWT = tokenHandler.ReadToken(jwtToken) as JwtSecurityToken;
            var newJWT = new JwtSecurityToken(
                issuer: oldJWT.Issuer,
                audience: oldJWT.Audiences.ToString(),
                claims: oldJWT.Claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);
            return tokenHandler.WriteToken(newJWT);
        }

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
            if (IsJWTSignatureTampered(userJwtToken) == false)
            {
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
            else
            {
                return false;
            }
        }

        public int GetUserIDFromToken(string jwtToken)
        {
            var jwt = tokenHandler.ReadToken(jwtToken) as JwtSecurityToken;
            var usersCurrClaims = jwt.Claims;
            var userID = usersCurrClaims.First().Value;
            try
            {
                return Convert.ToInt32(userID);
            }
            catch (FormatException)
            {
                //log
                return -1;
            }
        }

        /// <summary>
        /// Method IsJWTSignatureTampered checks if the user has altered the signature
        /// of their JWT in attempt to access more than the user should. If the user's
        /// signature of their JWT does not match the signature that is in GreetNGroup
        /// then the method returns true.
        /// </summary>
        /// <param name="userJwtToken">users JWT in string format</param>
        /// <returns>Returns bool value based on if the user has tampered with their
        /// JWT</returns>
        public bool IsJWTSignatureTampered(string userJwtToken)
        {
            var jwt = tokenHandler.ReadToken(userJwtToken) as JwtSecurityToken;
            bool isTampered = false;
            SigningCredentials signature = jwt.SigningCredentials;
            if (!credentials.ToString().Equals(signature.ToString()))
            {
                isTampered = true;
            }
            return isTampered;
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
