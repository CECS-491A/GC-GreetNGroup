using DataAccessLayer.Models;
using Gucci.ServiceLayer.Services;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;

namespace ManagerLayer.UADManagement
{
    public class UADManager
    {
        private static List<GNGLog> loglist = new List<GNGLog>();
        LoggerService _gngLoggerService;
        UADService _uadService;
        UserService _userService;
        private static List<string> months = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public UADManager()
        {
            _gngLoggerService = new LoggerService();
            _uadService = new UADService();
            _userService = new UserService();
            loglist = _gngLoggerService.ReadLogs();
        }

        /// <summary>
        /// Function that returns the number of registered accounts compared to the number of logins for a month
        /// </summary>
        /// <param name="month">specific month</param>
        /// <returns>the number of logins vs registered users for a month</returns>
        public string GetLoginComparedToRegistered(string month, int year)
        {
            string results = "";
            var loginID = "1004";
            loglist = _uadService.GetLogsForMonthAndYear(loglist, month, year);
            var registered = _userService.GetRegisteredUserCount();
            var loginCount = _uadService.GetNumberofLogsID(loglist, loginID);
            results = "Logins: " + loginCount +
                                    "\nRegistered Accounts: " + registered;
            return results;
        }

        /// <summary>
        /// Function that returns the average session duration for a month
        /// </summary>
        /// <returns>the average session duration for a month</returns>
        public string GetAverageSessionDuration(string month, int year)
        {
            string[] logID = { "1004", "1005" };
            var entryToWebsite = new List<GNGLog>(loglist);
            var exitFromWebsite = new List<GNGLog>(loglist);
            var sessions = new List<GNGLog>();
            string results = "";

            // Get two seperate logs 
            entryToWebsite = _uadService.GetLogsForMonthAndYear(entryToWebsite, month, year);
            exitFromWebsite = _uadService.GetLogsForMonthAndYear(exitFromWebsite, month, year);
            // Get logs for when they leave the website and for when they enter it
            entryToWebsite = _uadService.GetLogswithID(entryToWebsite, logID[0]);
            exitFromWebsite = _uadService.GetLogswithID(exitFromWebsite, logID[1]);

           
            // Pair sessions beginning and ending logs
            sessions = _uadService.PairStartAndEndLogs(entryToWebsite, exitFromWebsite);
            if (sessions.Count == 0)
            {
                results = "Average Session Time for " + month + ": " + 0;
            }
            else
            {
                var average = _uadService.CalculateAverageSessionTime(sessions);
                string sAverage = String.Format("{0:0.00}", average);
                results = "Average Session Time for " + month + ": " + sAverage;
            }
                
            
            
            return results;
        }

        /// <summary>
        /// Function that returns the top 5 page used for a month
        /// </summary>
        /// <param name="month">specific month</param>
        /// <returns>top 5 page used for a month</returns>
        public string GetTop5AveragePageSession(string month, int year)
        {
            var urlPages = new List<string>() { "https://www.greetngroup.com/create", "https://www.greetngroup.com/join", "https://www.greetngroup.com/searchUser", "https://www.greetngroup.com/searchEvent", "https://www.greetngroup.com/help", "https://www.greetngroup.com/faq" };
            var averageTimeSpent = new List<double>() ;
            string results = "";
            var monthValid = _uadService.IsMonthValid(month);

            // For every URL in the list get the average time spent on it
            for (int i = 0; i < urlPages.Count; i++)
            {
                var entryToPage = new List<GNGLog>(loglist);
                var exitFromPage = new List<GNGLog>(loglist);
                var sessions = new List<GNGLog>();

                // Get logs where user enters a url
                entryToPage = _uadService.GetLogsForMonthAndYear(entryToPage, month, year);
                _uadService.GetEntryLogswithURL(entryToPage, urlPages[i]);

                // Get Logs when user leaves a url
                exitFromPage = _uadService.GetLogsForMonthAndYear(exitFromPage, month, year);
                _uadService.GetExitLogswithURL(exitFromPage, urlPages[i]);

                // Pair the exit from pages with the entrance to a page
                sessions = _uadService.PairStartAndEndLogs(entryToPage, exitFromPage);

                if (sessions.Count == 0)
                {
                    averageTimeSpent.Add(0);
                }
                else
                {
                    // Get average time spent on the page
                    var average = _uadService.CalculateAverageSessionTime(sessions);
                    averageTimeSpent.Add(average);
                }    
            }
            //Sort Pages view time by shortest to longest
            _uadService.QuickSortDouble(averageTimeSpent, urlPages);
            results = _uadService.FormatTop5Pages(urlPages, averageTimeSpent);

            return results;
        }

