using Gucci.DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Gucci.ServiceLayer.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccessLayer.Tables;
using System.Data.Entity;

namespace Gucci.ServiceLayer.Services
{
    public class JWTService : IJWTService
    {
        private readonly string symmetricKeyFinal = Environment.GetEnvironmentVariable("JWTSignature", EnvironmentVariableTarget.Machine);
        private ILoggerService _gngLoggerService;
        private JwtSecurityTokenHandler tokenHandler;
        private readonly SigningCredentials credentials;

        /* Since retrieving information in a JWT requires checking first if the JWT
         * is tampered, a tampered message is required in the event that the JWT is 
         * tampered with.
         */ 
        private const string TAMPERED_MESSAGE = "Tampered";

        public JWTService()
        {
            _gngLoggerService = new LoggerService();
            tokenHandler = new JwtSecurityTokenHandler();
            credentials = GenerateJWTSignature(symmetricKeyFinal);
        }

        /// <summary>
        /// Method CreateToken creates the JWT which will be given to the user who
        /// is registered with GNG.
        /// </summary>
        /// <param name="username">username of the user</param>
        /// <param name="uId">userId of the user</param>
        /// <returns>The JWT in string form</returns>
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

            var jwtToken = tokenHandler.WriteToken(jwt);

            var isTokenAddedToDB = AddTokenToDB(jwtToken, uId);
            if (!isTokenAddedToDB)
            {
                return null;
            }

            return jwtToken;
        }

        /// <summary>
        /// Method IsTokenExpired checks if the JWT of the user is expired or not
        /// by comparing the time of expiration to the current time. The JWT must
        /// first be checked if it was tampered with or not.
        /// </summary>
        /// <param name="jwt">JWT in string form</param>
        /// <returns>Return a string based on the status of the JWT</returns>
        public string IsTokenExpired(string jwt)
        {
            if (!IsJWTSignatureTampered(jwt))
            {
                var jwtPayload = tokenHandler.ReadJwtToken(jwt).Payload;
                var assignTime = jwtPayload.ValidTo;
                if((DateTime.UtcNow - assignTime).TotalMinutes > 30)
                {
                    return "Expired";
                }
                else
                {
                    return "NotExpired";
                }
            }
            else
            {
                return TAMPERED_MESSAGE;
            }
        }

        /// <summary>
        /// Method RefreshToken returns the refreshed JWT of the user with an
        /// expired JWT so long as their JWT has not been tampered with.
        /// </summary>
        /// <param name="jwtToken">The expired JWT as a string</param>
        /// <returns>Return refreshed JWT string or an empty string if the JWT has been
        /// tampered</returns>
        public string RefreshToken(string oldJwtToken)
        {
            if (!IsJWTSignatureTampered(oldJwtToken))
            {
                var jwtHeader = new JwtHeader(credentials);

                var oldJwt = tokenHandler.ReadJwtToken(oldJwtToken); 
                var newJwtPayload = new JwtPayload( 
                    issuer: oldJwt.Issuer,
                    audience: oldJwt.Audiences.ToString(),
                    claims: oldJwt.Claims,
                    notBefore: null,
                    expires: DateTime.UtcNow.AddMinutes(30));

                var newJwt = new JwtSecurityToken(jwtHeader, newJwtPayload);

                var newJwtToken = tokenHandler.WriteToken(newJwt); // Create a new token

                
                var userIDOnToken = GetUserIDFromToken(oldJwtToken);

                AddTokenToDB(newJwtToken, userIDOnToken); // Add new token to DB
                DeleteTokenFromDB(oldJwtToken); // Delete old token from DB to 'revoke' 

                return newJwtToken;
            }
            else
            {
                return TAMPERED_MESSAGE;
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
        public string CheckUserClaims(string userJwtToken, List<string> claimsToCheck)
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

                if (pass == true)
                {
                    return "Authorized";
                }
                else
                {
                    return "Unauthorized";
                }
            }
            else
            {
                return TAMPERED_MESSAGE;
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
            if (IsJWTSignatureTampered(userJwtToken) == false)
            {
                var jwt = tokenHandler.ReadJwtToken(userJwtToken);
                var usersCurrClaims = jwt.Claims;
                var userID = usersCurrClaims.First().Value;
                try
                {
                    return Convert.ToInt32(userID);
                }
                catch (Exception e)
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
            if (IsJWTSignatureTampered(userJwtString) == false)
            {
                var jwt = tokenHandler.ReadJwtToken(userJwtString);
                return jwt.Audiences.First();
            }
            else
            {
                return TAMPERED_MESSAGE;
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
            catch (Exception od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return claimsList;
            }
        }

        /// <summary>
        /// Method GenerateJWTSignature generates the signature for the JWT
        /// </summary>
        /// <param name="symmetricKey">The key used to generate the signature</param>
        /// <returns>Returns the SigningCredentials object which is the signature</returns>
        private SigningCredentials GenerateJWTSignature(string symmetricKey)
        {
            return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricKey)),
                SecurityAlgorithms.HmacSha256Signature);
        }

        /// <summary>
        /// Method InvalidateUserJWT revokes the user's session should their JWT be
        /// tampered with.
        /// </summary>
        /// <param name="jwt">User's JWT as a string</param>
        /// <returns>Tampered string message or conflict if an error occurred in
        /// revoking their session on the database.</returns>
        public string InvalidateUserJWT(string jwt)
        {
            if(DeleteTokenFromDB(jwt) == true)
            {
                return TAMPERED_MESSAGE;
            }
            else
            {
                return "Conflict";
            }
        }


        #region Winn
        public bool AddTokenToDB(string jwtToken, int userID)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var newTokenID = GetNextJWTTokenID();
                    if (newTokenID == -1)
                    {
                        return false;
                    }

                    var JWTTokenToAdd = new JWTToken(newTokenID, jwtToken, userID, true);

                    ctx.JWTTokens.Add(JWTTokenToAdd);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool InvalidateToken(string jwtToken)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var userID = GetUserIDFromToken(jwtToken);
                    var retrievedToken = ctx.JWTTokens.Where(j => j.Token == jwtToken)
                                                     .Where(j => j.UserId == userID)
                                                     .First();
                    if (retrievedToken != null)
                    {
                        retrievedToken.isValid = false;
                        ctx.Entry(retrievedToken).State = EntityState.Modified;
                        ctx.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteTokenFromDB(string jwtToken)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var userID = GetUserIDFromToken(jwtToken);
                    var TokenToRemove = ctx.JWTTokens.Where(j => j.Token == jwtToken)
                                                     .Where(j => j.UserId == userID)
                                                     .First();
                    if (TokenToRemove != null)
                    {
                        ctx.JWTTokens.Remove(TokenToRemove);
                        ctx.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private int GetNextJWTTokenID()
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var lastTokenInDB = ctx.JWTTokens.OrderByDescending(j => j.Id).FirstOrDefault();
                    if(lastTokenInDB == null)
                    {
                        return 1;
                    }
                    return lastTokenInDB.Id + 1;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public bool IsTokenValid(string JwtToken)
        {
            try
            {
                if (IsJWTSignatureTampered(JwtToken))
                {
                    return false;
                }
                using(var ctx = new GreetNGroupContext())
                {
                    var tokenToFind = ctx.JWTTokens.Where(j => j.Token == JwtToken).FirstOrDefault<JWTToken>();
                    if(tokenToFind == null)
                    {
                        return false;
                    }
                    return tokenToFind.isValid;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion
    }
}
