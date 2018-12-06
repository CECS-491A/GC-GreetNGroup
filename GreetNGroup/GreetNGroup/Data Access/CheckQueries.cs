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
            catch (Exception e )
            {
                //Log excepetion e
            }
            
            
        }
        /// <summary>
        /// Checks the claims of the account that is going to be deleted
        /// </summary>
        /// <param name="UID">Delete account user ID</param>
        public static void CheckDeleteClaim(string UID)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var stud = ctx.UserClaims
                                  .Where(s => s.UserId == UID).Count();
                    if (stud > 0)
                    {
                        using (var ctx2 = new GreetNGroupContext())
                        {
                            var claimslist = ctx2.UserClaims
                                         .Where(s => s.UserId == UID).ToList();
                            Boolean canDelete = ValidationManager.checkAccountEditable(claimslist);
                            if (canDelete == true)
                            {
                                DeleteUser(UID);
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
        public static void CheckEditClaim(string UID, Boolean changeState)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var stud = ctx.UserClaims
                                  .Where(s => s.UserId == UID).Count();
                    if (stud > 0)
                    {
                        var claimslist = ctx.UserClaims
                                        .Where(s => s.UserId == UID).ToList();
                        Boolean canEdit = ValidationManager.checkAccountEditable(claimslist);
                        if (canEdit == true)
                        {
                            var currentState = ctx.UserTables
                                            .Where(s => s.UserId == UID).Single();
                            if(currentState.isActivated == changeState)
                            {
                                throw new System.ArgumentException("Account cannot not be changed to same state", "State Attribute");
                            }
                            else
                            {
                                ChangeState(UID, changeState);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uID"></param>
        /// <param name="changeState"></param>
        public static void ChangeState(string uID, Boolean changeState)
        {

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var stud = ctx.UserTables
                                   .Where(s => s.UserId == uID).Single();
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

        /// <summary>
        /// Deletes a user in the database given the following UID
        /// </summary>
        /// <param name="UID">User ID</param>
        public static void DeleteUser(String UID)
        {

            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    var Userclaims = ctx.UserClaims
                                   .Where(s => s.UserId == UID);
                    var stud = ctx.UserTables
                                   .Where(s => s.UserId == UID).Single();
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
    }
}