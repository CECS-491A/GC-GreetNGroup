using Gucci.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using Gucci.ServiceLayer.Interface;

namespace Gucci.ServiceLayer.Services
{
    public class UADService : IUADService, ISortService
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
        /// Quicksort function that orders most used function on website
        /// </summary>
        /// <param name="usedCounts">List of number of uses</param>
        /// <param name="logID">List of function ids</param>
        /// <param name="left">Left position</param>
        /// <param name="right">Right position</param>
        public void QuickSortInteger(List<int> usedCounts, List<string> logID, int left, int right)
        {
            if (left < right)
            {
                int pivot = PartitionInteger(usedCounts, logID, left, right);

                if (pivot > 1)
                {
                    QuickSortInteger(usedCounts, logID, left, pivot - 1);
                }
                if (pivot + 1 < right)
                {
                    QuickSortInteger(usedCounts, logID, pivot + 1, right);
                }
            }
        }

        /// <summary>
        /// Functions that partitions the list for quicksort and orders the list correctly
        /// </summary>
        /// <param name="usedCounts">List of integers representing the number of times a function was used</param>
        /// <param name="logID">List of log ids</param>
        /// <param name="left">Left index</param>
        /// <param name="right">right index</param>
        /// <returns>Next index to be used</returns>
        public int PartitionInteger(List<int> usedCounts, List<string> logID, int left, int right)
        {
            int pivot = usedCounts[left];
            while (true)
            {
                while (usedCounts[left] < pivot)
                {
                    left++;
                }
                while (usedCounts[right] > pivot)
                {
                    right--;
                }
                if (left < right)
                {
                    int temp = usedCounts[left];
                    usedCounts[left] = usedCounts[right];
                    usedCounts[right] = temp;

                    string tempID = logID[left];
                    logID[left] = logID[right];
                    logID[right] = tempID;

                    if (usedCounts[left] == usedCounts[right])
                    {
                        left++;
                    }
                }
                else
                {
                    return right;
                }
            }
        }

        /// <summary>
        /// Quicksort functions that sorts the average sessions times per page
        /// </summary>
        /// <param name="sessiontimes">array of session times</param>
        /// <param name="urls">list of urls</param>
        public void QuickSortDouble<T>(List<T> sessiontimes, List<string> urls) where T : IComparable<T>
        {
            PartitionDouble(sessiontimes, urls, 0, sessiontimes.Count - 1);
        }

        /// <summary>
        /// Function that partitions the list and orders it correctly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessiontimes">array of session times</param>
        /// <param name="urls">list of urls</param>
        /// <param name="left">left index of array</param>
        /// <param name="right">right index of array</param>
        public void PartitionDouble<T>(List<T> sessiontimes, List<string> urls, int left, int right) where T : IComparable<T>
        {
            int i, j;
            T pivot, temp;
            i = left;
            j = right;
            pivot = sessiontimes[(left + right) / 2];
            do
            {
                while ((sessiontimes[i].CompareTo(pivot) < 0) && (i < right))
                {
                    i++;
                }
                while ((pivot.CompareTo(sessiontimes[j]) < 0) && (j > left))
                {
                    j--;
                }
                if (i <= j)
                {
                    temp = sessiontimes[i];
                    string tempUrl = urls[i];
                    sessiontimes[i] = sessiontimes[j];
                    urls[i] = urls[j];
                    sessiontimes[j] = temp;
                    urls[j] = tempUrl;
                    i++;
                    j--;
                }
            } while (i <= j);

            if (left < j)
            {
                PartitionDouble(sessiontimes, urls, left, j);
            }
            if (i < right)
            {
                PartitionDouble(sessiontimes, urls, i, right);
            }
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
                Console.WriteLine(duration);
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
            logs = GetLogswithID(logs, "1001");
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
                //Check if its not logID 1001 or 1005
                if(string.Compare(logID, "1001") != 0 && string.Compare(logID, "1005") != 0)
                {
                        logs.Remove(logs[i]);
                }
                //Check to see if the url of the log doesnt match with the passed url
                if (string.Compare(logID, "1001") == 0)
                {
                    string[] lastPageUrl = logs[i].Description.Split(' ');
                    if (string.Compare(lastPageUrl[0], url) != 0)
                    {
                        logs.Remove(logs[i]);
                    }
                }
                //Check to see if the url when the user logs off doesnt matches the passed url
                if (string.Compare(logID, "1005") == 0)
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
            if(startLogs.Count > endLogs.Count)
            {
                for (int k = 0; k < endLogs.Count; k++)
                {
                    var notFound = true;
                    var pos = 0;
                    while (notFound == true)
                    {
                        if (startLogs[pos].UserID == endLogs[k].UserID)
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
                }
            }
            else
            {
                for (int k = 0; k < startLogs.Count; k++)
                {
                    var notFound = true;
                    var pos = 0;
                    while (notFound == true)
                    {
                        if (endLogs[pos].UserID == startLogs[k].UserID)
                        {
                            sessions.Add(endLogs[pos]);
                            sessions.Add(startLogs[k]);
                            startLogs.Remove(endLogs[pos]);
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
                }
            }

            return sessions;
        }
        /// <summary>
        /// Function that Formats the top 5 features into a string
        /// </summary>
        /// <param name="features">List of features</param>
        /// <param name="useCount">List of features use count</param>
        /// <returns>Proper string of top features</returns>
        public string FormatTop5Features(List<string> features, List<int> useCount)
        {
            var result = "";
            if(features.Count >= 5)
            {
                for(int i = features.Count - 1; i >= features.Count - 5; i--)
                {
                    result = result + " " + features[i] + " times used: " + useCount[i] + "\n";
                }
            }
            else
            {
                for (int i = features.Count - 1; i >= 0; i--)
                {
                    result = result + " " + features[i] + " times used: " + useCount[i] + "\n";
                }
            }
            return result;
        }

        /// <summary>
        /// Function that Formats the top 5 pages into a string
        /// </summary>
        /// <param name="features">List of features</param>
        /// <param name="timeSpent">List of time spent on page</param>
        /// <returns>Proper string of top pages</returns>
        public string FormatTop5Pages(List<string> features, List<double> timeSpent)
        {
            var result = "";
            if (features.Count >= 5)
            {
                for (int i = features.Count - 1; i >= features.Count - 5; i--)
                {
                    result = result + " " + features[i] + " average time spent: " + timeSpent[i] + "\n";
                }
            }
            else
            {
                for (int i = features.Count - 1; i >= 0; i--)
                {
                    result = result + " " + features[i] + " average time spent: " + timeSpent[i] + "\n";
                }
            }
            return result;
        }
    }
}
