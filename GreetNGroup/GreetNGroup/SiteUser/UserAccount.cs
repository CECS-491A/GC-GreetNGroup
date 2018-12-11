using System;
using System.Collections.Generic;
using GreetNGroup.UserManage;
using GreetNGroup.Validation;
/*
Basic user account class with all fields needed for a registered account     
*/
namespace GreetNGroup.SiteUser
{
    public class UserAccount : IIdentifiable, IUserManager
    {
        private string UserName;
        private string PassWord;
        private string FirstName;
        private string LastName;
        private string DateofBirth;
        private string City;
        private string State;
        private string Country;
        private Boolean isEnabled;
        //internal List<ClaimsPool.Claims> claimsList;

        // will need to define how to assign claims to user
        private List<string> Claims { get; set; }

        public UserAccount()
        {
            Username = "";
            Password = "";
            Firstname = "";
            Lastname = "";
            Cityloc = "";
            Stateloc = "";
            Countryloc = "";
            this.DOB = new DateTime(0, 0, 0);
            SecurityQ = "";
            SecurityA = "";
            UserID = "";
            accountLvl = 1;
            Enable = true;
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
        /// <param name="userID">Passed UserID</param>
        /// <param name="AccountLvl">Passed lvl of the account</param>
        /// <param name="isEnabled">Passed current state of account</param> 

        public UserAccount(string userN, string pword, string fName, string lName, string city, 
            string state, string country, DateTime DOB, string securityQ, string securityA, string userID, int accountLvl, Boolean isEnable)
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
            if (accountLvl == 1)
            {
                Claims = new List<string>
                {
                    "CanCreateEvents", "CanViewEvents", "CanFriendUsers", "AdminRights", "CanBlackListUsers"
                };
            }
            else
            {
                Claims = new List<string>
                {
                    "CanCreateEvents", "CanViewEvents", "CanFriendUsers"
                };
            }
            Enable = isEnable;
        }

        #region Getters and Setters
        
        #region IIdentifiable Declarations
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DOB { get; set; }
        
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
        public Boolean Enable { get; set; }
        public int accountLvl { get; set; }

        #endregion

        #region User Management
        /// <summary>
        /// Creats a new USer Account given the attributes and if the Username has not been taken.
        /// </summary>
        /// <param name="userName">New user Name</param>
        /// <param name="city">New City Location</param>
        /// <param name="state">New State Location</param>
        /// <param name="country">New Country Location</param>
        /// <param name="DOB">New user's Date of birth</param>
        /// <returns>A new User Account Object</returns>
        public void AddAccount(String userName, String city, String state, String country, DateTime DOB)
        {
            ValidationManager.checkAddToken(Claims, userName, city, state, country, DOB);

        }
        /// <summary>
        /// Checks claims of the users and returns if the user can delete and the account be deleted
        /// </summary>
        /// <param name="deleteUser">User Account that will be deletd</param>
        public void DeleteAccount(string UserID)
        {
            ValidationManager.CheckDeleteToken(Claims, UserID);
        }
        /**
        /// <summary>
        /// Enables or disables an account
        /// </summary>
        /// <param name="account">Account that is being enabled or disabled</param>
        /// <param name="changestate">Truth value of the accounts enabled status</param>
        public void ChangeEnable(string UserID, Boolean changeState)
        {
            Console.WriteLine("hello");
            ValidationManager.CheckEnableToken(Claims, UserID, changeState);
        }
        **/
        public void UpdateAccount(string UserID, List<string> changedAttributes)
        {
            ValidationManager.CheckEditToken(Claims, UserID, changedAttributes);
        }

        #endregion

        #region Overidden functions
        /// <summary>
        /// Overides equals function to compare to user account objects
        /// </summary>
        /// <param name="obj">The passed user account to be compared to</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var user1 = obj as UserAccount;
            if (user1 == null)
            {
                return false;
            }
            if ((user1.Username).Equals(Username) == false)
            {
                return false;
            }
            if ((user1.Password).Equals(Password) == false)
            {
                return false;
            }
            if ((user1.Firstname).Equals(Firstname) == false)
            {
                return false;
            }
            if ((user1.Lastname).Equals(Lastname) == false)
            {
                return false;
            }
            if ((user1.Cityloc).Equals(Cityloc) == false)
            {
                return false;
            }
            if ((user1.Stateloc).Equals(Stateloc) == false)
            {
                return false;
            }
            if ((user1.Countryloc).Equals(Countryloc) == false)
            {
                return false;
            }
            if ((user1.DOB).Equals(DOB) == false)
            {
                return false;
            }
            if ((user1.SecurityQ).Equals(SecurityQ) == false)
            {
                return false;
            }
            if ((user1.SecurityA).Equals(SecurityA) == false)
            {
                return false;
            }
            if ((user1.UserID).Equals(UserID) == false)
            {
                return false;
            }
            return true;
        }
        #endregion

        public void addClaim(string claimAdded)
        {
            Claims.Add(claimAdded);
        }
    }
}