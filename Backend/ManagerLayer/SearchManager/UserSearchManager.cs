using System.Collections.Generic;
using Gucci.DataAccessLayer.DataTransferObject;
using Gucci.ServiceLayer.Services;

namespace Gucci.ManagerLayer.SearchManager
{
    class UserSearchManager : ISearchable<List<DefaultUserSearchDto>>
    {
        private readonly UserService _userService = new UserService();

        // Implements interface within this region
        #region Interface Implementation

        public List<DefaultUserSearchDto> SearchByName(string name)
        {
            return _userService.GetDefaultUserInfoListByUsername(name);
        }

        public List<DefaultUserSearchDto> SearchById(int id)
        {
            return _userService.GetDefaultUserInfoById(id);
        }

        #endregion
    }
}
