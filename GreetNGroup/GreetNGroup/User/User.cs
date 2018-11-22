using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
    Basic user account class with all fields needed for a registered account     
*/
namespace GreetNGroup.User
{
    public class User
    {
        /// <summary>
        /// Cnstructor to set up a user account
        /// </summary>
        /// <param name="userN">Passed user name</param>
        /// <param name="pword">Passed Password</param>
        /// <param name="FName">Passed user name</param>
        /// <param name="LName">Passed Password</param>
        /// <param name="city">Passed City</param>
        /// <param name="state">Passed State</param>
        /// <param name="country">Passed Country</param>
        /// <param name="DOB">Passed Date of Birth</param>
        /// <param name="securityQ">Passed Security Question</param>
        /// <param name="securityA">Passed Answer to Security Question</param>
        public User(string userN, string pword, string FName, string LName, string city, 
            string state, string country, string DOB, string securityQ, string securityA)
        {
            Username = userN;
            Password = pword;
            Firstname = FName;
            Lastname = LName;
            Cityloc = city;
            Stateloc = state;
            Countryloc = country;
            this.DOB = DOB;
            SecurityQ = securityQ;
            SecurityA = securityA;
        }
        
        /// Returns the account user name or sets a new username
        public string Username { get; set; }

        /// Returns the account password or Sets a new Password
        public string Password { get; set; }

        /// Returns the first name of user or Sets a new first name
        public string Firstname { get; set; }

        /// Returns the last name of user or sets a new last name
        public string Lastname { get; set; }

        /// Returns the account City Location or Sets a new City
        public string Cityloc { get; set; }

        /// Returns the account State Location or sets a new State
        public string Stateloc { get; set; }

        /// Returns the account Country Location
        public string Countryloc { get; set; }

        /// Returns the account Date of Birth or sets a new Date of Birth
        public string DOB { get; set; }

        /// Returns the account Security Question or sets a new Security Question
        public string SecurityQ { get; set; }

        /// Returns the account Security question Answer or sets a new answer
        public string SecurityA { get; set; }

        public Boolean DoesNameExist(User[] list)
        {
            return true;
        }
    }
}