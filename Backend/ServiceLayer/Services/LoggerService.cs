using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Gucci.DataAccessLayer.Models;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Model;
using System.Globalization;

namespace Gucci.ServiceLayer.Services
{
    public class LoggerService : ILoggerService
    {

        /* Every logging method will catch FileNotFoundException explicitly
         * so we know that there is a problem when attempting to log. Let
         * other exceptions bubble up
         */

        private IErrorHandlerService _errorHandlerService;
        private string currentLogPath;
        private Configurations configurations;

        public LoggerService()
        {
            _errorHandlerService = new ErrorHandlerService();
            configurations = new Configurations();
        }

        /// <summary>
        /// Author: Jonalyn
        /// Method CheckForExistingLog checks if a log for today already exists. If
        /// a log already exists for the current date, it will set the existing log as
        /// the current log
        /// </summary>
        /// <returns>Returns true or false if log exists or not</returns>
        public bool CheckForExistingLog(string fileName, string directory)
        {
            var logExists = false;
            try
            {
                //Format current day like this to prevent errors with using forward slashes
                var currentDate = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat());
                logExists = File.Exists(directory + fileName);
            }
            /* Catch FileNotFound explicitly as it catches errors where it cannot find the log
             * Let other exceptions bubble up
             */ 
            catch (FileNotFoundException e)
            {
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }

            return logExists;
        }

        /// <summary>
        /// Author: Jonalyn
        /// Method CreateNewLog creates a new log if a log does 
        /// not exist for the current date
        /// </summary>
        public bool CreateNewLog(string fileName, string directory)
        {
            var isSuccessfulCreate = false;
            var currentDate = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat());

            // If drive space is less than the minimum specified or is not available, return false
            if (configurations.GetDriveInfo().AvailableFreeSpace < configurations.GetMinBytes()
                || !configurations.GetDriveInfo().IsReady)
            {
                _errorHandlerService.IncrementErrorOccurrenceCount("Insufficient memory in drive to create new log");
                return isSuccessfulCreate;
            }

            if (CheckForExistingLog(fileName, directory) == false)
            {
                try
                {
                    using (var fileStream = new FileStream(directory + fileName,
                    FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        currentLogPath = directory + fileName;
                        fileStream.Close();
                    }
                    isSuccessfulCreate = true;
                    return isSuccessfulCreate;
                }
                /* Catch IOException esxplicitly when it fails to create a log
                 * Let other errors bubble up
                 */ 
                catch (IOException e)
                {
                    _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
                    return isSuccessfulCreate;
                }

            }
            else
            {
                currentLogPath = directory + fileName;
                return isSuccessfulCreate;
            }
        }

        public string GetCurrentLogPath()
        {
            return currentLogPath;
        }

        /// <summary>
        /// Author: Dylan
        /// Read the current logs for the day
        /// </summary>
        public List<GNGLog> FillCurrentLogsList()
        {
            var logList = new List<GNGLog>();
            //Check to see if file is empty
            if (new FileInfo(currentLogPath).Length != 0)
            {
                using (var r = new StreamReader(currentLogPath))
                {
                    var jsonFile = r.ReadToEnd();
                    //Retrieve Current Logs
                    logList = JsonConvert.DeserializeObject<List<GNGLog>>(jsonFile);
                    r.Close();
                }
            }
            return logList;
        }

        /// <summary>
        /// Author: Dylan
        /// Reads all json files in directory and deserializes into GNGlog and puts them all into a list
        /// </summary>
        /// <returns></returns>
        public List<GNGLog> ReadLogs()
        {
            var logList = new List<GNGLog>();
            var di = new DirectoryInfo(configurations.GetLogDirectory());
            var dirs = Directory.GetFiles(configurations.GetLogDirectory(), "*.json");
            foreach (var dir in dirs)
            {
                if (new FileInfo(dir).Length != 0)
                {
                    using (var r = new StreamReader(dir))
                    {
                        string jsonFile = r.ReadToEnd();
                        //Retrieve Current Logs
                        List<GNGLog> logs = JsonConvert.DeserializeObject<List<GNGLog>>(jsonFile);
                        for (int index = 0; index < logs.Count; index++)
                        {
                            logList.Add(logs[index]);
                        }
                        r.Close();
                    }
                }
            }
            return logList;
        }

        /// <summary>
        /// Author: Jonalyn
        /// Method LogGNGInternalErrors logs errors that occur on the backend of GNG. Any errors 
        /// in logging will increment the error counter in the error handler. Should this continue to
        /// cause errors, the system admin will be contacted with the error message.
        /// </summary>
        /// <param name="exception">The exception that was caught in string form</param>
        /// <returns>Returns true or false if the log was successfully made or not</returns>
        public bool LogGNGInternalErrors(string exception)
        {
            var fileName = configurations.GetDateTimeFormat() + configurations.GetLogExtention();
            CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                //Don't care about user id or ip address for internal error log
                LogID = "InternalErrors",
                UserID = "N/A",
                IpAddress = "N/A",
                DateTime = DateTime.UtcNow.ToString(),
                Description = "Internal errors occurred: " + exception
            };

