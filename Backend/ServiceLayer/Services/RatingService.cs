using Gucci.DataAccessLayer.Context;
using Gucci.DataAccessLayer.Tables;
using Gucci.DataAccessLayer.Models;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gucci.ServiceLayer.Services
{
    public class RatingService
    {
        private ILoggerService _gngLoggerService;
        private Configurations configurations;

        public RatingService()
        {
            _gngLoggerService = new LoggerService();
            configurations = new Configurations();
        }

        public double GetRating(int userID)
        {
            GreetNGroupContext context = new GreetNGroupContext();
            var listOfRatingsForUser = from r in context.UserRatings
                                       where r.RatedId1 == userID
                                       select r;
            var totalRating = 0.0;
            var totalRaters = 0.0;
            // Iterate thorugh list of ratings and get total rating and raters
            foreach (UserRating rating in listOfRatingsForUser)
            {
                totalRating = totalRating + rating.Rating;
                if(rating.Rating != 0)
                {
                    totalRaters++;
                }
            }
            if(totalRaters == 0)
            {
                return 0;
            }
            var average = totalRating / totalRaters;
            average = Math.Round(average, 2);
            average = average * 100;
            return average;
        }

        public bool CreateRating(UserRating rating, string ip)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    ctx.UserRatings.Add(rating);
                    ctx.SaveChanges();
                    // LogGNGUserRating(rating.RaterId.ToString(), rating.RatedId.ToString(), ip);
                    return true;
                }
            }
            catch(Exception e) 
            {
                _gngLoggerService.LogGNGInternalErrors(e.ToString());
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Method LogGNGUserRating logs when a user rates another user. The log tracks
        /// the rater and the ratee's user ID. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">user ID of the rater</param>
        /// <param name="ratedUserID">user ID of the ratee</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if the logwas successfully made or not</returns>
        public bool LogGNGUserRating(string usersID, string ratedUserID, string ip)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            _gngLoggerService.CreateNewLog(fileName, configurations.GetLogDirectory()); var logMade = false;
            var log = new GNGLog
            {
                LogID = "UserRatings",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "User " + usersID + " rated " + ratedUserID
            };

            var logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }
        /// <summary>
        /// Returns boolean if rating aleady exist in the databse
        /// </summary>
        /// <param name="raterID">Rater user ID</param>
        /// <param name="ratedID">the rated user ID</param>
        /// <returns>boolean if exist or not</returns>
        public bool GetRating(int raterID, int ratedID)
        {
            var userRating = false;
            using (var context = new GreetNGroupContext())
            {
                userRating = context.UserRatings.Any(s => s.RaterId1 == raterID && s.RatedId1 == ratedID);
            }
            return userRating;
        }
        /// <summary>
        /// Updates current user rating with a new rating
        /// </summary>
        /// <param name="rating">new Rating information</param>
        /// <param name="ip">user ip</param>
        /// <returns></returns>
        public bool UpdateRating(UserRating rating, string ip)
        {
            var userRating = new UserRating();

            try
            {
                using (var context = new GreetNGroupContext())
                {
                    var query = context.UserRatings.Where(s => s.RaterId1 == rating.RaterId1 && s.RatedId1 == rating.RatedId1).FirstOrDefault<UserRating>();
                    if (query.Rating == rating.Rating)
                    {
                        query.Rating = 0;
                    }
                    else
                    {
                        query.Rating = rating.Rating;
                    }
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                _gngLoggerService.LogGNGInternalErrors(e.ToString());
                return false;
            }
        }
    }
}
