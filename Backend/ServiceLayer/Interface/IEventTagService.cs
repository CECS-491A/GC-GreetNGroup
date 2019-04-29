using System.Collections.Generic;

namespace Gucci.ServiceLayer.Interface
{
    public interface IEventTagService
    {
        bool InsertEventTag(int eventId, int tagId);
        bool DeleteEventTag(int eventId, int tag);
        List<string> ReturnEventTagsOfEvent(int eventId);
    }
}