            var logList = FillCurrentLogsList();
            logList.Add(log);

            logMade = WriteGNGLogToFile(logList);

            return logMade;
        }

        /// <summary> 
        /// Author: Dylan
        /// Reads all json files in directory and deserializes into GNGlog and puts them all into a list
        /// </summary>
        /// <returns></returns>
        public List<GNGLog> ReadLogsPath(string path)
        {
            var logList = new List<GNGLog>();
            var di = new DirectoryInfo(path);
            var dirs = Directory.GetFiles(path, "*.json");
            foreach (var dir in dirs)
            {
                if (new FileInfo(dir).Length != 0)
                {
                    using (var r = new StreamReader(dir))
                    {
                        var jsonFile = r.ReadToEnd();
                        //Retrieve Current Logs
                        var logs = JsonConvert.DeserializeObject<List<GNGLog>>(jsonFile);
                        for (var index = 0; index < logs.Count; index++)
                        {
                            logList.Add(logs[index]);
                        }
                        r.Close();
                    }
                }
            }
            return logList;
        }

        /// <summary> 
        /// Author: Dylan
        /// Reads all json files in directory and deserializes into GNGlog and puts them all into a list
        /// </summary>
        /// <returns></returns>
        public List<GNGLog> ReadLogsGivenMonthYear(string month, int year)
        {
            var logList = new List<GNGLog>();
            var intMonth = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month;
            int daysOfMonth = System.DateTime.DaysInMonth(year, intMonth);
            var utcMonth = intMonth.ToString("00");
            var utcYear = year.ToString("0000");

            for (int i = 1; i <= daysOfMonth; i++)
            {
                var utcDay = i.ToString("00");
                var currentDate = utcYear + "_" + utcMonth + "_" + utcDay;
                var currentFile = configurations.GetLogDirectory() + currentDate + "_gnglog.json";
                if(File.Exists(currentFile))
                {
                    if (new FileInfo(currentFile).Length != 0)
                    {
                        using (var r = new StreamReader(currentFile))
                        {
                            var jsonFile = r.ReadToEnd();
                            //Retrieve Current Logs
                            var logs = JsonConvert.DeserializeObject<List<GNGLog>>(jsonFile);
                            for (var index = 0; index < logs.Count; index++)
                            {
                                logList.Add(logs[index]);
                            }
                            r.Close();
                        }
                    }
                }
            }
            return logList;
        }

        /// <summary>
        /// Method WriteGNGLogToFile writes the logs into the JSON log file. Should the
        /// write fail, the error counter is incremented and the exception is sent to
        /// the error handler service if it causes 100 or more errors to send to the
        /// system admin.
        /// </summary>
        /// <param name="logList">List of GNG logs to write to the file</param>
        /// <returns>Returns true or false based on if it was written to file properly</returns>
        public bool WriteGNGLogToFile(List<GNGLog> logList)
        {
            var isLogWritten = false;

            // If drive space is less than the minimum specified or is not available, return false
            if (configurations.GetDriveInfo().AvailableFreeSpace < configurations.GetMinBytes()
                || !configurations.GetDriveInfo().IsReady)
            {
                _errorHandlerService.IncrementErrorOccurrenceCount("Insufficient memory in drive to write to log");
                return isLogWritten;
            }

            try
            {
                using (var file = File.CreateText(currentLogPath))
                {
                    var jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    isLogWritten = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                isLogWritten = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
            return isLogWritten;
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
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "ErrorEncountered",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = errorCode + " encountered at " + urlOfErr + "\n" + errDesc
            };
            var logList = FillCurrentLogsList();
            logList.Add(log);

            logMade = WriteGNGLogToFile(logList);

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
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "BadRequest",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "Bad request at " + url + ": " + exception
            };

            var logList = FillCurrentLogsList();
            logList.Add(log);

            logMade = WriteGNGLogToFile(logList);

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
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "MaliciousAttacks",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "Malicious attack attempted at " + url
            };

            var logList = FillCurrentLogsList();
            logList.Add(log);

            logMade = WriteGNGLogToFile(logList);

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
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "SearchAction",
                UserID = usersID,
                IpAddress = ip,
                DateTime = DateTime.UtcNow.ToString(),
                Description = "User searched for " + searchedItem
            };

            var logList = FillCurrentLogsList();
            logList.Add(log);

            logMade = WriteGNGLogToFile(logList);

            return logMade;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool LogFailedLogin(string userID)
        {
            var fileName = DateTime.UtcNow.ToString(configurations.GetDateTimeFormat()) + configurations.GetLogExtention();
            CreateNewLog(fileName, configurations.GetLogDirectory());
            var logMade = false;
            var log = new GNGLog
            {
                LogID = "FailedLogin",
                UserID = "",
                IpAddress = "",
                DateTime = DateTime.UtcNow.ToString(),
                Description = userID +" Failed to Login"
            };

            var logList = FillCurrentLogsList();
            logList.Add(log);

            logMade = WriteGNGLogToFile(logList);

            return logMade;
        }
    }
}
