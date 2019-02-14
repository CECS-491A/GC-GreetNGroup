namespace GreetNGroup.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Attendance")]
    public partial class Attendance
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string EventId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string UserId { get; set; }

        [StringLength(30)]
        public string RoleName { get; set; }

        public bool? CheckedInStatus { get; set; }

        public virtual Event Event { get; set; }

        public virtual UserTable UserTable { get; set; }
    }
}
