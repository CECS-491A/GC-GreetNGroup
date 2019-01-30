namespace GreetNGroup
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ClaimsTable")]
    public partial class ClaimsTable
    {
        [Key]
        [StringLength(30)]
        public string ClaimId { get; set; }

        [Required]
        [StringLength(30)]
        public string Claim { get; set; }
    }
}
