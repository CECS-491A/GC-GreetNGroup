using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using DataAccessLayer.Models;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Model;

namespace Gucci.ServiceLayer.Services
{
    public class LoggerService : ILoggerService
    {

        /* Every logging method will catch FileNotFoundException explicitly
         * so we know that there is a problem when attempting to log. Let
         * other exceptions bubble up
         */

        private IErrorHandlerService _errorHandlerService;
        private Dictionary<string, int> listOfIDs;
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

        /// <summary>
        /// Author: Jonalyn
        /// Method GetLogIDs creates a Dictionary of int values representing the specific
        /// event that is being logged and string keys associated with that ID
        /// </summary>
        /// <returns>Dictionary<string, int> which holds the mapped ids</returns>
        public Dictionary<string, int> GetLogIDs()
        {
            listOfIDs = new Dictionary<string, int>();

            var logIDMap = new Dictionary<string, int>();
            logIDMap.Add("ClickEvent", 1001);
            logIDMap.Add("ErrorEncountered", 1002);
            logIDMap.Add("EventCreated", 1003);
            logIDMap.Add("EntryToWebsite", 1004);
            logIDMap.Add("ExitFromWebsite", 1005);
            logIDMap.Add("AccountDeletion", 1006);
            logIDMap.Add("InternalErrors", 1007);
            logIDMap.Add("MaliciousAttacks", 1008);
            logIDMap.Add("EventUpdated", 1009);
            logIDMap.Add("SearchAction", 1010);
            logIDMap.Add("FindEventForMe", 1011);
            logIDMap.Add("UserRatings", 1012);
            logIDMap.Add("EventJoined", 1013);
            logIDMap.Add("BadRequest", 1014);
            logIDMap.Add("EventDeleted", 1015);
            logIDMap.Add("EventExpired", 1016);

            return logIDMap;
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
            listOfIDs.TryGetValue("InternalErrors", out int clickLogID);
            var clickLogIDString = clickLogID.ToString();
            var log = new GNGLog
            {
                //Don't care about user id or ip address for internal error log
                LogID = clickLogIDString,
                UserID = "",
                IpAddress = "",
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

    }
}
