using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace ServiceLayer.Services
{
    public interface IJWTService
    {
        bool CheckUserClaims(JwtSecurityToken jwt, List<string> claimsToCheck);
    }
}
