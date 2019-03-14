using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace GreetNGroup.Logging
{
    public class GNGLogger
    {
        private string LOGS_FOLDERPATH = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, 
            @"GreetNGroup\GreetNGroup\Logs\");
        private const string LOG_IDENTIFIER = "_gnglog.json";
        private static LogIDGenerator logIDGenerator = new LogIDGenerator();
        private Dictionary<string, int> listOfIDs = logIDGenerator.GetLogIDs();
        private string currentLogPath;
        private int errorCounter = 0;

        public GNGLogger()
        {
            CreateNewLog();
        }

        private void CreateNewLog()
        {
            bool logExists = CheckForExistingLog();
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            if (logExists == false)
            {
                
                try
                {
                    var newLog = File.Create(LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER);
                    currentLogPath = LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER;
                    newLog.Close();
                }
                catch (IOException e)
                {
                    errorCounter++;   
                }
                
            }
            else
            {
                currentLogPath = LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER;
            }
        }

        private bool CheckForExistingLog()
        {
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            bool logExists = File.Exists(LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER);

            return logExists;
        }

        [HttpPost]
        public bool LogClicksMade(string startPoint, string endPoint, string usersID, string ip)
        {
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
                errorCounter++;
            }
            return logMade;

        }

        [HttpPost]
        public bool LogErrorsEncountered(string usersID, string errorCode, string urlOfErr, string errDesc, string ip)
        {
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
                errorCounter++;
            }
            return logMade;
        }

        [HttpPost]
        public bool LogGNGEventsCreated(string usersID, string eventID, string ip)
        {
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
                errorCounter++;
            }
            return logMade;
        }

        [HttpPost]
        public bool LogEntryToWebsite(string usersID, string urlEntered, string ip)
        {
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
                errorCounter++;
            }
            return logMade;
        }

        [HttpPost]
        public bool LogExitFromWebsite(string usersID, string urlOfExit, string ip)
        {
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
                errorCounter++;
            }
            return logMade;
        }

        [HttpPost]
        public bool LogAccountDeletion(string usersID, string ip)
        {
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
                errorCounter++;
            }
            return logMade;
        }

    }
}