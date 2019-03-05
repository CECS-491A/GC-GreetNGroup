using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreetNGroup.DataAccess
{
    [Table("UserRating")]
    public class UserRating
    {
        public UserRating() { }

        [Key, Required, ForeignKey("RaterId"), Column(Order = 1)]
        public virtual int RaterId1 { get; set; }
        public User RaterId { get; set; }

        [Key, Required, ForeignKey("RatedId"), Column(Order = 2)]
        public virtual int RatedId1 { get; set; }
        public User RatedId { get; set; }

        [Required]
        public float Rating { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }
}