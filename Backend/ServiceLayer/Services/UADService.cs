using Gucci.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using Gucci.ServiceLayer.Interface;

namespace Gucci.ServiceLayer.Services
{
    public class UADService : IUADService
    {
        /// <summary>
        /// Function that returns the number of logs with a specific logid
        /// </summary>
        /// <param name="logs">list of logs</param>
        /// <param name="logID">log id that wants to be counted</param>
        /// <returns>returns the number of logs with the passed logid found</returns>
        public int GetNumberofLogsID(List<GNGLog> logs, string logID)
        {
            int logcount = 0;
            for (int index = 0; index < logs.Count; index++)
            {
                if (logs[index].LogID.Equals(logID) == true)
                {
                    logcount++;
                }
            }
            return logcount;
        }
        /// <summary>
        /// Function that returns the login information(average logins, max, min) for the current year and month
        /// </summary>
        /// <param name="logs">list of logs</param>
        /// <returns>returns the number of logs with the passed logid found</returns>
        public List<string> GetLoginInfo(List<GNGLog> logs)
        {
            var loginID = "EntryToWebsite";
            var totalLogin = 0;
            var currentLogin = 0;
            int minLogin = 0;
            int maxLogin = 0;
            var totalDays = 1.0;
            var firstDate = DateTime.Parse(logs[0].DateTime);
            var firstDay = firstDate.Day;
            var loginInfoList = new List<string>();
            var averageLogins = 0.0;
            for (int i = 0; i < logs.Count; i++)
            {
                var currentDate = DateTime.Parse(logs[i].DateTime);
                var currentDay = currentDate.Day;
                if (logs[i].LogID.Equals(loginID) == true)
                {
                    totalLogin++;
                    currentLogin++;
                }
                // Sets max and min for first day
                if (totalDays == 1.0)
                {
                    minLogin = currentLogin;
                    maxLogin = currentLogin;
                }
                // Checks to see if days have changed
                if (firstDay != currentDay)
                {
                    firstDay = currentDay;
                    totalDays++;
                    maxLogin = Math.Max(currentLogin, maxLogin);
                    minLogin = Math.Min(currentLogin, minLogin);
                    currentLogin = 0;
                }
            }
            // Checks last day of logs
            maxLogin = Math.Max(currentLogin, maxLogin);
            minLogin = Math.Min(currentLogin, minLogin);
            averageLogins = totalLogin / totalDays;
            loginInfoList.Add(averageLogins.ToString("0.##"));
            loginInfoList.Add(minLogin.ToString("0.##"));
            loginInfoList.Add(maxLogin.ToString("0.##"));
            return loginInfoList;
        }
        /// <summary>
        /// Function that takes a list of logs and removes the logs not related to the specific month
        /// </summary>
        /// <param name="logs">List of logs</param>
        /// <param name="month">specified month</param>
        /// <returns>returns logs with the specified month</returns>
        public List<GNGLog> GetLogsFortheMonth(List<GNGLog> logs, string month)
        {
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                //Return the Name of the month
                DateTime parsedDate = DateTime.Parse(logs[i].DateTime);
                if (string.Compare(parsedDate.ToString("MMMM"), month) != 0)
                {
                    logs.Remove(logs[i]);
                }
            }
            return logs;
        }

