namespace GreetNGroup.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserTag
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string TagID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string UserID { get; set; }

        public virtual UserTable UserTable { get; set; }
    }
}
