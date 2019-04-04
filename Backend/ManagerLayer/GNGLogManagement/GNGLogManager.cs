using DataAccessLayer.Models;
using Newtonsoft.Json;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace ManagerLayer.GNGLogManagement
{
    public class GNGLogManager
    {
        IErrorHandlerService _errorHandlerService = new ErrorHandlerService();
        IGNGLoggerService _gngLoggerService = new GNGLoggerService();
        private Dictionary<string, int> listOfIDs;
        private string currentLogpath;
        private static List<GNGLog> logList = new List<GNGLog>();
        public GNGLogManager()
        {
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
        /// <param name="usersID">Hashed user ID (empty if user does not exist)</param>
        /// <param name="ip">IP address of the user/guest</param>
        /// <returns>Return true or false if the log was made successfully</returns>
        public bool LogClicksMade(string startPoint, string endPoint, string usersID, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            bool logMade = false;
            listOfIDs.TryGetValue("ClickEvent", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = startPoint + " to " + endPoint
            };
            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogpath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);

                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
            return logMade;

        }

        /// <summary>
        /// Method LogErrorsEncountered logs any errors a user encountered inside GreetNGroup.
        /// The error code and url of the error encountered will be tracked inside the log. If the
        /// log was failed to be made, it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">Hashed user ID (empty if user does not exist)</param>
        /// <param name="errorCode">Error code encountered</param>
        /// <param name="urlOfErr">URL of error encountered</param>
        /// <param name="ip">IP address of the user/guest</param>
        /// <returns>Return true or false if the log was made successfully</returns>
        public bool LogErrorsEncountered(string usersID, string errorCode, string urlOfErr, string errDesc, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            bool logMade = false;
            listOfIDs.TryGetValue("ErrorEncountered", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = errorCode + " encountered at " + urlOfErr + "\n" + errDesc
            };
            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);
            //string json = JsonConvert.SerializeObject(log, Formatting.Indented);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogpath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
            return logMade;
        }

        /// <summary>
        /// Method LogGNGEventsCreated logs the events users made on GreetNGroup. The event ID
        /// and user ID of the host will be tracked. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">Hashed user ID</param>
        /// <param name="eventID">Event ID</param>
        /// <param name="ip">IP Address of user</param>
        /// <returns>Return true or false if the log was made successfully</returns>
        public bool LogGNGEventsCreated(string usersID, string eventID, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            bool logMade = false;
            listOfIDs.TryGetValue("EventCreated", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = "Event " + eventID + " created"
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogpath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
            return logMade;
        }

        /// <summary>
        /// Method LogEntryToWebsite logs when a user first enters GreetNGroup. The log
        /// will keep track of the url that the user landed on as an entrypoint. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">Hashed user ID (empty if not a registered user)</param>
        /// <param name="urlEntered">URL entry point</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if log was successfully made</returns>
        public bool LogEntryToWebsite(string usersID, string urlEntered, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            bool logMade = false;
            listOfIDs.TryGetValue("EntryToWebsite", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = usersID + " entered at " + urlEntered
            };
            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogpath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
            return logMade;
        }

        /// <summary>
        /// Method LogExitFromWebsite logs when a user exits GreetNGroup and goes off to a 
        /// URL outside of GreetNGroup. The log tracks the URL the user was last on before 
        /// exiting GreetNGroup. If the log was failed to be made, it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">Hashed user ID (blank if user is not registered)</param>
        /// <param name="urlOfExit">Last URL the user visited inside GreetNGroup</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if the log was successfully made</returns>
        public bool LogExitFromWebsite(string usersID, string urlOfExit, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            bool logMade = false;
            listOfIDs.TryGetValue("ExitFromWebsite", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = usersID + " exited website from " + urlOfExit
            };
            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogpath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
            return logMade;
        }

        /// <summary>
        /// Method LogAccountDeletion logs when a user deletes their GreetNGroup account.
        /// </summary>
        /// <param name="usersID">Hashed user ID</param>
        /// <param name="ip">IP address</param>
        /// <returns>Returns true or false if log was successfully made</returns>
        public bool LogAccountDeletion(string usersID, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            bool logMade = false;
            listOfIDs.TryGetValue("ErrorEncountered", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = usersID + " deleted account"
            };
            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogpath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
            return logMade;
        }

        /// <summary>
        /// Method LogGNGSearchForUser logs when a user searches for another user. The log
        /// tracks the search entry the user made. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">Hashed user ID</param>
        /// <param name="searchedUser">Search entry</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if the log was successfully made</returns>
        public bool LogGNGSearchForUser(string usersID, string searchedUser, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            bool logMade = false;
            listOfIDs.TryGetValue("SearchForUser", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = "User searched for " + searchedUser
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogpath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
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
        public bool LogGNGJoinEvent(string usersID, string eventID, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            bool logMade = false;
            listOfIDs.TryGetValue("EventJoined", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = "User " + usersID + " joined Event " + eventID
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogpath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
            return logMade;
        }

        /// <summary>
        /// Method LogGNGUserRating logs when a user rates another user. The log tracks
        /// the rater and the ratee's user ID. If the log was failed to be made, 
        /// it will increment the errorCounter.
        /// </summary>
        /// <param name="usersID">Hashed user ID of the rater</param>
        /// <param name="ratedUserID">Hashed user ID of the ratee</param>
        /// <param name="ip">IP Address</param>
        /// <returns>Returns true or false if the logwas successfully made or not</returns>
        public bool LogGNGUserRating(string usersID, string ratedUserID, string ip)
        {
            _gngLoggerService.CreateNewLog();
            currentLogpath = _gngLoggerService.GetCurrentLogPath();
            bool logMade = false;
            listOfIDs.TryGetValue("UserRatings", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = "Rated " + ratedUserID
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogpath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
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
            bool logMade = false;
            listOfIDs.TryGetValue("FindEventForMe", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = "Event Searched for"
            };

            logList = _gngLoggerService.FillCurrentLogsList();
            logList.Add(log);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogpath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
            return logMade;
        }
    }
}
