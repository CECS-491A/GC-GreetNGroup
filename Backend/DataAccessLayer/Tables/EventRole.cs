using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Tables
{
    [Table("EventRole")]
    public class EventRole
    {
        public EventRole() { }
        
        [Required, ForeignKey("Event"), Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int EventId { get; set; }
        public Event Event { get; set; }

        [Key, Column(Order = 2)]
        public string RoleName { get; set; }

        public int MaxRoleCount { get; set; }
        public bool RequiredRole { get; set; }
    }
}