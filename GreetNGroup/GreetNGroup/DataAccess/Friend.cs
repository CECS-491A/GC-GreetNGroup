using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess
{
    [Table("Friend")]
    public class Friend
    {
        public Friend() { }

        [Key]
        public string FriendId { get; set; }

        [Required, ForeignKey("User1")]
        public virtual string UserId1 { get; set; }
        public User User1 { get; set; }

        [Required, ForeignKey("User2")]
        public virtual string UserId2 { get; set; }
        public User User2 { get; set; }
    }
}