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
        /// <returns>returns an integer with the number of logs found</returns>
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
                if (string.Compare(parsedDate.ToString("MMMM"), month) != 0 || parsedDate.Year != year)
                {
                    logs.Remove(logs[i]);
                }
            }
            return logs;
        }

        /// <summary>
        /// Removes logs from list that does not have the specified ID
        /// </summary>
        /// <param name="logs">List of Logs</param>
        /// <param name="ID">Specified ID</param>
        public List<GNGLog> GetLogswithID(List<GNGLog> logs, string ID)
        {
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                if (string.Compare(logs[i].LogID, ID) != 0)
                {
                    logs.Remove(logs[i]);
                }
            }
            return logs;
        }

        /// <summary>
        /// Function the calcualtes the average session time over the number of sessions
        /// </summary>
        /// <param name="session">List of session times</param>
        /// <returns>the average session time</returns>
        public double CalculateAverageSessionTime(List<GNGLog> session)
        {
            var average = 0;
            var totalSessions = (session.Count) / 2;
            var totalTime = 0;
            //Iterate list as pairs and find time difference for each pair
            for(int i = 1; i < session.Count;)
            {
                DateTime end = DateTime.Parse(session[i - 1].DateTime);
                DateTime beginning = DateTime.Parse(session[i].DateTime);
                TimeSpan duration = end - beginning;
                //Convert time to minutes
                totalTime = totalTime + (int)duration.TotalMinutes;
                i = i + 2;
            }
            //Calculate the average time
            average = totalTime / totalSessions;
            return average;
        }

        /// <summary>
        /// Functions that removes logs without the specified string and keeps only entrence logs
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
        /// Functions that removes logs without the specified string and keeps only the exit logs
        /// </summary>
        /// <param name="logs">List of logs</param>
        /// <param name="url">specific url</param>
        public void GetExitLogswithURL(List<GNGLog> logs, string url)
        {
            //For everylog find the exit point to the website
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                string logID = logs[i].LogID;
                //Check if its not logID ClickEvent or ExitFromWebsite
                if(string.Compare(logID, "ClickEvent") != 0 && string.Compare(logID, "ExitFromWebsite") != 0)
                {
                        logs.Remove(logs[i]);
                }
                //Check to see if the url of the log doesnt match with the passed url
                if (string.Compare(logID, "ClickEvent") == 0)
                {
                    string[] lastPageUrl = logs[i].Description.Split(' ');
                    if (string.Compare(lastPageUrl[0], url) != 0)
                    {
                        logs.Remove(logs[i]);
                    }
                }
                //Check to see if the url when the user logs off doesnt matches the passed url
                if (string.Compare(logID, "ExitFromWebsite") == 0)
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
        /// Functions that removes logs without the specified month and keeps only the exit logs
        /// </summary>
        /// <param name="logs">List of logs</param>
        /// <param name="url">specific url</param>
        public bool IsMonthValid(string month)
        {
            var result = false;
            var months = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            if (string.IsNullOrEmpty(month))
            {
                return result;
            }
            for(int i = 0; i < months.Count; i++)
            {
                if(month.CompareTo(months[i]) == 0)
                {
                    result = true;
                }
            }
            return result;
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
    }
}
