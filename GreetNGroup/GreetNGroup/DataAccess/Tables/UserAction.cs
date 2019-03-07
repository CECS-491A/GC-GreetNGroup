using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess.Tables
{
    [Table("UserAction")]
    public class UserAction
    {
        public UserAction() { }

        public UserAction(DateTime aTime, int sId, int actionId, int uId)
        {
            ActionTime = aTime;
            SessionId = sId;
            ActionId = actionId;
            UserId = uId;
        }

        [Key, Column(Order = 1)]
        public DateTime ActionTime { get; set; }

        [Required, Key, Column(Order = 2), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SessionId { get; set; }

        [Required, Key, Column(Order = 3), ForeignKey("ActionsTable"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int ActionId { get; set; }
        public ActionsTable ActionsTable { get; set; }

        [ForeignKey("User")]
        public virtual int UserId { get; set; }
        public User User { get; set; }
    }
}