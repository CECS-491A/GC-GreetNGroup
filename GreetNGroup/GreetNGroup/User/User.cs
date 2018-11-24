using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/*
    Basic user account class with all fields needed for a registered account     
*/
namespace GreetNGroup.User
{
    public class User : IIdentifiable
    {
        private string UserName;
        private string PassWord;
        private string FirstName;
        private string LastName;
        private string DateofBirth;
        private string City;
        private string State;
        private string Country;

        /// <summary>
        /// Constructor to set up a user account
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
            string state, string country, string DOB, string securityQ, string securityA, string userID)
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
            UserID = userID;
        }
        
#region Getters and Setters
        
    #region IIdentifiable Declarations
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string DOB { get; set; }
        
    #endregion
               
    #region ILocatable Declarations
        
        public string Cityloc { get; set; }
        public string Stateloc { get; set; }
        public string Countryloc { get; set; }

        #endregion

        /// Returns the account Security Question or sets a new Security Question
        public string SecurityQ { get; set; }

        /// Returns the account Security question Answer or sets a new answer
        /// </summary>
        public string SecurityA { get; set; }

        public string UserID { get; set; }
        #endregion
        public Boolean DoesNameExist(User[] list)
        {
            return true;
        }
    }
}