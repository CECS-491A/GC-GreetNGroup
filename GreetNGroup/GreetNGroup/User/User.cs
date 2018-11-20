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
        /// Returns the account user name
        /// </summary>
        /// <returns>The user name</returns>
        public string GetUserName()
        {
            return UserName;
        }

        /// <summary>
        /// Returns the account password
        /// </summary>
        /// <returns>The password</returns>
        public string GetPassword()
        {
            return PassWord;
        }

        /// <summary>
        /// Returns the account City Location
        /// </summary>
        /// <returns>The city</returns>
        public string GetCity()
        {
            return City;
        }

        /// <summary>
        /// Returns the account State Location
        /// </summary>
        /// <returns>The state</returns>
        public string GetState()
        {
            return State;
        }

        /// <summary>
        /// Returns the account Country Location
        /// </summary>
        /// <returns>The user name</returns>
        public string GetCountry()
        {
            return Country;
        }

        /// <summary>
        /// Returns the account Date of Birth
        /// </summary>
        /// <returns>The date of birth</returns>
        public string GetDOB()
        {
            return DateofBirth;
        }

        /// <summary>
        /// Returns the account Security Question
        /// </summary>
        /// <returns>The security question</returns>
        public string GetSecurityQuestion()
        {
            return SecurityQuestion;
        }

        /// <summary>
        /// Returns the account Security question Answer
        /// </summary>
        /// <returns>The answer to security question</returns>
        public string GetSecurityAnswer()
        {
            return SecurityAnswer;
        }


    }
}