using System.Collections.Generic;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Services;

namespace Gucci.ManagerLayer.SearchManager
{
    class EventSearchManager : ISearchable<List<Event>>
    {
        private readonly EventService _eventService = new EventService();

        // Implements interface within this region
        #region Interface Implementation

        public List<Event> SearchByName(string name)
        {
            return _eventService.GetEventListByName(name);
        }

        public List<Event> SearchById(int id)
        {
            return  _eventService.GetEventListById(id);
        }

        #endregion
    }
}
