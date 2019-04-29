using Gucci.DataAccessLayer.Models;
using Gucci.ServiceLayer.Services;
using System;
using System.Collections.Generic;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Model;

namespace Gucci.ManagerLayer.LogManagement
{
    public class LogManager
    {
        /* Every logging method will catch FileNotFoundException explicitly
         * so we know that there is a problem when attempting to log. Let
         * other exceptions bubble up
         */

        private IErrorHandlerService _errorHandlerService;
        private ILoggerService _gngLoggerService;
        private Dictionary<string, int> listOfIDs;
        private List<GNGLog> logList = new List<GNGLog>();
        private Configurations configurations;
        private string logFileName;

        public LogManager()
        {
            _errorHandlerService = new ErrorHandlerService();
            _gngLoggerService = new LoggerService();
            listOfIDs = _gngLoggerService.GetLogIDs();
            configurations = new Configurations();
            logFileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
        }

        

        

        

        

        

        



        /// <summary>
        /// Method LogGNGJoinEvent logs when a user joins an event to partake in. The log
        /// tracks the event ID and the user who attempts to join the event. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">User ID of the person attempting to join event</param>
        /// <param name="eventID">Event ID</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if the log was successfully made</returns>
        public bool LogGNGJoinEvent(string usersID, int eventID, string ip)
        {
            _gngLoggerService.CreateNewLog(logFileName, configurations.GetLogDirectory());
            var logMade = false;
            listOfIDs.TryGetValue("EventJoined", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "User " + usersID + " joined Event " + eventID
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
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
            _gngLoggerService.CreateNewLog(logFileName, configurations.GetLogDirectory());
            var logMade = false;
            listOfIDs.TryGetValue("UserRatings", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "User " + usersID + " rated " + ratedUserID
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogGNGFindEventForMe logs when a user calls the 'find events for me'
        /// function on GreetNGroup. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">Hahsed user ID</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if the log was successfully made</returns>
        public bool LogGNGFindEventForMe(string usersID, string ip)
        {
            _gngLoggerService.CreateNewLog(logFileName, configurations.GetLogDirectory());
            var logMade = false;
            listOfIDs.TryGetValue("FindEventForMe", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "Event Searched for"
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogMaliciousAttack logs any attempted malicious attacks
        /// made by a registered or nonregistered user
        /// </summary>
        /// <param name="url">Url where the malicious attack occurred</param>
        /// <param name="ip">IP address of the attacker</param>
        /// <param name="usersID">User ID of attacker (empty string if not a registered user)</param>
        /// <returns>Returns bool based on if log was successfully made</returns>
        public bool LogMaliciousAttack(string url, string ip, string usersID)
        {
            _gngLoggerService.CreateNewLog(logFileName, configurations.GetLogDirectory());
            var logMade = false;
            listOfIDs.TryGetValue("MaliciousAttacks", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "Malicious attack attempted at " + url
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        } 

    }
}
