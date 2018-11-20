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
        private string GetUserName()
        {
            return UserName;
        }
    }
}