using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.Code_First
{
    [Table("User Claim Table")]
    public partial class UserClaimTable
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("ClaimPool")]
        public string ClaimId { get; set; }
    }
}