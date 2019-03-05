using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreetNGroup.DataAccess
{
    [Table("EventRole")]
    public class EventRole
    {
        public EventRole() { }
        
        [Required, ForeignKey("Event"), Key, Column(Order = 1)]
        public virtual int EventId { get; set; }
        public Event Event { get; set; }

        [Key, Column(Order = 2)]
        public string RoleName { get; set; }

        public int MaxRoleCount { get; set; }
        public bool RequiredRole { get; set; }
    }
}