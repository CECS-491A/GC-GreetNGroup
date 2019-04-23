using DataAccessLayer.Models;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using ServiceLayer.Interface;

namespace ManagerLayer.GNGLogManagement
{
    public class GNGLogManager
    {
        /* Every logging method will catch FileNotFoundException explicitly
         * so we know that there is a problem when attempting to log. Let
         * other exceptions bubble up
         */

        private IErrorHandlerService _errorHandlerService;
        private IGNGLoggerService _gngLoggerService;
        private Dictionary<string, int> listOfIDs;
        private string currentLogpath;
        private List<GNGLog> logList = new List<GNGLog>();

        public GNGLogManager()
        {
            _errorHandlerService = new ErrorHandlerService();
            _gngLoggerService = new GNGLoggerService();
            listOfIDs = _gngLoggerService.GetLogIDs();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
        }

        /// <summary>
        /// Method LogClicksMade logs a user navigating around GreetNGroup based on the
        /// url they started at and the url they ended at inside GreetNGroup. If the log
        /// failed to be made, it will increment the errorCounter.
        /// </summary>
        /// <param name="startPoint">Starting URL</param>
        /// <param name="endPoint">Ending URL</param>
        /// <param name="usersID">user ID (empty if user does not exist)</param>
        /// <param name="ip">IP address of the user/guest</param>
        /// <returns>Return true or false if the log was made successfully</returns>
        public bool LogClicksMade(string startPoint, string endPoint, string usersID, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("ClickEvent", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = startPoint + " to " + endPoint
            };
            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;

        }

        /// <summary>
        /// Method LogErrorsEncountered logs any errors a user encountered inside GreetNGroup.
        /// The error code and url of the error encountered will be tracked inside the log. If the
        /// log was failed to be made, it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">user ID (empty if user does not exist)</param>
        /// <param name="errorCode">Error code encountered</param>
        /// <param name="urlOfErr">URL of error encountered</param>
        /// <param name="ip">IP address of the user/guest</param>
        /// <returns>Return true or false if the log was made successfully</returns>
        public bool LogErrorsEncountered(string usersID, string errorCode, string urlOfErr, string errDesc, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("ErrorEncountered", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = errorCode + " encountered at " + urlOfErr + "\n" + errDesc
            };
            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogGNGEventsCreated logs the events users made on GreetNGroup. The event ID
        /// and user ID of the host will be tracked. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">user ID</param>
        /// <param name="eventID">Event ID</param>
        /// <param name="ip">IP Address of user</param>
        /// <returns>Return true or false if the log was made successfully</returns>
        public bool LogGNGEventsCreated(string usersID, int eventID, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("EventCreated", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "Event " + eventID + " created"
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogEntryToWebsite logs when a user first enters GreetNGroup. The log
        /// will keep track of the url that the user landed on as an entrypoint. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">user ID (empty if not a registered user)</param>
        /// <param name="urlEntered">URL entry point</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if log was successfully made</returns>
        public bool LogEntryToWebsite(string usersID, string urlEntered, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("EntryToWebsite", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "User " + usersID + " entered at " + urlEntered
            };
            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogExitFromWebsite logs when a user exits GreetNGroup and goes off to a 
        /// URL outside of GreetNGroup. The log tracks the URL the user was last on before 
        /// exiting GreetNGroup. If the log was failed to be made, it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">user ID (blank if user is not registered)</param>
        /// <param name="urlOfExit">Last URL the user visited inside GreetNGroup</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if the log was successfully made</returns>
        public bool LogExitFromWebsite(string usersID, string urlOfExit, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("ExitFromWebsite", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "User " + usersID + " exited website from " + urlOfExit
            };
            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogAccountDeletion logs when a user deletes their GreetNGroup account.
        /// </summary>
        /// <param name="usersID">user ID</param>
        /// <param name="ip">IP address</param>
        /// <returns>Returns true or false if log was successfully made</returns>
        public bool LogAccountDeletion(string usersID, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("AccountDeletion", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "User " + usersID + " deleted account"
            };
            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogGNGSearchAction logs when a user searches for another user or event. The log
        /// tracks the search entry the user made. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">user ID</param>
        /// <param name="searchedItem">Search entry</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if the log was successfully made</returns>
        public bool LogGNGSearchAction(string usersID, string searchedItem, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("SearchAction", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "User searched for " + searchedItem
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
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
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("EventJoined", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
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
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("UserRatings", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
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
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("FindEventForMe", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
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
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("MaliciousAttacks", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "Malicious attack attempted at " + url
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogGNGEventUpdate logs when a user updates their GNG event info
        /// </summary>
        /// <param name="eventId">Event id of the event</param>
        /// <param name="userHostId">User id of the host</param>
        /// <param name="ip">IP address of the host</param>
        /// <returns>Returns a bool based on if the log was successfully made or not</returns>
        public bool LogGNGEventUpdate(int eventId, string userHostId, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("EventUpdated", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = userHostId,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "Event " + eventId + " updated"
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// LogGNGEventJoined logs when a user successfully joins a GNG event to participate in.
        /// </summary>
        /// <param name="joinedUserId">User ID of user who joins the event</param>
        /// <param name="eventId">Event ID of the event the user joined</param>
        /// <param name="ip">IP Address of the user</param>
        /// <returns>Returns a bool based on if the log was successfully made</returns>
        public bool LogGNGEventJoined(string joinedUserId, int eventId, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("EventJoined", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = joinedUserId,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "User " + joinedUserId + " joined event " + eventId
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogBadRequest logs when a user gets a bad request code from the controller
        /// </summary>
        /// <param name="usersID">UserID who received the bad request (Blank if user is not registered)</param>
        /// <param name="ip">IP of the user</param>
        /// <param name="url">Url of where the bad request occurred</param>
        /// <param name="exception">Exception message</param>
        /// <returns></returns>
        public bool LogBadRequest(string usersID, string ip, string url, string exception)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("BadRequest", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "Bad request at " + url + ": " + exception
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogGNGEventDeleted logs when a user deletes their GNG event
        /// </summary>
        /// <param name="hostId">User ID of the host</param>
        /// <param name="eventId">Event ID of the event being deleted</param>
        /// <param name="ip">IP Address of the host</param>
        /// <returns>Returns a bool based on if it was logged successfully</returns>
        public bool LogGNGEventDeleted(string hostId, int eventId, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("EventDeleted", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = hostId,
                IpAddress = ip,
                DateTime = DateTime.Now.ToString(),
                Description = "Event " + eventId + " deleted"
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary>
        /// Method LogGNGEventExpiration logs when an event has passed and can no longer
        /// be joined
        /// </summary>
        /// <param name="hostId">User ID of the host who created the event</param>
        /// <param name="eventId">Event ID of the event that expired</param>
        /// <returns>Returns a bool based on if it was logged successfully or not</returns>
        public bool LogGNGEventExpiration(string hostId, int eventId)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            var logMade = false;
            listOfIDs.TryGetValue("EventExpired", out int logID);
            var logIDString = logID.ToString();
            var log = new GNGLog
            {
                LogID = logIDString,
                UserID = hostId,
                IpAddress = "",
                DateTime = DateTime.Now.ToString(),
                Description = "Event " + eventId + " expired"
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);

            logMade = _gngLoggerService.WriteGNGLogToFile(logList);

            return logMade;
        }
    }
}
