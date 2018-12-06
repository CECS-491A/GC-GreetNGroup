using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.Code_First
{
    public class UserClaim
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("ClaimPool")]
        public string ClaimId { get; set; }
    }
}