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

        private readonly string LOGS_FOLDERPATH = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\Logs";
        private readonly string LOG_IDENTIFIER = "_gnglog.json";
        private Dictionary<string, int> listOfIDs;
        private string currentLogPath;

        /// <summary>
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
            catch (FileNotFoundException e)
            {
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }

            return logExists;
        }

        /// <summary>
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
                    var newLog = File.Create(LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER);
                    currentLogPath = LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER;
                }
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
            logIDMap.Add("SearchForUser", 1010);
            logIDMap.Add("FindEventForMe", 1011);
            logIDMap.Add("UserRatings", 1012);
            logIDMap.Add("EventJoined", 1013);
            logIDMap.Add("ViewHistory", 1014);
            logIDMap.Add("EventDeleted", 1015);
            logIDMap.Add("EventExpired", 1016);

            return logIDMap;
        }

        /// <summary>
        /// Method GetLogsExtentionName returns the extention of the logs specific to
        /// GreetNGroup
        /// </summary>
        /// <returns>Returns extention of logs as a string</returns>
        public string GetLogsExtentionName()
        {
            return LOG_IDENTIFIER;
        }

        /// <summary>
        /// Method GetLogsFolderpath returns the path of the folder housing GreetNGroup
        /// logs
        /// </summary>
        /// <returns>Full path of the logs as a string</returns>
        public string GetLogsFolderpath()
        {
            return LOGS_FOLDERPATH;
        }

        /// <summary>
        /// Method GetCurrentLogPath returns the path of the current day's log
        /// </summary>
        /// <returns>Full path of the current log as a string</returns>
        public string GetCurrentLogPath()
        {
            return currentLogPath;
        }

        /// <summary>
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
                }
            }
            return logList;
        }

        /// <summary>
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
                    }
                }
            }
            return logList;
        }
    }
}
