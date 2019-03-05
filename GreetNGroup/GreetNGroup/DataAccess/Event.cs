using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess
{
    [Table("Event")]
    public class Event
    {
        public Event() {}
        public Event(int userId, int eventId, DateTime startDate, string eventName)
        {
            UserId = userId;
            EventId = eventId;
            StartDate = startDate;
            EventName = eventName;
        }

        [Required, ForeignKey("User")]
        public virtual int UserId { get; set; }
        public User User { get; set; }

        [Key]
        public int EventId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public string EventName { get; set; }
    }
}