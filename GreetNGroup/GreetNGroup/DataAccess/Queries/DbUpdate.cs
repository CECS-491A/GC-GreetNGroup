using System;
using System.Linq;
using GreetNGroup.DataAccess.Tables;

namespace GreetNGroup.DataAccess.Queries
{
    public class DbUpdate
    {
        /// <summary>
        /// The following region handles update of user information
        /// User is grabbed from the database via userId and the information is updated
        /// using the information supplied in the arguments
        /// </summary>
         #region "Update User Info"

        public static void UpdateUserPassword(int uId, string password)
        {
            using (var ctx = new GreetNGroupContext())
            {
                User curUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(uId));
                if (curUser != null)
                    curUser.Password = password;
                ctx.SaveChanges();
            }
        }

        public static void UpdateUserCity(int uId, string city)
        {
            using (var ctx = new GreetNGroupContext())
            {
                User curUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(uId));
                if (curUser != null)
                    curUser.City = city;
                ctx.SaveChanges();
            }
        }
        
        public static void UpdateUserState(int uId, string state)
        {
            using (var ctx = new GreetNGroupContext())
            {
                User curUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(uId));
                if (curUser != null)
                    curUser.State = state;
                ctx.SaveChanges();
            }
        }

        public static void UpdateUserCountry(int uId, string country)
        {
            using (var ctx = new GreetNGroupContext())
            {
                User curUser = ctx.Users.FirstOrDefault(c => c.UserId.Equals(uId));
                if (curUser != null)
                    curUser.Country = country;
                ctx.SaveChanges();
            }
        }
        #endregion

        /// <summary>
        /// The following region handles update of Event specific information -- found via eventId
        /// </summary>
        /// <param name="eId"></param>
        /// <param name="startDate"></param>
        #region "Update Event Information"

        public static void UpdateEventStartDate(int eId, DateTime startDate)
        {
            using (var ctx = new GreetNGroupContext())
            {
                Event curEvent = ctx.Events.FirstOrDefault(c => c.EventId.Equals(eId));
                if (curEvent != null)
                    curEvent.StartDate = startDate;
                ctx.SaveChanges();
            }
        }

        #endregion
    }  
}