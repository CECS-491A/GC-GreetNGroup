using DataAccessLayer.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface IUserService
    {
        bool CreateUser(User user);
        //Jonalyn
        bool IsExistingGNGUser(string username);
        //Winn
        int GetNextUserID();
    }
}
