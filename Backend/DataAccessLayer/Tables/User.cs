using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Tables
{
    [Table("User")]
    public class User
    {
        public User()
        {
            IsActivated = false;
        }

        public User(int uId, string firstName, string lastName, string userName, string city,
                    string state, string country, DateTime dob, bool isActivated)
        {
            UserId = uId;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            City = city;
            State = state;
            Country = country;
            DoB = dob;
            IsActivated = isActivated;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public DateTime DoB { get; set; }

        public bool IsActivated { get; set; }
    }
}