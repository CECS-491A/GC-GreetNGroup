using System.Collections.Generic;

namespace Gucci.ServiceLayer.Interface
{
    public interface IEventTagService
    {
        bool InsertEventTag(int eventId, string tagId);
        bool DeleteEventTag(int eventId, string tag);
        List<string> ReturnEventTagsOfEvent(int eventId);
    }
}
