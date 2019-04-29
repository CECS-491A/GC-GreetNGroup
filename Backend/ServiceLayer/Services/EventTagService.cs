using System;
using System.Collections.Generic;
using System.Linq;
using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Interface;

namespace Gucci.ServiceLayer.Services
{
    public class EventTagService : IEventTagService
    {
        private ILoggerService _gngLoggerService;

        public EventTagService()
        {
            _gngLoggerService = new LoggerService();
        }

        #region Insert Tag Information

        // Inserts EventTag into the database, creates a link between an event and a tag
        public bool InsertEventTag(int eventId, int tag)
        {
            bool isSuccessfulAdd = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var tagToAdd = ctx.Tags.FirstOrDefault(t => t.TagName.Equals(tag));
                    var tagIdNum = tagToAdd.TagId;
                    var eventTag = new EventTag(eventId, tagIdNum);
                    ctx.EventTags.Add(eventTag);

                    ctx.SaveChanges();
                    isSuccessfulAdd = true;
                }
                return isSuccessfulAdd;
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isSuccessfulAdd;
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
        public bool DeleteEventTag(int eventId, int tag)
        {
            bool isSuccessfulDelete = false;
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var eventTags = ctx.EventTags.Where(e => e.EventId.Equals(eventId));
                    foreach (var tags in eventTags)
                    {
                        if (tags.Tag.TagName.Equals(tag))
                        {
                            ctx.EventTags.Remove(tags);
                            isSuccessfulDelete = true;
                        }
                    }
                    ctx.SaveChanges();
                }
                return isSuccessfulDelete;
            }
            catch (ObjectDisposedException od)
            {
                _gngLoggerService.LogGNGInternalErrors(od.ToString());
                return isSuccessfulDelete;
            }
        }

        #endregion
    }
}
