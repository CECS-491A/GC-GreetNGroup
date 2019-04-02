using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class JWTService : IJWTService
    {
        /// <summary>
        /// Method to check if a user has the appropriate claims to access/perform an
        /// action on the app. If they have the claim, return true, otherwise, return
        /// false.
        /// </summary>
        /// <param name="jwt">JWT of the user</param>
        /// <param name="claimToCheck">Required claim needed to perform/access</param>
        /// <returns>True or false depending on if the user has the claim</returns>
        public bool CheckUserClaims(JwtSecurityToken jwt, List<string> claimsToCheck)
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
