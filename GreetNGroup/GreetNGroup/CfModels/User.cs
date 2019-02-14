using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GreetNGroup.CfModels
{
    [Table("User")]
    public class User
    {
        public User()
        {
            IsActivated = true;
        }

        [Key]
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public DateTime DoB { get; set; }

        [Required]
        public string SecurityQuestion { get; set; }
        [Required]
        public string SecurityAnswer { get; set; }

        public bool IsActivated { get; set; }
    }
}