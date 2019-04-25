using Gucci.DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Gucci.ServiceLayer.Interface;
using Microsoft.IdentityModel.Tokens;

namespace Gucci.ServiceLayer.Services
{
    public class JWTService : IJWTService
    {
        private readonly string AppLaunchSecretKey = Environment.GetEnvironmentVariable("AppLaunchSecretKey", EnvironmentVariableTarget.User);
        private readonly string symmetricKeyFinal = Environment.GetEnvironmentVariable("JWTSignature", EnvironmentVariableTarget.User);
        private ILoggerService _gngLoggerService;
        private ICryptoService _cryptoService;
        private JwtSecurityTokenHandler tokenHandler;
        private readonly SigningCredentials credentials;
        public JWTService()
        {
            _gngLoggerService = new LoggerService();
            _cryptoService = new CryptoService(AppLaunchSecretKey);
            tokenHandler = new JwtSecurityTokenHandler();
            credentials = _cryptoService.GenerateJWTSignature(symmetricKeyFinal);
        }

        public string CreateToken(string username, int uId)
        {
            var jwtHeader = new JwtHeader(credentials);

            var usersClaims = RetrieveClaims(username);
            var securityClaimsList = new List<System.Security.Claims.Claim>();

            //Takes the claims the user has and puts it in a list of Security Claims objects
            foreach (var c in usersClaims)
            {
                securityClaimsList.Add(new System.Security.Claims.Claim(c.ClaimName, uId.ToString()));
            }

            var jwtPayload = new JwtPayload(
                issuer: "greetngroup.com",
                audience: username,
                claims: securityClaimsList,
                notBefore: null,
                expires: DateTime.UtcNow.AddMinutes(30)
                );

            var jwt = new JwtSecurityToken(jwtHeader, jwtPayload);

            return tokenHandler.WriteToken(jwt);
        }

        public string UpdateToken(string jwtToken)
        {
            if (!IsJWTSignatureTampered(jwtToken))
            {
                return RefreshToken(jwtToken);
            }
            else
            {
                return "";
            }
        }

        public string RefreshToken(string userJwtString)
        {
            var jwtHeader = new JwtHeader(credentials);

            var oldJwt = tokenHandler.ReadJwtToken(userJwtString);
            var newJwtPayload = new JwtPayload(
                issuer: oldJwt.Issuer,
                audience: oldJwt.Audiences.ToString(),
                claims: oldJwt.Claims,
                notBefore: null,
                expires: DateTime.UtcNow.AddMinutes(30));

            var newJwt = new JwtSecurityToken(jwtHeader, newJwtPayload);

            return tokenHandler.WriteToken(newJwt);
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
            //Must always check if JWT is tampered or not with JWT operations
            if (IsJWTSignatureTampered(userJwtToken) == false)
            {
                var jwt = tokenHandler.ReadJwtToken(userJwtToken);

                var usersCurrClaims = jwt.Claims;
                var pass = false;

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

        /// <summary>
        /// Method GetUserIDFromToken retrieves user id from the jwt. If the id
        /// cannot be parsed from the token or if the user's jwt signature has been tampered
        /// return -1.
        /// </summary>
        /// <param name="userJwtToken">JWT in string format</param>
        /// <returns>Return the user id as an int</returns>
        public int GetUserIDFromToken(string userJwtToken)
        {
            if(IsJWTSignatureTampered(userJwtToken) == false)
            {
                var jwt = tokenHandler.ReadJwtToken(userJwtToken);
                var usersCurrClaims = jwt.Claims;
                var userID = usersCurrClaims.First().Value;
                try
                {
                    return Convert.ToInt32(userID);
                }
                //Catch format exception to show that it could not parse the userId 
                //into an int value, let other errors bubble
                catch (FormatException e)
                {
                    _gngLoggerService.LogGNGInternalErrors(e.ToString());
                    return -1;
                }
            }
            else
            {
                return -1;
            }
            
        }

        /// <summary>
        /// Method GetUsernameFromToken retrieves the username from the jwt. If 
        /// JWT signature has been tampered, return null.
        /// </summary>
        /// <param name="jwtString">Users JWT in string form</param>
        /// <returns>Returns username as string</returns>
        public string GetUsernameFromToken(string userJwtString)
        {
            if(IsJWTSignatureTampered(userJwtString) == false)
            {
                var jwt = tokenHandler.ReadJwtToken(userJwtString);
                return jwt.Audiences.First();
            }
            else
            {
                return null;
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
        public bool IsJWTSignatureTampered(string userJwtString)
        {
            var jwt = tokenHandler.ReadJwtToken(userJwtString);
            var isTampered = false;
            var userPayload = jwt.Payload;

            var unalteredHeader = new JwtHeader(credentials);
            var unalteredJwt = new JwtSecurityToken(unalteredHeader, userPayload);
            var unalteredJwtString = tokenHandler.WriteToken(unalteredJwt);

            if (!userJwtString.Equals(unalteredJwtString))
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
                    var user = ctx.Users.FirstOrDefault(u => u.UserName.Equals(username));
                    var userId = user.UserId;
                    var usersClaims = ctx.UserClaims.Where(c => c.UId.Equals(userId));
                    foreach (var claim in usersClaims)
                    {
                        claimsList.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(claim.ClaimId)));
                    }
                }
                return claimsList;
            }
            // Exception caught specifically as it is used in the event that the context 
            // doesnt exist or is broken or fails to dispose
            catch (ObjectDisposedException od) 
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return claimsList;
            }
        }

    }
}
