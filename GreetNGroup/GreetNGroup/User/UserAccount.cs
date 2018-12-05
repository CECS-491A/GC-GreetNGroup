using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GreetNGroup.Claim_Controls;
using GreetNGroup.Account;
using GreetNGroup.Tokens;
using GreetNGroup.UserManage;
using GreetNGroup.Account_Fields_Random_Generator;
using GreetNGroup.User;
/*
    Basic user account class with all fields needed for a registered account     
*/
namespace GreetNGroup.Account
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

        public UserAccount(string userN, string pword, string fName, string lName, string city, 
            string state, string country, string DOB, string securityQ, string securityA, string userID, int accountLvl, Boolean isEnable)
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
                Claims = new List<ClaimsPool.Claims> { ClaimsPool.Claims.CanCreateEvents, ClaimsPool.Claims.CanViewEvents, ClaimsPool.Claims.CanFriendUsers, ClaimsPool.Claims.AdminRights, ClaimsPool.Claims.CanBlacklistUsers, };
            }
            else
            {
                Claims = new List<ClaimsPool.Claims> { ClaimsPool.Claims.CanCreateEvents, ClaimsPool.Claims.CanViewEvents, ClaimsPool.Claims.CanFriendUsers };
            }
            Enable = isEnable;
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
        public Boolean Enable { get; set; }
        public int accountLvl { get; set; }
        #endregion

        #region User Managent
        /// <summary>
        /// Creats a new USer Account given the attributes and if the Username has not been taken.
        /// </summary>
        /// <param name="userName">New user Name</param>
        /// <param name="city">New City Location</param>
        /// <param name="state">New State Location</param>
        /// <param name="country">New Country Location</param>
        /// <param name="DOB">New user's Date of birth</param>
        /// <param name="Users">The list of Current users</param>
        /// <returns>A new User Account Object</returns>

        public Object AddAccount(String userName, String city, String state, String country, String DOB, List<UserAccount> Users)
        {

            //User is first checked if they have the Admin rights claim to be able to create an account
            List<ClaimsPool.Claims> _requireAdminRights = new List<ClaimsPool.Claims> { ClaimsPool.Claims.AdminRights };
            var currentUserToken = new Token(UserID);
            currentUserToken.Claims = Claims;
            var canAdd = ClaimsAuthorization.VerifyClaims(currentUserToken, _requireAdminRights);
            //If they have the claims they will be able to create a new account but if they don't the function will throw an error
            if (canAdd == false)
            {
                throw new System.ArgumentException("User does not have the right Claims", "Claims");
            }
            //Compares username to the database and will only create an account if the username is unique
            var isDupe = DataBaseQuery.isUserNameDuplicate(userName, Users);
            if (isDupe == false)
            {
                //var randomPassword = RandomFieldGenerator.generatePassword();

                UserAccount newAccount = new UserAccount(userName, "", "", "", city, state, country, DOB, "", "", "", 0, true);
                return newAccount;
            }
            else
            {
                throw new System.ArgumentException("Username already exist", "Database");
            }

        }
        /// <summary>
        /// Checks claims of the users and returns if the user can delete and the account be deleted
        /// </summary>
        /// <param name="deleteUser">User Account that will be deletd</param>
        /// <returns>If user is able to delete the account or not</returns>
        public Boolean DeleteAccount(UserAccount deleteUser)
        {
            Boolean deletable = false;
            List<ClaimsPool.Claims> _requireAdminRights = new List<ClaimsPool.Claims> { ClaimsPool.Claims.AdminRights };
            var currentUserToken = new Token(Username);
            var deleteUserToken = new Token(deleteUser.Username);
            currentUserToken.Claims = Claims;
            deleteUserToken.Claims = deleteUser.Claims;
            var canDelete = ClaimsAuthorization.VerifyClaims(currentUserToken, _requireAdminRights);
            var canBeDelete = ClaimsAuthorization.VerifyClaims(deleteUserToken, _requireAdminRights);
            if (canDelete == false || canBeDelete == true)
            {
                throw new System.ArgumentException("One of the Users does not have the right Claims", "Claims");
            }
            else
            {
                deletable = true;
            }
            return deletable;
        }
        /// <summary>
        /// Enables or disables an account
        /// </summary>
        /// <param name="account">Account that is being enabled or disabled</param>
        /// <param name="isEnabled">Truth value of the accounts enabled status</param>
        public void ChangeEnable(UserAccount account, Boolean isEnabled)
        {
            List<ClaimsPool.Claims> _requireAdminRights = new List<ClaimsPool.Claims> { ClaimsPool.Claims.AdminRights };
            var currentUserToken = new Token(Username);
            var changeUserToken = new Token(account.Username);
            currentUserToken.Claims = Claims;
            changeUserToken.Claims = account.Claims;
            var canEnable = ClaimsAuthorization.VerifyClaims(currentUserToken, _requireAdminRights);
            var canBeEnabled = ClaimsAuthorization.VerifyClaims(changeUserToken, _requireAdminRights);
            if (canEnable == false || canBeEnabled == true)
            {
                System.Diagnostics.Debug.WriteLine("error");
                throw new System.ArgumentException("One of the Users does not have the right Claims", "Claims");
            }
            else
            {
                if (account.Enable == isEnabled)
                {
                    throw new System.ArgumentException("Account is already enabled or disabled", "Activate/Deactivate");
                }
                else
                {
                    account.Enable = isEnabled;
                }

            }

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

        public void addClaim(ClaimsPool.Claims  claimAdded)
        {
            Claims.Add(claimAdded);
        }
    }
}