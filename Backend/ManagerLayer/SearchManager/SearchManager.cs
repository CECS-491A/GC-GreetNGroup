using System.Collections.Generic;
using DataAccessLayer.Tables;
using ServiceLayer.Interface;
using ServiceLayer.Services;

namespace ManagerLayer.SearchManager
{
    // This class abstracts the needed functions of the event and user service for the SearchBar
    public class SearchManager
    {
        private readonly EventService _eventService = new EventService();
        private readonly IUserService _userService = new UserService();
        private readonly IJWTService _jwtService = new JWTService();

        // Used to return list of events given a name -- List is allowed to be empty
        public List<Event> GetEventListByName(string name)
        {
            return _eventService.GetEventListByName(name);
        }

        // Used to return User given username -- User is allowed to be null
        public User GetUserByUsername(string name)
        {
            return _userService.GetUserByUsername(name);
        }

        // Retrieves and returns Username from jwt using jwt service
        public int GetUserIdFromJwt(string jwt)
        {
            if (jwt.Length > 0) return _jwtService.GetUserIDFromToken(jwt);
            // Non registered user
            return -1;
        }
    }
}
