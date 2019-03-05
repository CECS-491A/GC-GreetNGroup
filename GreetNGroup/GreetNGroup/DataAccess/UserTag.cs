using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess
{
    [Table("UserTag")]
    public class UserTag
    {
        public UserTag() {}

        [Required, ForeignKey("Tag"), Column(Order = 1), Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int TagId { get; set; }
        public Tag Tag { get; set; }

        [Required, ForeignKey("User"), Column(Order = 2), Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int UserId { get; set; }
        public User User { get; set; }
    }
}