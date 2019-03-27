using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace GreetNGroup.Logging
{
    public class GNGLogger
    {
        private static string LOGS_FOLDERPATH = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, 
            @"GreetNGroup\GreetNGroup\Logs\");
        private static string LOG_IDENTIFIER = "_gnglog.json";
        private static Dictionary<string, int> listOfIDs = LogIDGenerator.GetLogIDs();
        private static string currentLogPath;
        private static int errorCounter = 0;
        private static List<GNGLog> logList = new List<GNGLog>();

        /// <summary>
        /// Method CreateNewLog creates a new log if a log does 
        /// not exist for the current date
        /// </summary>
        private static void CreateNewLog()
        {
            bool logExists = CheckForExistingLog();
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            if (logExists == false)
            {
                
                try
                {
                    var newLog = File.Create(LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER);
                    currentLogPath = LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER;
                    List<GNGLog> listOfLogsInit = new List<GNGLog>();
                    using(StreamWriter writer = File.AppendText(currentLogPath))
                    {
                        JsonSerializer jsonSerializer = new JsonSerializer();
                        jsonSerializer.Serialize(writer, listOfLogsInit);
                        writer.Close();
                    }
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
        /// <summary>
        /// Read the current logs for the day
        /// </summary>
        private static void ReadLogs()
        {
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
        }
        /// <summary>
        /// Method CheckForExistingLog checks if a log for today already exists. If
        /// a log already exists for the current date, it will set the existing log as
        /// the current log
        /// </summary>
        /// <returns>Returns true or false if log exists or not</returns>
        private static bool CheckForExistingLog()
        {
            bool logExists = false;
            try
            {
                string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                logExists = File.Exists(LOGS_FOLDERPATH + currentDate + LOG_IDENTIFIER);
            }
            catch(FileNotFoundException e)
            {
                errorCounter++;
            }

            return logExists;
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
        [HttpPost]
        public static bool LogClicksMade(string startPoint, string endPoint, string usersID, string ip)
        {
            CreateNewLog();
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

            ReadLogs();
            //Add new log to list
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
            }catch(FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
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
        [HttpPost]
        public static bool LogErrorsEncountered(string usersID, string errorCode, string urlOfErr, string errDesc, string ip)
        {
            CreateNewLog();
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

            ReadLogs();

            //Add new log to list
            logList.Add(log);

            string json = JsonConvert.SerializeObject(log, Formatting.Indented);
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
            catch (FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
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
        [HttpPost]
        public static bool LogGNGEventsCreated(string usersID, string eventID, string ip)
        {
            CreateNewLog();
            List<GNGLog> logList = new List<GNGLog>();
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

            ReadLogs();
            //Add new log to list
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
            catch (FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
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
        [HttpPost]
        public static bool LogEntryToWebsite(string usersID, string urlEntered, string ip)
        {
            CreateNewLog();
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

            ReadLogs();
            //Add new log to list
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
            catch (FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
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
        [HttpPost]
        public static bool LogExitFromWebsite(string usersID, string urlOfExit, string ip)
        {
            CreateNewLog();
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

            ReadLogs();
            //Add new log to list
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
            catch (FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
            }
            return logMade;
        }

        /// <summary>
        /// Method LogAccountDeletion logs when a user deletes their GreetNGroup account.
        /// </summary>
        /// <param name="usersID">Hashed user ID</param>
        /// <param name="ip">IP address</param>
        /// <returns>Returns true or false if log was successfully made</returns>
        [HttpPost]
        public static bool LogAccountDeletion(string usersID, string ip)
        {
            CreateNewLog();
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

            ReadLogs();
            //Add new log to list
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
            catch (FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
            }
            return logMade;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usersID"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpPost]
        public static bool LogGNGSessionStart(string usersID, string ip)
        {
            CreateNewLog();
            bool logMade = false;
            listOfIDs.TryGetValue("SessionStarted", out int clickLogID);
            string clickLogIDString = clickLogID.ToString();
            GNGLog log = new GNGLog
            {
                logID = clickLogIDString,
                userID = usersID,
                ipAddress = ip,
                dateTime = DateTime.Now.ToString(),
                description = "Session Started"
            };

            ReadLogs();
            //Add new log to list
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
            catch (FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
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
        [HttpPost]
        public static bool LogGNGSearchForUser(string usersID, string searchedUser, string ip)
        {
            CreateNewLog();
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

            ReadLogs();
            //Add new log to list
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
            catch (FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
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
        [HttpPost]
        public static bool LogGNGJoinEvent(string usersID, string eventID, string ip)
        {
            CreateNewLog();
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

            ReadLogs();
            //Add new log to list
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
            catch (FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
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
        [HttpPost]
        public static bool LogGNGUserRating(string usersID, string ratedUserID, string ip)
        {
            CreateNewLog();
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

            ReadLogs();
            //Add new log to list
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
            catch (FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
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
        [HttpPost]
        public static bool LogGNGFindEventForMe(string usersID, string ip)
        {
            CreateNewLog();
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

            ReadLogs();
            //Add new log to list
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
            catch (FileNotFoundException e)
            {
                logMade = false;
                errorCounter++;
            }
            return logMade;
        }

        /// <summary>
        /// Method errorHandler checks if the error counter has reached 100 or more. If
        /// it does, then the error handler will call the function to contact the system admin.
        /// </summary>
        public static void errorHandler()
        {
            if(errorCounter >= 100)
            {
                //Contact system admin
                errorCounter = 0;
            }
        }

    }
}