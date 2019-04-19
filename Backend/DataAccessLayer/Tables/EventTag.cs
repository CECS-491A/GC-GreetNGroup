using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Tables
{
    [Table("EventTag")]
    public class EventTag
    {
        public EventTag()
        {
            EventId = -1;
            TagId = -1;
        }

        public EventTag(int eventId, int tagId)
        {
            EventId = eventId;
            TagId = tagId;
        }

        [Required, ForeignKey("Event"), Column(Order = 1), Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int EventId { get; set; }
        public Event Event { get; set; }

        [Required, ForeignKey("Tag"), Column(Order = 2), Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}