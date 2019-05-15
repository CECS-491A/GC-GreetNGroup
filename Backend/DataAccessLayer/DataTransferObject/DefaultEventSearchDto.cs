using System;

namespace Gucci.DataAccessLayer.DataTransferObject
{
    public class DefaultEventSearchDto
    {
        public int Uid { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventLocation { get; set; }
        public DateTime StartDate { get; set; }

        public DefaultEventSearchDto()
        {
            Uid = -1;
            EventId = -1;
            EventName = "";
            EventLocation = "";
            StartDate = new DateTime();
        }

        public DefaultEventSearchDto(int uid, int eventId, string eventName, string location, DateTime startDate)
        {
            Uid = uid;
            EventId = eventId;
            EventName = eventName;
            EventLocation = location;
            StartDate = startDate;
        }

        public bool CompareDefaultEventSearchDto(DefaultEventSearchDto other)
        {
            if ((Uid == other.Uid) &&
                (EventId == other.EventId) &&
                (string.Compare(EventName, other.EventName, StringComparison.Ordinal) == 0) &&
                (string.Compare(EventLocation, other.EventLocation, StringComparison.Ordinal) == 0) &&
                (DateTime.Equals(StartDate, other.StartDate)))
            {
                return true;
            }

            return false;
        }
    }
}
