using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess
{
    [Table("EventTag")]
    public class EventTag
    {
        public EventTag() {}

        [Required, ForeignKey("Event"), Column(Order = 1), Key]
        public virtual string EventId { get; set; }
        public Event Event { get; set; }

        [Required, ForeignKey("Tag"), Column(Order = 2), Key]
        public virtual string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}