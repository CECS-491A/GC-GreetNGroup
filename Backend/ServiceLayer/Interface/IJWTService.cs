using System.Collections.Generic;

namespace ServiceLayer.Interface
{
    public interface IJWTService
    {
        string CreateToken(string username, string hashedUID);
        bool CheckUserClaims(string jwtToken, List<string> claimsToCheck);
    }
}
