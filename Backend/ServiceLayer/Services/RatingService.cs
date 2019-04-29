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

        public int GetRating(int userID)
        {
            GreetNGroupContext context = new GreetNGroupContext();
            var listOfRatingsForUser = from r in context.UserRatings
                                       where r.RatedId1 == userID
                                       select r;
            int averageRating = 0;
            foreach (UserRating rating in listOfRatingsForUser)
            {
                averageRating = averageRating + rating.Rating;
            }
            return averageRating;
        }

        public bool CreateRating(UserRating rating, string ip)
        {
            try
            {
                using (var ctx = new GreetNGroupContext())
                {
                    ctx.UserRatings.Add(rating);
                    ctx.SaveChanges();
                    LogGNGUserRating(rating.RaterId.ToString(), rating.RatedId.ToString(), ip);
                    return true;
                }
            }
            catch(Exception e) 
            {
                _gngLoggerService.LogGNGInternalErrors(e.ToString());
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
    }
}
