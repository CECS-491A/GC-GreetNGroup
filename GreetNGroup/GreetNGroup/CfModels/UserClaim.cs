using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreetNGroup.CfModels
{
    [Table("UserClaim")]
    public class UserClaim
    {
        public UserClaim() { }

        [Required, ForeignKey("User"), Column(Order = 1), Key]
        public virtual string UId{ get; set; }
        public User User { get; set; }

        [Required, ForeignKey("Claim"), Column(Order = 2), Key]
        public virtual string ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}