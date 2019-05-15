using System.Collections.Generic;

namespace Gucci.ServiceLayer.Interface
{
    public interface IJWTService
    {
        string CreateToken(string username, int userId);
        string CheckUserClaims(string jwtToken, List<string> claimsToCheck);
        int GetUserIDFromToken(string jwtToken);
        bool IsJWTSignatureTampered(string userJwtToken);
        string GetUsernameFromToken(string jwtToken);
        bool DeleteTokenFromDB(string jwtToken);
        string IsTokenExpired(string jwt);
        bool InvalidateToken(string jwt);
    }
}
