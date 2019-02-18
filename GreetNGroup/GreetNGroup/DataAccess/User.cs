using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess
{
    [Table("User")]
    public class User
    {
        public User()
        {
            IsActivated = true;
        }

        public User(string uId, string firstName, string lastName, string userName, string password, string city,
                    string state, string country, DateTime dob, string securityQ, string securityA, bool isActivated)
        {
            UserId = uId;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Password = password;
            City = city;
            State = state;
            Country = country;
            DoB = dob;
            SecurityQuestion = securityQ;
            SecurityAnswer = securityA;
            IsActivated = isActivated;
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