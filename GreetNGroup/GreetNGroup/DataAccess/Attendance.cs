using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreetNGroup.DataAccess
{
    [Table("Attendee")]
    public class Attendance
    {
        public Attendance() { }

        [Key, ForeignKey("Event"), Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int EventId { get; set; }
        public Event Event { get; set; }

        [Key, ForeignKey("User"), Column(Order = 2), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public bool CheckedIn { get; set; }

        public string RoleName { get; set; }
    }
}