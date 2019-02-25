using System;

namespace GreetNGroup.DataAccess.Queries
{
    public class DbInsert
    {
        /// <summary>
        /// Inserts a user into the database
        ///
        /// The function will simply add into the database -- the correct usage will be to check whether
        /// or not the user exists before allowing a call to this function -- The check is not handled in this function itself
        /// 
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="country"></param>
        /// <param name="dob"></param>
        /// <param name="securityQ"></param>
        /// <param name="securityA"></param>
        /// <param name="isActivated"></param>
        public static void InsertUser(string uId, string firstName, string lastName, string userName, string password, string city,
            string state, string country, DateTime dob, string securityQ, string securityA, bool isActivated)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var user = new User(uId, firstName, lastName, userName, password, city, state, country, dob, securityQ, securityA, isActivated);

                ctx.Users.Add(user);

                ctx.SaveChanges();
            }
        }
        
        /// <summary>
        /// Inserts a Claim into the database of claims
        /// </summary>
        /// <param name="claimId"></param>
        /// <param name="claimName"></param>
        public static void InsertClaim(int claimId, string claimName)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var claim = new Claim(claimId, claimName);

                ctx.Claims.Add(claim);

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Inserts an event into the database of events
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <param name="startDate"></param>
        /// <param name="eventName"></param>
        public static void InsertEvent(string userId, string eventId, DateTime startDate, string eventName)
        {
            using (var ctx = new GreetNGroupContext())
            {
                var userEvent = new Event(userId, eventId, startDate, eventName);

                ctx.Events.Add(userEvent);

                ctx.SaveChanges();
            }
        }
    }
}