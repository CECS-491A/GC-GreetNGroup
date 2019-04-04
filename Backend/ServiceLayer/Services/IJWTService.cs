using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ServiceLayer.Services
{
    public interface IJWTService
    {
        bool CheckUserClaims(JwtSecurityToken jwt, List<string> claimsToCheck);
        JwtSecurityToken CreateToken(List<Claim> securityClaimsList);
    }
}
