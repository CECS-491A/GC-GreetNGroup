using DataAccessLayer.Tables;
using ServiceLayer.Interface;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.ProfileManagement
{
    public class ProfileManager
    {
        private ICryptoService _cryptoService;
        private IUserService _userService;
        private IJWTService _jwtServce;

        public ProfileManager()
        {
            _userService = new UserService();
            _cryptoService = new CryptoService();
            _jwtServce = new JWTService();
        }

        public bool CheckProfileActivated(string jwtToken)
        {
            string hashedUID = _jwtServce.GetUserIDFromToken(jwtToken);
            //need a function to undero hashedUID to regular uid
            int userID = 0;
            if (_userService.IsUsernameFoundById(userID))
            {
                User retrievedUser = _userService.GetUserById(userID);
                return retrievedUser.IsActivated;
            }
            return false;
        }
    }
}
