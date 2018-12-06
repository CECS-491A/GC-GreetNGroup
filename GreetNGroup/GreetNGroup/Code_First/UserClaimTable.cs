using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.Code_First
{
    [Table("UserClaims")]
    public partial class UserClaimTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserClaimTable()
        {
            ClaimsTables = new HashSet<ClaimsTable>();
            UserTables = new HashSet<UserTable>();
        }
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string ClaimId { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClaimsTable> ClaimsTables { get; set; }
        public virtual ICollection<UserTable> UserTables { get; set; }

    }
}