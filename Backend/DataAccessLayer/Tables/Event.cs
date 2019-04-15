using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Tables
{
    [Table("Event")]
    public class Event
    {
        public Event() {}
        public Event(int userId, DateTime startDate, string eventName, string eventLocation, string eventDescription)
        {
            UserId = userId;
            StartDate = startDate;
            EventName = eventName;
            EventLocation = eventLocation;
            EventDescription = eventDescription;

        }

        [Required, ForeignKey("User")]
        public virtual int UserId { get; set; }
        public User User { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public string EventLocation { get; set; }

        public string EventDescription { get; set; }
    }
}