        /// <summary>
        /// Function that returns the top 5 most used features of the website
        /// </summary>
        /// <param name="month">specific month</param>
        /// <returns>the top 5 most used features</returns>
        public string GetTop5MostUsedFeature(string month, int year)
        {
            loglist = _uadService.GetLogsForMonthAndYear(loglist, month, year);
            var features = new List<String>() { "EventCreated", "EventJoined", "SearchAction", "FindEventForMe", "UserRatings"};
            var timesFeaturedUsed = new List<int>() {};
            Dictionary<string, int> listOfIDs = _gngLoggerService.GetLogIDs();
            var result = "";

            // For each feature get the amount of times they were used
            for (int i = 0; i < features.Count; i++)
            {
                // Get the log id for the feature
                listOfIDs.TryGetValue(features[i], out int clickLogID);
                var logID = clickLogID.ToString();
                var timesUsed = _uadService.GetNumberofLogsID(loglist, logID);
                timesFeaturedUsed.Add(timesUsed);
            }
            _uadService.QuickSortInteger(timesFeaturedUsed, features, 0, features.Count - 1);
            result = _uadService.FormatTop5Features(features, timesFeaturedUsed);

            return result;
        }

        /// <summary>
        /// Funcion that gets the average session duration over six months
        /// </summary>
        /// <param name="month">specific month to start from</param>
        /// <returns>the average session duration over six months</returns>
        public string GetAverageSessionMonthly(string month, int year)
        {
            var logID = new List<string>() { "1004", "1005" };
            var averages = new List<string>() { };
            var monthsUsed = new List<string> { };
            var monthIndex = 0;
            var result = "";

            for (int index = 0; index < 6; index++)
            {
                var entryToWebsite = new List<GNGLog>(loglist);
                var exitFromWebsite = new List<GNGLog>(loglist);
                var sessions = new List<GNGLog>();
                if (monthIndex >= 0)
                {
                    monthIndex = months.IndexOf(month) - index;
                }
                if (monthIndex < 0)
                {
                    monthIndex = monthIndex + months.Count;
                    year = year - 1;
                }
                // Get entry to website logs
                entryToWebsite = _uadService.GetLogsForMonthAndYear(entryToWebsite, months[monthIndex], year);
                entryToWebsite = _uadService.GetLogswithID(entryToWebsite, logID[0]);
                // Get exit from website logs
                exitFromWebsite = _uadService.GetLogsForMonthAndYear(exitFromWebsite, months[monthIndex], year);
                exitFromWebsite = _uadService.GetLogswithID(exitFromWebsite, logID[1]);

                monthsUsed.Add(months[monthIndex]);
                sessions = _uadService.PairStartAndEndLogs(entryToWebsite, exitFromWebsite);
                if (sessions.Count == 0)
                {
                    averages.Add("0");
                }
                else
                {
                    var average = _uadService.CalculateAverageSessionTime(sessions);
                    string sAverage = String.Format("{0:0.00}", average);
                    averages.Add(sAverage);
                }
            }
            result =
                                    monthsUsed[0] + ": " + averages[0] + " " +
                                    monthsUsed[1] + ": " + averages[1] + " " +
                                    monthsUsed[2] + ": " + averages[2] + " " +
                                    monthsUsed[3] + ": " + averages[3] + " " +
                                    monthsUsed[4] + ": " + averages[4] + " " +
                                    monthsUsed[5] + ": " + averages[5];

            return result;
        }

        /// <summary>
        /// Function that returns the number of logins over six months
        /// </summary>
        /// <returns>number of logins over six months</returns>
        public string GetLoggedInMonthly(string month, int year)
        {
            var logins = new List<int>() { };
            var monthsUsed = new List<string> {};
            var monthIndex = 0;
            var result = "";

            for (int index = 0; index < 6; index++)
            {
                if (monthIndex >= 0)
                {
                    monthIndex = months.IndexOf(month) - index;
                }
                if (monthIndex < 0)
                {
                    monthIndex = monthIndex + months.Count;
                    year = year - 1;
                }
                loglist = _gngLoggerService.ReadLogs();
                loglist = _uadService.GetLogsForMonthAndYear(loglist, months[monthIndex], year);
                monthsUsed.Add(months[monthIndex]);
                var loginCount = _uadService.GetNumberofLogsID(loglist, "1004");
                logins.Add(loginCount);
            }
            result =
                                    monthsUsed[0] + ": " + logins[0] + " " +
                                    monthsUsed[1] + ": " + logins[1] + " " +
                                    monthsUsed[2] + ": " + logins[2] + " " +
                                    monthsUsed[3] + ": " + logins[3] + " " +
                                    monthsUsed[4] + ": " + logins[4] + " " +
                                    monthsUsed[5] + ": " + logins[5];
        return result;
        }
    }

}