using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess
{
    [Table("Blocklist")]
    public class Blocklist
    {
        public Blocklist() { }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BlockId { get; set; }

        [Required, ForeignKey("User1")]
        public virtual int UserId1 { get; set; }
        public User User1 { get; set; }

        [Required, ForeignKey("User2")]
        public virtual int UserId2 { get; set; }
        public User User2 { get; set; }
    }
}