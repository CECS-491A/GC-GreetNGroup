using System;
using GreetNGroup.Account_Fields_Random_Generator;
using GreetNGroup.Models;
namespace GreetNGroup.Data_Access
{
    public class DataBaseInsert
    {
        /// <summary>
        /// Inserts a new user in the database given the following attributes
        /// </summary>
        /// <param name="userName">New Username</param>
        /// <param name="city">New City Location</param>
        /// <param name="state">New State Location</param>
        /// <param name="country">New Country Location</param>
        /// <param name="DOB">New Date of birth</param>
        public static void InsertUser(string userName, string city, string state, string country, DateTime DOB)
        {
            try
            {
                //Checks for duplicate user names by checking if it exists
                var isDupe = DataBaseCheck.FindUsername(userName);
                
                if(isDupe == false)
                {
                    //Generates random id
                    string uID = RandomFieldGenerator.generateID();
                    using (var ctx = new GreetNGroupContext())
                    {
                        Console.WriteLine("Insert");
                        //Generates random password
                        string password = RandomFieldGenerator.generatePassword();
                        //Creates a new user with given attributes
                        var newUser = new UserTable() { UserName = userName, Password = password, City = city, State = state, Country = country, DoB = DOB, UserId = uID };
                        //Adds to the table
                        ctx.UserTables.Add(newUser);
                        ctx.SaveChanges();
                    }
                    using (var ctx = new GreetNGroupContext())
                    {
                        //Basic Claims every user should have
                        var newClaims1 = new UserClaim() { UserId = uID, ClaimId = "0001" };
                        var newClaims2 = new UserClaim() { UserId = uID, ClaimId = "0002" };
                        var newClaims3 = new UserClaim() { UserId = uID, ClaimId = "0003" };
                        ctx.UserClaims.Add(newClaims1);
                        ctx.UserClaims.Add(newClaims2);
                        ctx.UserClaims.Add(newClaims3);
                        ctx.SaveChanges();
                    }
                }
                else
                {
                    throw new System.ArgumentException("User name already Exist", "Database");
                }              
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //Log exception e
            }
        }
        
        /// <summary>
        /// Insert claims into user table
        /// </summary>
        /// <param name="claimId"> Id to reference claim in database </param>
        /// <param name="userId"> Id to reference user in database </param>
        public static void InsertClaimsInUser(string claimId, string userId)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var claim = new UserClaim() { UserId = userId, ClaimId = claimId };
                ctx.UserClaims.Add(claim);
                ctx.SaveChanges();
            }
        }
    }
}