        /// <summary>
        /// Function that takes a list of logs and removes the logs not related to the specific month and year
        /// </summary>
        /// <param name="logs">List of logs</param>
        /// <param name="month">specified month</param>
        /// <param name="year">specified year</param>
        /// <returns>returns logs with the specified month and year</returns>
        public List<GNGLog> GetLogsForMonthAndYear(List<GNGLog> logs, string month, int year)
        {
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                //Return the Name of the month
                DateTime parsedDate = DateTime.Parse(logs[i].DateTime);
                if (month.CompareTo(parsedDate.ToString("MMMM")) != 0 || parsedDate.Year != year)
                {
                    logs.Remove(logs[i]);
                }
            }
            return logs;
        }

        /// <summary>
        /// Gets logs from list that have the specified ID
        /// </summary>
        /// <param name="logs">List of Logs</param>
        /// <param name="ID">Specified ID</param>
        public List<GNGLog> GetLogswithID(List<GNGLog> logs, string ID)
        {
            var loglist = new List<GNGLog>();
            for (int i = 0;  i < logs.Count; i++)
            {
                if (string.Compare(logs[i].LogID, ID) == 0)
                {
                    loglist.Add(logs[i]);
                }
            }
            return loglist;
        }

        /// <summary>
        /// Function the calcualtes the average session time over the number of sessions
        /// </summary>
        /// <param name="session">List of session times</param>
        /// <returns>the average session time</returns>
        public List<string> CalculateAverageSessionInformation(List<GNGLog> session)
        {
            var average = 0.0;
            var totalSessions = session.Count / 2.0;
            var totalTime = 0;
            var maxSession = 0;
            var minSession = 0;
            var sessionInformation = new List<string>();
            //Iterate list as pairs and find time difference for each pair
            for(int i = 1; i < session.Count;)
            {
                
                var end = DateTime.Parse(session[i - 1].DateTime);
                var beginning = DateTime.Parse(session[i].DateTime);
                var duration = end - beginning;
                //Convert time to minutes
                totalTime = totalTime + (int)duration.TotalMinutes;
                if (i == 1)
                {
                    maxSession = (int)duration.TotalMinutes;
                    minSession = (int)duration.TotalMinutes;
                }
                maxSession = Math.Max((int)duration.TotalMinutes, maxSession);
                minSession = Math.Min((int)duration.TotalMinutes, minSession);
                i = i + 2;
            }
            //Calculate the average time
            average = totalTime / totalSessions;
            sessionInformation.Add(average.ToString("0.##"));
            sessionInformation.Add(minSession.ToString("0.##"));
            sessionInformation.Add(maxSession.ToString("0.##"));
            return sessionInformation;
        }

        /// <summary>
        /// Functions that removes logs without the specified url and keeps only entrence logs
        /// </summary>
        /// <param name="logs">list of logs</param>
        /// <param name="url">specific url</param>
        public void GetEntryLogswithURL(List<GNGLog> logs, string url)
        {
            logs = GetLogswithID(logs, "ClickEvent");
            //For every log check to see if the urls dont match
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                string[] words = logs[i].Description.Split(' ');
                if (string.Compare(words[2], url) != 0)
                {
                    logs.Remove(logs[i]);
                }
            }
        }

        /// <summary>
        /// Functions that removes logs without the specified url and keeps only the exit logs
        /// </summary>
        /// <param name="logs">List of logs</param>
        /// <param name="url">specific url</param>
        public void GetExitLogswithURL(List<GNGLog> logs, string url)
        {
            //For everylog find the exit point to the website
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                string logID = logs[i].LogID;
                //Check if log is does not have logID ClickEvent or ExitFromWebsite
                if(string.Compare(logID, "ClickEvent") != 0 && string.Compare(logID, "ExitFromWebsite") != 0)
                {
                        logs.Remove(logs[i]);
                }
                //Check to see if the url of the log doesnt match with the passed url
                else if (string.Compare(logID, "ClickEvent") == 0)
                {
                    string[] lastPageUrl = logs[i].Description.Split(' ');
                    if (string.Compare(lastPageUrl[0], url) != 0)
                    {
                        logs.Remove(logs[i]);
                    }
                }
                //Check to see if the url when the user logs off doesnt matches the passed url
                else if (string.Compare(logID, "ExitFromWebsite") == 0)
                {
                    string[] logoutUrl = logs[i].Description.Split(' ');
                    if (string.Compare(logoutUrl[4], url) != 0)
                    {
                        logs.Remove(logs[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Function that Pairs starting and ending logs together to create sessions
        /// </summary>
        /// <param name="startLogs">List of Logs for when sessions begin</param>
        /// <param name="endLogs">List of Logs whenever sessions end</param>
        /// <returns>Paired List of Logs</returns>
        public List<GNGLog> PairStartAndEndLogs(List<GNGLog> startLogs, List<GNGLog> endLogs)
        {
            var sessions = new List<GNGLog>();
            for (int k = 0; k < endLogs.Count; k++)
            {
                var notFound = true;
                var pos = 0;
                while (notFound == true)
                {
                    if(startLogs.Count != 0)
                    {
                        if (startLogs[pos].UserID == endLogs[k].UserID) // Assumes logs are directly linked to one another
                        {
                            sessions.Add(endLogs[k]);
                            sessions.Add(startLogs[pos]);
                            startLogs.Remove(startLogs[pos]);
                            notFound = false;
                        }
                        else
                        {
                            pos++;
                        }
                        if (pos == startLogs.Count)
                        {
                            notFound = false;
                        }
                    }
                    else
                    {
                        notFound = false;
                    }
                }
            }
            return sessions;
        }
        /// <summary>
        /// Function that converts arrays of strings and list into UAD objects
        /// </summary>
        /// <param name="informationTitles">Array of titles for information</param>
        /// <param name="values">List of values that go with the titles</param>
        /// <returns>List of UADObjects</returns>
        public List<UADObject> ConvertListToUADObjects(string[] informationTitles, List<string> values)
        {
            var uadObjects = new List<UADObject>();
            for(int i = 0; i < informationTitles.Length; i++)
            {
                var currentUADObject = new UADObject
                {
                    InfoType = informationTitles[i],
                    Value = values[i]
                };
                uadObjects.Add(currentUADObject);
            }
            return uadObjects;
        }
        
    }
}
