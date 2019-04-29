using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gucci.ServiceLayer.Interface
{
    public interface IEventTagService
    {
        bool InsertEventTag(int eventId, string tagId);
        List<string> ReturnEventTagsOfEvent(int eventId);
        bool DeleteEventTag(int eventId, string tag);

    }
}
