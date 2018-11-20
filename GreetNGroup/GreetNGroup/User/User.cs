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
        private string UserName;
        private string PassWord;
        private string City;
        private string State;
        private string Country;
        private string DateofBirth;
        private string SecurityQuestion;
        private string SecurityAnswer;

        /// <summary>
        /// Cnstructor to set up a user account
        /// </summary>
        /// <param name="userN">Passed user name</param>
        /// <param name="pword">Passed Password</param>
        /// <param name="city">Passed City</param>
        /// <param name="state">Passed State</param>
        /// <param name="country">Passed Country</param>
        /// <param name="DOB">Passed Date of Birth</param>
        /// <param name="securityQ">Passed Security Question</param>
        /// <param name="securityA">Passed Answer to Security Question</param>
        public User(string userN, string pword, string city, string state, string country, string DOB, string securityQ, string securityA)
        {
            UserName = userN;
            PassWord = pword;
            City = city;
            State = state;
            Country = country;
            DateofBirth = DOB;
            SecurityQuestion = securityQ;
            SecurityAnswer = securityA;
        }
        /// <summary>
        /// Returns the account user name or sets a new username
        /// </summary>
        public string Username
        {
            get
            {
                return this.UserName;
            }
            set
            {
                this.UserName = value;
            }
        }

        /// <summary>
        /// Returns the account password or Sets a new Passwrod
        /// </summary>
        public string Password
        {

            get
            {
                return this.PassWord;
            }
            set
            {
                this.PassWord = value;
            }
        }

        /// <summary>
        /// Returns the account City Location or Sets a new City
        /// </summary>
        public string Cityloc
        {

            get
            {
                return this.City;
            }
            set
            {
                this.City = value;
            }
        }

        /// <summary>
        /// Returns the account State Location or sets a new State
        /// </summary>
        public string Stateloc
        {

            get
            {
                return this.State;
            }
            set
            {
                this.State = value;
            }
        }

        /// <summary>
        /// Returns the account Country Location
        /// </summary>
        public string Countryloc
        {

            get
            {
                return this.Country;
            }
            set
            {
                this.Country = value;
            }
        }

        /// <summary>
        /// Returns the account Date of Birth or sets a new Date of Birth
        /// </summary>
        public string DOB
        {

            get
            {
                return this.DateofBirth;
            }
            set
            {
                this.DateofBirth = value;
            }
        }

        /// <summary>
        /// Returns the account Security Question or sets a new Security Question
        /// </summary>
        public string SecurityQ
        {

            get
            {
                return this.SecurityQuestion;
            }
            set
            {
                this.SecurityQuestion = value;
            }
        }

        /// <summary>
        /// Returns the account Security question Answer or sets a new answer
        /// </summary>
        public string SecurityA
        {

            get
            {
                return this.SecurityAnswer;
            }
            set
            {
                this.SecurityAnswer = value;
            }
        }
        public Boolean DoesNameExist(User[] list)
        {
            return true;
        }


    }
}