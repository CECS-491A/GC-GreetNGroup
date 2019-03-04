using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace GreetNGroup.Logging
{
    public class GNGLogger
    {
        private const string LOGS_FOLDERPATH = "C:\\Users\\Yuki\\Documents\\GitHub\\GreetNGroup\\GreetNGroup\\GreetNGroup\\Logs\\";
        private static LogIDGenerator logIDGenerator = new LogIDGenerator();
        private Dictionary<string, int> listOfIDs = logIDGenerator.GetLogIDs();
        private string currentLogPath;
        public GNGLogger()
        {
            CreateNewLog();
        }

        public void CreateNewLog()
        {
            bool logExists = CheckForExistingLog();
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            if (logExists == false)
            {
                
                try
                {
                    var newLog = File.Create(LOGS_FOLDERPATH + currentDate + "_gnglog.json");
                    currentLogPath = LOGS_FOLDERPATH + currentDate + "_gnglog.json";
                    newLog.Close();
                }
                catch (IOException e)
                {
                    
                }
                
            }
            else
            {
                currentLogPath = LOGS_FOLDERPATH + currentDate + "_gnglog.json";
            }
        }

        public bool CheckForExistingLog()
        {
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            bool logExists = File.Exists(LOGS_FOLDERPATH + currentDate + "_gnglog.json");

            return logExists;
        }

        [HttpPost]
        public bool LogClicksMade(string startPoint, string endPoint, string usersID)
        {
            bool logMade = false;
            listOfIDs.TryGetValue("ClickEvent", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                dateTime = DateTime.Now.ToString(),
                description = startPoint + " to " + endPoint
            };

            string json = JsonConvert.SerializeObject(log, Formatting.Indented);
            try
            {
                using (StreamWriter file = File.AppendText(currentLogPath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, log);
                    logMade = true;
                    file.Close();
                }
            }catch(FileNotFoundException e)
            {
                logMade = false;
            }
            return logMade;

        }

        [HttpPost]
        public bool LogErrorsEncountered(string usersID, string errorCode, string urlOfErr)
        {
            bool logMade = false;
            listOfIDs.TryGetValue("ErrorEncountered", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                dateTime = DateTime.Now.ToString(),
                description = errorCode + " encountered at " + urlOfErr
            };

            string json = JsonConvert.SerializeObject(log, Formatting.Indented);
            try
            {
                using (StreamWriter file = File.AppendText(currentLogPath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, log);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
            }
            return logMade;
        }

        [HttpPost]
        public bool LogGNGEventsCreated(string usersID, string eventID)
        {
            bool logMade = false;
            listOfIDs.TryGetValue("EventCreated", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                dateTime = DateTime.Now.ToString(),
                description = "Event " + eventID + " created"
            };

            string json = JsonConvert.SerializeObject(log, Formatting.Indented);
            try
            {
                using (StreamWriter file = File.AppendText(currentLogPath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, log);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
            }
            return logMade;
        }

        [HttpPost]
        public bool LogEntryToWebsite(string usersID, string urlEntered)
        {
            bool logMade = false;
            listOfIDs.TryGetValue("EntryToWebsite", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                dateTime = DateTime.Now.ToString(),
                description = usersID + " entered at " + urlEntered
            };

            string json = JsonConvert.SerializeObject(log, Formatting.Indented);
            try
            {
                using (StreamWriter file = File.AppendText(currentLogPath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, log);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
            }
            return logMade;
        }

        [HttpPost]
        public bool LogExitFromWebsite(string usersID, string urlOfExit)
        {
            bool logMade = false;
            listOfIDs.TryGetValue("ExitFromWebsite", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                dateTime = DateTime.Now.ToString(),
                description = usersID + " exited website from " + urlOfExit
            };

            string json = JsonConvert.SerializeObject(log, Formatting.Indented);
            try
            {
                using (StreamWriter file = File.AppendText(currentLogPath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, log);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
            }
            return logMade;
        }

        [HttpPost]
        public bool LogAccountDeletion(string usersID)
        {
            bool logMade = false;
            listOfIDs.TryGetValue("ErrorEncountered", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                dateTime = DateTime.Now.ToString(),
                description = usersID + " deleted account"
            };

            string json = JsonConvert.SerializeObject(log, Formatting.Indented);
            try
            {
                using (StreamWriter file = File.AppendText(currentLogPath))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(file, log);
                    logMade = true;
                    file.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                logMade = false;
            }
            return logMade;
        }

    }
}