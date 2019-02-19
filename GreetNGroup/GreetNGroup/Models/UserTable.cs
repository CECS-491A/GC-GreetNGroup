namespace GreetNGroup.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserTable")]
    public partial class UserTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserTable()
        {
            Attendances = new HashSet<Attendance>();
            BlockTables = new HashSet<BlockTable>();
            BlockTables1 = new HashSet<BlockTable>();
            Events = new HashSet<Event>();
            FriendsTables = new HashSet<FriendsTable>();
            FriendsTables1 = new HashSet<FriendsTable>();
            UserTags = new HashSet<UserTag>();
            ClaimsTables = new HashSet<ClaimsTable>();
        }

        [StringLength(30)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string LastName { get; set; }

        [StringLength(30)]
        public string UserName { get; set; }

        [StringLength(30)]
        public string Password { get; set; }

        [Key]
        [StringLength(30)]
        public string UserId { get; set; }

        [StringLength(30)]
        public string City { get; set; }

        [StringLength(30)]
        public string State { get; set; }

        [StringLength(30)]
        public string Country { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DoB { get; set; }

        [StringLength(50)]
        public string SecurityQuestion { get; set; }

        [StringLength(30)]
        public string SecurityAnswer { get; set; }

        public bool? isActivated { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlockTable> BlockTables { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlockTable> BlockTables1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Event> Events { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FriendsTable> FriendsTables { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FriendsTable> FriendsTables1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserTag> UserTags { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClaimsTable> ClaimsTables { get; set; }
    }
}
