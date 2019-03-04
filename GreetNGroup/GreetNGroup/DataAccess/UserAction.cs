using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreetNGroup.DataAccess
{
    [Table("UserAction")]
    public class UserAction
    {
        public UserAction() { }

        public UserAction(DateTime aTime, string sId, string action, string uId)
        {
            ActionTime = aTime;
            SessionId = sId;
            Action = action;
            UserId = uId;
        }

        [Key, Column(Order = 1)]
        public DateTime ActionTime { get; set; }

        [Required, Key, Column(Order = 2)]
        public string SessionId { get; set; }

        [Required, Key, Column(Order = 3), ForeignKey("ActionsTable")]
        public virtual string Action { get; set; }
        public ActionsTable ActionsTable { get; set; }

        [ForeignKey("User")]
        public virtual string UserId { get; set; }
        public User User { get; set; }
    }
}