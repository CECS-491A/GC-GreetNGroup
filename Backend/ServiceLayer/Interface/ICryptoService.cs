using Microsoft.IdentityModel.Tokens;

namespace Gucci.ServiceLayer.Interface
{
    public interface ICryptoService
    {
        //Winn Created
        string HashHMAC(string message);
        string GenerateToken();
    }
}
