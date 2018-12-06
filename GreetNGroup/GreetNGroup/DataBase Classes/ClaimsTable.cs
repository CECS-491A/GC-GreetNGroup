namespace GreetNGroup
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
