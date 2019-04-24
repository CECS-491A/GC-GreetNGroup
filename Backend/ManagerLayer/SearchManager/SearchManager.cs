using System.Collections.Generic;
using Gucci.DataAccessLayer.DataTransferObject;

namespace Gucci.ManagerLayer.SearchManager
{
    // This class provides an interface for interaction with required search functions
    public class SearchManager
    {
        private readonly ISearchable<List<DefaultEventSearchDto>> _eventManager = new EventSearchManager();
        private readonly ISearchable<List<DefaultUserSearchDto>> _userManager = new UserSearchManager();

        // Used to return list of events given a name -- List is allowed to be empty
        public List<DefaultEventSearchDto> GetEventListByName(string name)
        {
            return _eventManager.SearchByName(name);
        }

        // Used to return User given username -- User is allowed to be null
        public List<DefaultUserSearchDto> GetUserByUsername(string name)
        {
            return _userManager.SearchByName(name);
        }

        // Used to return Username given id -- user is allowed to be null
        public List<DefaultUserSearchDto> GetUserByUserId(int id)
        {
            return _userManager.SearchById(id);
        }
    }
}
