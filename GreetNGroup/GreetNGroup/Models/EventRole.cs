namespace GreetNGroup.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EventRole
    {
        [Required]
        [StringLength(20)]
        public string EventId { get; set; }

        [Key]
        [StringLength(30)]
        public string RoleName { get; set; }

        public int? MaxOccupancy { get; set; }

        public bool? Required { get; set; }

        public virtual Event Event { get; set; }
    }
}
