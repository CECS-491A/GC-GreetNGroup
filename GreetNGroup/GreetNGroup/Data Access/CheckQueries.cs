using GreetNGroup.Account_Fields_Random_Generator;
using GreetNGroup.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreetNGroup.Data_Access
{
    public static class CheckQueries
    {
        /// <summary>
        /// Checks for duplicate usernames in the database
        /// </summary>
        /// <param name="userName">New Username</param>
        /// <param name="city">New City Location</param>
        /// <param name="state">New State Location</param>
        /// <param name="country">New Country Location</param>
        /// <param name="DOB">New Date of birth</param>
        public static void CheckDuplicates(String userName, String city, String state, String country, DateTime DOB)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var stud = ctx.UserTables
                                  .Where(s => s.UserName == userName).Any();
                    Console.WriteLine(stud);
                    if(stud == false)
                    {
                        InsertUser(userName, city, state,country,DOB);
                    }
                    else
                    {
                        throw new System.ArgumentException("Name already exist", "Database");
                    }

                }
            }
            catch (Exception e)
            {
                //Log Excepetion
                //Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Checks the claims of the account that is going to be deleted
        /// </summary>
        /// <param name="UID">Delete account user ID</param>
        public static void CheckDeleteClaim(string UserID)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var stud = ctx.UserClaims
                                  .Where(s => s.UserId == UserID).Count();
                    if (stud > 0)
                    {

                        List<string> checkClaims = DataBaseQueries.FindClaimsFromUser(UserID);
                        Boolean canDelete = ValidationManager.checkAccountEditable(checkClaims);
                        if (canDelete == true)
                        {
                            DeleteUser(UserID);
                        }
                        else
                        {
                            throw new System.ArgumentException("Account cannot be deleted", "Claim");
                        }
                        
                    }
                    else
                    {
                        throw new System.ArgumentException("user ID doesn't exist exist", "Database");
                    }

                }
            }
            catch (Exception e)
            {
                //Log Excepetion
                Console.WriteLine(e);
            }
        }
        /**
        /// <summary>
        /// Checks to see of the account you want to edit is editable
        /// </summary>
        /// <param name="UID">User ID</param>
        /// <param name="changeState">The state of isActivated</param>
        public static void CheckStateClaim(string UserID, Boolean changeState)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var stud = ctx.UserClaims
                                  .Where(s => s.UserId == UserID).Count();
                    if (stud > 0)
                    {
                        var claimslist = ctx.UserClaims
                                        .Where(s => s.UserId == UserID).ToList();
                        Boolean canEdit = ValidationManager.checkAccountEditable(claimslist);
                        if (canEdit == true)
                        {
                            var currentState = ctx.UserTables
                                            .Where(s => s.UserId == UserID).Single();
                            if(currentState.isActivated == changeState)
                            {
                                throw new System.ArgumentException("Account cannot not be changed to same state", "State Attribute");
                            }
                            else
                            {
                                ChangeState(UserID, changeState);
                            }
                           
                        }
                        else
                        {
                            throw new System.ArgumentException("Account cannot be edited", "Claim");
                        }
                        
                    }
                    else
                    {
                        throw new System.ArgumentException("user ID doesn't exist exist", "Database");
                    }

                }
            }
            catch (Exception e)
            {
                //Log Excepetion
                Console.WriteLine(e);
            }
        }
    **/
        public static void CheckEditClaim(string UserID, List<string> attributeContents)

        {
            try
            {

                var stud = DataBaseQueries.FindClaimsFromUser(UserID);
                Boolean canEdit = ValidationManager.checkAccountEditable(stud);
                if (canEdit)
                {

                    UpdateUser(UserID, attributeContents);

                }
                else
                {
                    throw new System.ArgumentException("Account cannot be updated", "Claim");
                }
   
            }
            catch (Exception e)
            {
                //Log Excepetion
                Console.WriteLine(e);
            }
        }


        /// <summary>
        /// Inserts a new user in the database given the following attributes
        /// </summary>
        /// <param name="userName">New Username</param>
        /// <param name="city">New City Location</param>
        /// <param name="state">New State Location</param>
        /// <param name="country">New Country Location</param>
        /// <param name="DOB">New Date of birth</param>
        public static void InsertUser(String userName, String city, String state, String country, DateTime DOB)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    string UID = RandomFieldGenerator.generatePassword();
                    var newUser = new UserTable() { UserName = userName, City = city, State = state, Country = country, DoB = DOB, UserId = UID };
                    var newClaims1 = new UserClaim() { UserId = UID, ClaimId = "0001" };
                    var newClaims2 = new UserClaim() { UserId = UID, ClaimId = "0002" };
                    var newClaims3 = new UserClaim() { UserId = UID, ClaimId = "0003" };
                    ctx.UserTables.Add(newUser);
                    ctx.UserClaims.Add(newClaims1);
                    ctx.UserClaims.Add(newClaims2);
                    ctx.UserClaims.Add(newClaims3);
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //Log excepetion e
            }
        }
        /// <summary>
        /// Deletes a user in the database given the following UID
        /// </summary>
        /// <param name="UID">User ID</param>
        public static void DeleteUser(String UserID)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var Userclaims = ctx.UserClaims
                                   .Where(s => s.UserId == UserID);
                    var stud = ctx.UserTables
                                   .Where(s => s.UserId == UserID).Single();
                    ctx.UserClaims.RemoveRange(Userclaims);
                    ctx.UserTables.Remove(stud);
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //Log excepetion e
                Console.WriteLine(e);
            }
        }

        public static void UpdateUser(string UserID, List<string> attributeContents)
        {
            var ctx = new GreetNGroupContext();
            string firstname = "";
            string lastname = "";
            string username = "";
            string password = "";
            string city = "";
            string state = "";
            string country = "";
            string DOB = "";
            string securityquestion = "";
            string securityanswer = "";
            string isactivated = "";
            //Try statement to fill the variables with user's current attributes
            try
            {
                using (ctx)
                {
                    var userToUpdate = ctx.UserTables
                                   .Where(s => s.UserId == UserID).Single();
                    firstname = userToUpdate.FirstName;
                    lastname = userToUpdate.LastName;
                    username = userToUpdate.UserName;
                    password = userToUpdate.Password;
                    city = userToUpdate.City;
                    state = userToUpdate.State;
                    country = userToUpdate.Country;
                    DOB = userToUpdate.DoB.ToString();
                    securityquestion = userToUpdate.SecurityQuestion;
                    securityanswer = userToUpdate.SecurityAnswer;
                    isactivated = userToUpdate.isActivated.ToString();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //For loop to update the attributes with new values, if there are values to update it to
            for (int i = 0; i < attributeContents.Count; i++)
            {
                if (i == 0 && attributeContents[i] != null)
                {
                    firstname = attributeContents[i];
                }
                else if (i == 1 && attributeContents[i] != null)
                {
                    lastname = attributeContents[i];
                }
                else if (i == 2 && attributeContents[i] != null)
                {
                    username = attributeContents[i];
                }
                else if (i == 3 && attributeContents[i] != null)
                {
                    password = attributeContents[i];
                }
                else if (i == 4 && attributeContents[i] != null)
                {
                    city = attributeContents[i];
                }
                else if (i == 5 && attributeContents[i] != null)
                {
                    state = attributeContents[i];
                }
                else if (i == 6 && attributeContents[i] != null)
                {
                    country = attributeContents[i];
                }
                else if (i == 7 && attributeContents[i] != null)
                {
                    DOB = attributeContents[i];
                }
                else if (i == 8 && attributeContents[i] != null)
                {
                    securityquestion = attributeContents[i];
                }
                else if (i == 9 && attributeContents[i] != null)
                {
                    securityanswer = attributeContents[i];
                }
                else if (i == 10 && attributeContents[i] != null)
                {
                    isactivated = attributeContents[i];
                }
            }
            //Try statement update the user in the database
            try
            {
                using (ctx)
                {
                    var userToUpdate = ctx.UserTables
                                   .Where(s => s.UserId == UserID).Single();
                    userToUpdate.FirstName = firstname;
                    userToUpdate.LastName = lastname;
                    userToUpdate.UserName = username;
                    userToUpdate.Password = password;
                    userToUpdate.City = city;
                    userToUpdate.State = state;
                    userToUpdate.Country = country;
                    userToUpdate.DoB = Convert.ToDateTime(DOB);
                    userToUpdate.SecurityQuestion = securityquestion;
                    userToUpdate.SecurityAnswer = securityanswer;
                    userToUpdate.isActivated = isactivated.Equals("true");
                    ctx.SaveChanges();
                }
                
            }
            catch(Exception e)
            {
                //log
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uID"></param>
        /// <param name="changeState"></param>
        public static void ChangeState(string UserID, Boolean changeState)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var stud = ctx.UserTables
                                   .Where(s => s.UserId == UserID).Single();
                    stud.isActivated = changeState;
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                //Log excepetion e
                Console.WriteLine(e);
            }
        }
    }
}