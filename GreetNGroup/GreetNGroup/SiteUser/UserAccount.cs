using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GreetNGroup.Claim_Controls;

/*
    Basic user account class with all fields needed for a registered account     
*/
namespace GreetNGroup.SiteUser
{
    public class UserAccount : IIdentifiable
    {
        private string UserName;
        private string PassWord;
        private string FirstName;
        private string LastName;
        private string DateofBirth;
        private string City;
        private string State;
        private string Country;

        
        // will need to define how to assign claims to user
        private List<ClaimsPool.Claims> Claims { get; set; }

        public UserAccount()
        {
            Username = "";
            Password = "";
            Firstname = "";
            Lastname = "";
            Cityloc = "";
            Stateloc = "";
            Countryloc = "";
            this.DOB = "";
            SecurityQ = "";
            SecurityA = "";
            UserID = "";
        }
        /// <summary>
        /// Constructor to set up a user account
        /// </summary>
        /// <param name="userN">Passed user name</param>
        /// <param name="pword">Passed Password</param>
        /// <param name="fName">Passed user name</param>
        /// <param name="lName">Passed Password</param>
        /// <param name="city">Passed City</param>
        /// <param name="state">Passed State</param>
        /// <param name="country">Passed Country</param>
        /// <param name="DOB">Passed Date of Birth</param>
        /// <param name="securityQ">Passed Security Question</param>
        /// <param name="securityA">Passed Answer to Security Question</param>

        public UserAccount(string userN, string pword, string fName, string lName, string city, 
            string state, string country, string DOB, string securityQ, string securityA, string userID)
        {
            Username = userN;
            Password = pword;
            Firstname = fName;
            Lastname = lName;
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
        public string SecurityA { get; set; }
        public string UserID { get; set; }
        
#endregion

#region Overidden functions
        /// <summary>
        /// Overides equals function to compare to user account objects
        /// </summary>
        /// <param name="obj">The passed user account to be compared to</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var user = obj as UserAccount;

            if (user == null)
                return false;

            if (user.UserName != this.UserName)
                return false;

            if (user.PassWord != this.PassWord)
                return false;

            if (user.FirstName != this.FirstName)
                return false;

            if (user.Lastname != this.Lastname)
                return false;

            if (user.Cityloc != this.Cityloc)
                return false;

            if (user.Stateloc != this.Stateloc)
                return false;

            if (user.Countryloc != this.Countryloc)
                return false;

            if (user.DOB != this.DOB)
                return false;

            if (user.SecurityQ != this.SecurityQ)
                return false;

            if (user.SecurityA != this.SecurityA)
                return false;

            if (user.UserID != this.UserID)
                return false;
            return true;
        }
#endregion

        public void addClaim(ClaimsPool.Claims  claimAdded)
        {
            Claims.Add(claimAdded);
        }
    }
}