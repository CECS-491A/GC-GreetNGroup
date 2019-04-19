using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Tables
{
    [Table("UserClaim")]
    public class UserClaim
    {
        public UserClaim() { }

        public UserClaim(int userId, Claim claim)
        {
            UId = userId;
            ClaimId = claim.ClaimId;
            Claim = claim;
        }

        [Required, ForeignKey("User"), Column(Order = 1), Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int UId{ get; set; }
        public User User { get; set; }

        [Required, ForeignKey("Claim"), Column(Order = 2), Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}