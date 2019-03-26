using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Tables
{
    [Table("UserRating")]
    public class UserRating
    {
        public UserRating() { }

        [Key, Required, ForeignKey("RaterId"), Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int RaterId1 { get; set; }
        public User RaterId { get; set; }

        [Key, Required, ForeignKey("RatedId"), Column(Order = 2), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int RatedId1 { get; set; }
        public User RatedId { get; set; }

        [Required]
        public float Rating { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }
}