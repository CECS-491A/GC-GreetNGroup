using GreetNGroup.Claim_Controls;
using GreetNGroup.Data_Access;
using GreetNGroup.Tokens;
using System;
using System.Collections.Generic;
namespace GreetNGroup.Validation
{
    public static class ValidationManager
    {
        /// <summary>
        /// Checks to see if the person who is creating an accout is allowed to
        /// </summary>
        /// <param name="userName">New user Name</param>
        /// <param name="city">New City Location</param>
        /// <param name="state">New State Location</param>
        /// <param name="country">New Country Location</param>
        /// <param name="DOB">New user's Date of birth</param>
        public static void CheckAddToken(List<string> claims, string userName, string city, string state, string country, DateTime DOB)
        {
            try
            {
                //Check the required claims
                string temp = "test";
                var requireAdminRights = new List<string> { "AdminRights" };
                var currentUserToken = new Token(temp);
                
                
                // Don't do this
                currentUserToken.Claims = claims;
                
                
                
                //Compare current user's claim
                var canAdd = ClaimsAuthorization.VerifyClaims(currentUserToken, requireAdminRights);
                //If they have the claims they will be able to create a new account but if they don't the function will throw an error
                if (canAdd == true)
                {
                    //validate the passing attributes
                    var attributeCheck = CheckAddAttributes(userName, city, state, country, DOB);
                    if (attributeCheck == true)
                    {
                        //Insert the user in the database
                        DataBaseInsert.InsertUser(userName, city, state, country, DOB);
                    }
                    else
                    {
                        throw new System.ArgumentException("User attributes are not formatted correctly", "Attributes");
                    }

                }
                else
                {
                    throw new System.ArgumentException("User does not have the right Claims", "Claims");
                }
            }
            catch(Exception e)
            {
                //Log
            }
            

        }
        
        /// <summary>
        /// Checks to see if the person who is deleting an account has the right claims
        /// </summary>
        /// <param name="claims">List of claims</param>
        /// <param name="uID">User ID </param>
        public static void CheckDeleteToken(List<string> claims, string uID)
        {
            try
            {
                //Check the required claims
                Console.WriteLine("hello");
                string temp = "test";
                List<string> requireAdminRights = new List<string> {"AdminRights" };
                var currentUserToken = new Token(temp);
                currentUserToken.Claims = claims;
                var canDelete = ClaimsAuthorization.VerifyClaims(currentUserToken, requireAdminRights);
                
                if (canDelete == true)
                {
                    //Check the passed userid
                    if(CheckDeletedAttributes(uID) == true)
                    {
                        DataBaseDelete.DeleteUser(uID);
                    }
                    
                }
                else
                {
                    throw new System.ArgumentException("User does not have the right Claims");
                }
            }
            catch(Exception e)
            {
                //log
            }
            
        }
        
        public static void CheckEditToken(List<string> claims, string UserID, List<string> attributeContents) { 
            try
            {
                //Check current user's claims
                Console.WriteLine("Updating User");
                var temp = "test";
                var requireAdminRights = new List<string> { "AdminRights" };
                var currentUserToken = new Token(temp);
                currentUserToken.Claims = claims;
                var canEdit = ClaimsAuthorization.VerifyClaims(currentUserToken, requireAdminRights);
                
                //Check the passed list of attributes
                if (canEdit && CheckEditAttributes(attributeContents))
                {
                    //Check edited account claims
                    DataBaseUpdate.TryUpdateUser(UserID, attributeContents);
                }
            }
            catch (Exception e)
            {
                //log
            }
        }

        /// <summary>
        /// Checks the current attributes of a new user account
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="country"></param>
        /// <param name="DOB"></param>
        /// <returns>Whether the inputs are valid or not</returns>
        public static bool CheckAddAttributes(string userName, string city, string state, string country, DateTime DOB)
        {
            if(userName.Equals(null) || city.Equals(null) || state.Equals(null) || country.Equals(null) || DOB == null)
            {
                throw new System.ArgumentException("User attributes are not correct", "Attributes");
            }
            if (userName.Equals("") || city.Equals("") || state.Equals("") || country.Equals(""))
            {
                throw new System.ArgumentException("User attributes are not correct", "Attributes");
            }
            //Validates Input
            return true;
        }

        /// <summary>
        /// Verifies the userId and makes sure its valid
        /// </summary>
        /// <param name="UID">The passed userid</param>
        /// <returns>If the input is valid or not</returns>
        public static bool CheckDeletedAttributes(string UID)
        {
            try
            {
                if (UID.Equals(null))
                {
                    throw new System.ArgumentException("User attributes are not correct null", "Attributes");
                }
                if (UID.Equals(""))
                {
                    throw new System.ArgumentException("User attributes are not correct empty string", "Attributes");
                }
            }
           catch(Exception e)
            {
                //Log error
                return false;
            }
            //Validates Input
            return true;
        }
        
        /// <summary>
        /// Checks the current attributes of a new user account
        /// </summary>
        /// <param name="attributeContents"></param>
        /// <returns>Whether the inputs are valid or not</returns>
        public static bool CheckEditAttributes(List<string> attributeContents)
        {
            foreach(string i in attributeContents)
            {
                if (string.IsNullOrWhiteSpace(i))
                {
                    //Log "User attributes cannot be empty"
                    return false;
                }
            }
            //Validates Input
            return true;
        }
    }
}