using System.Collections.Generic;
using Gucci.DataAccessLayer.DataTransferObject;
using Gucci.ServiceLayer.Requests;

namespace Gucci.ManagerLayer.FindEventsForMe
{
    public interface IEventFilterable
    {
        List<DefaultEventSearchDto> FilterEvents(FindEventsForMeRequest req);
    }
}
