namespace GreetNGroup
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class UserClaim
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string ClaimId { get; set; }
    }
}
