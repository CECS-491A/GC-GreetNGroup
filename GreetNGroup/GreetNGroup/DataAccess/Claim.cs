using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace GreetNGroup.DataAccess
{
    [Table("Claim")]
    public class Claim
    {
        public Claim(){}

        public Claim(int claimId, string claimName)
        {
            ClaimId = claimId;
            ClaimName = claimName;
        }

        [Key]
        public int ClaimId { get; set; }
        public string ClaimName { get; set; }
    }
}