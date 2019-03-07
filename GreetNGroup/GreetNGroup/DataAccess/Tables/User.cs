using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreetNGroup.DataAccess.Tables
{
    [Table("User")]
    public class User
    {
        public User()
        {
            IsActivated = true;
        }

        public User(int uId, string firstName, string lastName, string userName, string password, string city,
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

        public User(int uId, string firstName, string lastName, string userName, string city, string state,
            string country, DateTime dob)
        {
            UserId = uId;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            City = city;
            State = state;
            Country = country;
            DoB = dob;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

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