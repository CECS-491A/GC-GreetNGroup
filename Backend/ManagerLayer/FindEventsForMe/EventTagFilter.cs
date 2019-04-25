using System;
using System.Collections.Generic;
using Gucci.DataAccessLayer.DataTransferObject;
using Gucci.ServiceLayer.Requests;

namespace Gucci.ManagerLayer.FindEventsForMe
{
    class EventTagFilter : IEventFilterable
    {
        public List<DefaultEventSearchDto> FilterEvents(FindEventsForMeRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
