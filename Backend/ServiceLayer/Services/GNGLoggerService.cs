using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using DataAccessLayer.Models;
using ServiceLayer.Interface;

namespace ServiceLayer.Services
{
    public class GNGLoggerService : IGNGLoggerService
    {
        private IErrorHandlerService _errorHandlerService;

        private readonly string LOGS_FOLDERPATH = "C:\\Users\\Yuki\\Documents\\GitHub\\GreetNGroup\\Backend\\Logs\\";
        private readonly string LOG_IDENTIFIER = "_gnglog.json";
        private Dictionary<string, int> listOfIDs;
        private string currentLogPath;

        public GNGLoggerService()
        {
            _errorHandlerService = new ErrorHandlerService();
        }

        /// <summary>
        /// Author: Jonalyn
        /// Method CheckForExistingLog checks if a log for today already exists. If
        /// a log already exists for the current date, it will set the existing log as
        /// the current log
        /// </summary>
        /// <returns>Returns true or false if log exists or not</returns>
        public bool CheckForExistingLog()
        {
            bool logExists = false;
            try
            {
                string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                logExists = File.Exists(LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER);
            }
            //Catch FileNotFound explicitly as it catches errors where it cannot find the log
            //Let other exceptions bubble up
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
        public string CreateNewLog()
        {
            bool logExists = CheckForExistingLog();
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            if (logExists == false)
            {
                try
                {
                    using (var fileStream = new FileStream(LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER,
                    FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        currentLogPath = LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER;
                        fileStream.Close();
                    }
                }
                //Catch IOException esxplicitly when it fails to create a log
                //Let other errors bubble up
                catch (IOException e)
                {
                    _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
                }

            }
            else
            {
                currentLogPath = LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER;
            }

            return currentLogPath;
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

            Dictionary<string, int> logIDMap = new Dictionary<string, int>();
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

        public string GetLogsExtentionName()
        {
            return LOG_IDENTIFIER;
        }

        public string GetLogsFolderpath()
        {
            return LOGS_FOLDERPATH;
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
            List<GNGLog> logList = new List<GNGLog> ();
            //Check to see if file is empty
            if (new FileInfo(currentLogPath).Length != 0)
            {
                using (StreamReader r = new StreamReader(currentLogPath))
                {
                    string jsonFile = r.ReadToEnd();
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
            List<GNGLog> logList = new List<GNGLog>();
            DirectoryInfo di = new DirectoryInfo(LOGS_FOLDERPATH);
            string[] dirs = Directory.GetFiles(LOGS_FOLDERPATH, "*.json");
            foreach (string dir in dirs)
            {
                if (new FileInfo(dir).Length != 0)
                {
                    using (StreamReader r = new StreamReader(dir))
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
            CreateNewLog();
            bool logMade = false;
            listOfIDs.TryGetValue("InternalErrors", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = "",
                ipAddress = "",
                dateTime = DateTime.Now.ToString(),
                description = "Internal errors occurred: " + exception
            };

            var logList = FillCurrentLogsList();
            logList.Add(log);
            try
            {
                using (StreamWriter file = File.CreateText(currentLogPath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, logList);
                    logMade = true;
                    file.Close();
                }
            }
            catch (Exception e) //Catch any exceptions caused resulting of failure in writing log
            {
                logMade = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }
            return logMade;
        }

        /// <summary> 
        /// Author: Dylan
        /// Reads all json files in directory and deserializes into GNGlog and puts them all into a list
        /// </summary>
        /// <returns></returns>
        public List<GNGLog> ReadLogsPath(string path)
        {
            List<GNGLog> logList = new List<GNGLog>();
            DirectoryInfo di = new DirectoryInfo(path);
            string[] dirs = Directory.GetFiles(path, "*.json");
            foreach (string dir in dirs)
            {
                if (new FileInfo(dir).Length != 0)
                {
                    using (StreamReader r = new StreamReader(dir))
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
    }
}
