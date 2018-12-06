using GreetNGroup.Claim_Controls;
using GreetNGroup.Data_Access;
using GreetNGroup.SiteUser;
using GreetNGroup.Tokens;
using System;
using System.Collections.Generic;
using GreetNGroup;
namespace GreetNGroup.Validation
{
    public static class ValidationManager
    {
        public static void checkAddToken(List<string> claims, String userName, String city, String state, String country, DateTime DOB)
        {
            try
            {
                string temp = "test";
                List<string> _requireAdminRights = new List<string> { "AdminRights" };
                var currentUserToken = new Token(temp);
                currentUserToken.Claims = claims;
                var canAdd = ClaimsAuthorization.VerifyClaims(currentUserToken, _requireAdminRights);
                //If they have the claims they will be able to create a new account but if they don't the function will throw an error
                if (canAdd == true)
                {
                    var attributeCheck = checkAddAttributes(userName, city, state, country, DOB);
                    if (attributeCheck == true)
                    {
                        CheckQueries.CheckDuplicates(userName, city, state, country, DOB);
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
        public static void CheckDeleteToken(List<string> claims, string UID)
        {
            try
            {
                Console.WriteLine("hello");
                string temp = "test";
                List<string> _requireAdminRights = new List<string> {"AdminRights" };
                var currentUserToken = new Token(temp);
                currentUserToken.Claims = claims;
                var canDelete = ClaimsAuthorization.VerifyClaims(currentUserToken, _requireAdminRights);
                
                if (canDelete == true)
                {
                    if(CheckDeletedAttributes(UID) == true)
                    {
                        CheckQueries.CheckDeleteClaim(UID);
                    }
                    
                }
                else
                {
                    throw new System.ArgumentException("User does not have the right Claims", "Claims");
                }
            }
            catch(Exception e)
            {
                //log
            }
            
        }

        public static void CheckEnableToken(List<string> claims, string UID, Boolean changeState)
        {
            try
            {
                Console.WriteLine("hello");
                string temp = "test";
                List<string> _requireAdminRights = new List<string> {"AdminRights" };
                var currentUserToken = new Token(temp);
                currentUserToken.Claims = claims;
                var canDelete = ClaimsAuthorization.VerifyClaims(currentUserToken, _requireAdminRights);
                if (canDelete == true)
                {
                    CheckQueries.CheckEditClaim(UID, changeState);
                }
                else
                {
                    throw new System.ArgumentException("User does not have the right Claims", "Claims");
                }
            }
            catch (Exception e)
            {
                //log
            }

        }

        public static Boolean checkAddAttributes(String userName, String city, String state, String country, DateTime DOB)
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

        public static Boolean CheckDeletedAttributes(String UID)
        {
            try
            {
                if (UID.Equals(null))
                {
                    throw new System.ArgumentException("User attributes are not correct", "Attributes");
                }
                if (UID.Equals(""))
                {
                    throw new System.ArgumentException("User attributes are not correct", "Attributes");
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

        public static Boolean checkAccountEditable (List<UserClaim> items)
        {
            Console.WriteLine("hello");
            foreach (var i in items)
            {
                if(i.ClaimId.Equals("0005"))
                {
                    return false;
                }
            }
            return true;
        }

    }
}