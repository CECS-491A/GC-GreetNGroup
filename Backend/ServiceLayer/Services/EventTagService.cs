using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Tables;
using ServiceLayer.Interface;

namespace ServiceLayer.Services
{
    public class EventTagService
    {
        private IGNGLoggerService _gngLoggerService;

        public EventTagService()
        {
            _gngLoggerService = new GNGLoggerService();
        }

        #region Insert Tag Information

        // Inserts EventTag into the database, creates a link between an event and a tag
        public bool InsertEventTag(int eventId, int tagId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    EventTag newTag = new EventTag(eventId, tagId);
                    ctx.EventTags.Add(newTag);
                    ctx.SaveChanges();
                    return true;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        #endregion

        #region Retrieve Tag Information

        // Returns list of tag names of an event given event id
        public List<string> ReturnEventTagsOfEvent(int eventId)
        {
            var tagNameList = new List<string>();
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var tagIdList = ctx.EventTags.Where(e => e.EventId.CompareTo(eventId) == 0).ToList();

                    foreach (EventTag t in tagIdList)
                    {
                        tagNameList.Add(ctx.Tags.FirstOrDefault(c => c.TagId.Equals(t.TagId))?.TagName);
                    }
                    
                    return tagNameList;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return tagNameList;
            }
        }

        #endregion

        #region Delete Tag Information

        // Removes pair of tagId and eventId where values match in database
        public bool DeleteEventTag(int eventId, int tagId)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var foundTag = ctx.EventTags.FirstOrDefault(t => t.EventId.Equals(eventId) && t.TagId.Equals(tagId));

                    if (foundTag != null)
                    {
                        ctx.EventTags.Remove(foundTag);
                        ctx.SaveChanges();
                        return true;
                    }

                    return false;
                }
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return false;
            }
        }

        #endregion
    }
}
