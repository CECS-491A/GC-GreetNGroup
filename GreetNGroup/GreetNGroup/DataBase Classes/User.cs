using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreetNGroup.DataBase_Classes
{
    [Table("StudentAccount")]
    public class User
    {
        [Key]
        public string UserID { get; set; }

        public string Username { get; set; }

        [MinLength(12)]
        [MaxLength(20)]
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Cityloc { get; set; }
        public string Stateloc { get; set; }
        public string Countryloc { get; set; }
        public string SecurityQ { get; set; }
        public string SecurityA { get; set; }
        public Boolean EnableSwitch { get; set; }


    }
}