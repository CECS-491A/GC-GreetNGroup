namespace GreetNGroup.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FriendsTable")]
    public partial class FriendsTable
    {
        [Key]
        [StringLength(30)]
        public string FriendId { get; set; }

        [Required]
        [StringLength(30)]
        public string UserId1 { get; set; }

        [Required]
        [StringLength(30)]
        public string UserId2 { get; set; }

        public virtual UserTable UserTable { get; set; }

        public virtual UserTable UserTable1 { get; set; }
    }
}
