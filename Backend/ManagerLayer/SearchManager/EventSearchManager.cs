using System.Collections.Generic;
using Gucci.DataAccessLayer.DataTransferObject;
using Gucci.ServiceLayer.Services;

namespace Gucci.ManagerLayer.SearchManager
{
    class EventSearchManager : ISearchable<List<DefaultEventSearchDto>>
    {
        private readonly EventService _eventService = new EventService();

        // Implements interface within this region
        #region Interface Implementation

        public List<DefaultEventSearchDto> SearchByName(string name)
        {
            return _eventService.GetPlainEventDetailListByName(name);
        }

        public List<DefaultEventSearchDto> SearchById(int id)
        {
            return  _eventService.GetPlainEventDetailListById(id);
        }

        #endregion
    }
}
