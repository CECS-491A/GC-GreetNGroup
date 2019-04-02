using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface ICryptoService
    {
        string HashHMAC(byte[] key, string message);
        string GenerateToken();
    }
}
