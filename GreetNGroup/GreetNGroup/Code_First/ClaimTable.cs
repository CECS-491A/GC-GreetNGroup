using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.Code_First
{
    [Table("Claim Table")]
    public partial class ClaimTable
    {
        [Key]
        [StringLength(30)]
        public string ClaimId { get; set; }
        [StringLength(30)]
        public string ClaimName { get; set; }
    }
}