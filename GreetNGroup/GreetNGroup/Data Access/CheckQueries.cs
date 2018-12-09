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
                        using (var ctx2 = new GreetNGroupContext())
                        {
                            var claimslist = ctx2.UserClaims
                                         .Where(s => s.UserId == UserID).ToList();
                            Boolean canDelete = ValidationManager.checkAccountEditable(claimslist);
                            if (canDelete == true)
                            {
                                DeleteUser(UserID);
                            }
                            else
                            {
                                throw new System.ArgumentException("Account cannot be deleted", "Claim");
                            }
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
        }/// <summary>
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

        public static void CheckEditClaim(string UserID, Boolean state)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var stud = ctx.UserClaims
                                  .Where(s => s.UserId == UserID).Count();
                    if (stud > 0)
                    {
                        using (var ctx2 = new GreetNGroupContext())
                        {
                            var claimslist = ctx2.UserClaims
                                         .Where(s => s.UserId == UserID).ToList();
                            Boolean canEdit = ValidationManager.checkAccountEditable(claimslist);
                            if (canEdit)
                            {
                                ChangeState(UserID, state);
                            }
                            else
                            {
                                throw new System.ArgumentException("Account cannot be updated", "Claim");
                            }
                        }
                    }
                    else
                    {
                        throw new System.ArgumentException("User ID doesn't exist exist", "Database");
                    }

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

        public static void UpdateUser(string UserID, List<string> attributesToUpdate, List<string> attributeContents)
        {
            string firstname;
            string lastname;
            string username;
            string password;
            string city;
            string state;
            string country;
            string DOB;

            //Try statement to fill the variables with user's current attributes
            try
            {
                using (var ctx = new GreetNGroupContext())
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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //For loop to update the attributes with new values, if there are values to update it to
            for (int i = 0; i < attributesToUpdate.Count; i++)
            {
                if(attributesToUpdate[i] == "FirstName")
                {
                    firstname = attributeContents[i];
                }else if(attributesToUpdate[i] == "LastName")
                {
                    lastname = attributeContents[i];
                }else if(attributesToUpdate[i] == "UserName")
                {
                    username = attributeContents[i];
                }else if(attributesToUpdate[i] == "Password")
                {
                    password = attributeContents[i];
                }
                else if(attributesToUpdate[i] == "City")
                {
                    city = attributeContents[i];
                }else if(attributesToUpdate[i] == "State")
                {
                    state = attributeContents[i];
                }else if(attributesToUpdate[i] == "Country")
                {
                    country = attributeContents[i];
                }else if(attributesToUpdate[i] == "DOB")
                {
                    DOB = attributeContents[i];
                }
            }
            //Try statement update the user in the database
            
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