using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ServiceLayer.Services
{
    public interface IJWTService
    {
        string CreateToken(string username, List<Claim> securityClaimsList);
        bool CheckUserClaims(string jwtToken, List<string> claimsToCheck);
    }
}
