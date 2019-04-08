using Microsoft.IdentityModel.Tokens;

namespace ServiceLayer.Interface
{
    public interface ICryptoService
    {
        //Winn Created
        string HashHMAC(string message);
        string GenerateToken();
        //Jonalyn
        string HashSha256(string message);
        SigningCredentials GenerateJWTSignature();
    }
}
