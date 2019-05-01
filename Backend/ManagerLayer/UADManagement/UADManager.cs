using Gucci.DataAccessLayer.Models;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Services;
using System;
using System.Collections.Generic;

namespace Gucci.ManagerLayer.UADManagement
{
    public class UADManager
    {
        private List<GNGLog> loglist = new List<GNGLog>();
        ILoggerService _gngLoggerService;
        IUADService _uadService;
        UserService _userService;
        ISortService _sortService;
        private static List<string> months = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private List<UADObject> uadObjects = new List<UADObject>();
        public UADManager()
        {
            _gngLoggerService = new LoggerService();
            _uadService = new UADService();
            _userService = new UserService();
            _sortService = new SortService();
            loglist = _gngLoggerService.ReadLogs();
        }

        /// <summary>
        /// Function that returns the number of registered accounts compared to the number of logins for a month
        /// </summary>
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns>A list of objects that hold number of logins and registered users for a month</returns>
        public List<UADObject> GetLoginComparedToRegistered(string month, int year)
        {
            var loginID = "EntryToWebsite";
            loglist = _uadService.GetLogsForMonthAndYear(loglist, month, year);
            var registered = _userService.GetRegisteredUserCount();
            var loginCount = _uadService.GetNumberofLogsID(loglist, loginID);

            var loginUADObject = new UADObject
            {
                InfoType = "Logins",
                Value = loginCount.ToString()
            };
            var registeredUADObject = new UADObject
            {
                InfoType = "Registered",
                Value = registered.ToString()
            };

            uadObjects.Add(loginUADObject);
            uadObjects.Add(registeredUADObject);
            return uadObjects;
        }

        /// <summary>
        /// Function that returns the average session duration for a month
        /// </summary>
        /// <returns>the average session duration for a month</returns>
        public UADObject GetAverageSessionDuration(string month, int year)
        {
            string[] logID = { "EntryToWebsite", "ExitFromWebsite" };
            var entryToWebsite = new List<GNGLog>(loglist);
            var exitFromWebsite = new List<GNGLog>(loglist);
            var sessions = new List<GNGLog>();
            var average = 0.0;

            // Get two seperate logs 
            entryToWebsite = _uadService.GetLogsForMonthAndYear(entryToWebsite, month, year);
            exitFromWebsite = _uadService.GetLogsForMonthAndYear(exitFromWebsite, month, year);

            // Get logs for when they leave the website and for when they enter it
            entryToWebsite = _uadService.GetLogswithID(entryToWebsite, logID[0]);
            exitFromWebsite = _uadService.GetLogswithID(exitFromWebsite, logID[1]);

            // Pair sessions beginning and ending logs
            sessions = _uadService.PairStartAndEndLogs(entryToWebsite, exitFromWebsite);
            if (sessions.Count > 0)
            {
                average = _uadService.CalculateAverageSessionTime(sessions);
            }
            var sessionUADObject = new UADObject
            {
                InfoType = "Average Session Duration",
                Value = average.ToString()
            };
            return sessionUADObject;
        }

        /// <summary>
        /// Function that returns the top 5 page used for a month
        /// </summary>
        /// <param name="month">specific month</param>
        /// <returns>top 5 page used for a month</returns>
        public List<UADObject> GetTop5AveragePageSession(string month, int year)
        {
            var urlPages = new List<string>() { "https://www.greetngroup.com/createevent", "https://www.greetngroup.com", "https://www.greetngroup.com/search", "https://www.greetngroup.com/findeventsforme", "https://www.greetngroup.com/help", "https://www.greetngroup.com/faq" };
            var averageTimeSpent = new List<double>() ;

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
            _sortService.QuickSortDouble(averageTimeSpent, urlPages);
            if (urlPages.Count >= 5)
            {
                for (int i = urlPages.Count - 1; i >= urlPages.Count - 5; i--)
                {
                    var sessionUADObject = new UADObject
                    {
                        InfoType = urlPages[i],
                        Value = averageTimeSpent[i].ToString()
                    };
                    uadObjects.Add(sessionUADObject);
                }
            }
            else
            {
                for (int i = urlPages.Count - 1; i >= 0; i--)
                {
                    var sessionUADObject = new UADObject
                    {
                        InfoType = urlPages[i],
                        Value = averageTimeSpent[i].ToString()
                    };
                    uadObjects.Add(sessionUADObject);
                }
            }
            return uadObjects;
        }

        /// <summary>
        /// Function that returns the top 5 most used features of the website
        /// </summary>
        /// <param name="month">specific month</param>
        /// <returns>the top 5 most used features</returns>
        public List<UADObject> GetTop5MostUsedFeature(string month, int year)
        {
            loglist = _uadService.GetLogsForMonthAndYear(loglist, month, year);
            // List of features wanting to be analyzed
            var features = new List<String>() { "EventCreated", "EventJoined", "SearchAction", "FindEventForMe", "UserRatings"};
            var timesFeaturedUsed = new List<int>() {};
            // For each feature get the amount of times they were used
            for (int i = 0; i < features.Count; i++)
            {
                // Get the log id for the feature
                var timesUsed = _uadService.GetNumberofLogsID(loglist, features[i]);
                timesFeaturedUsed.Add(timesUsed);
            }
            // Sort Features from 
            _sortService.QuickSortInteger(timesFeaturedUsed, features, 0, features.Count - 1);
            if (features.Count >= 5)
            {
                for (int i = features.Count - 1; i >= features.Count - 5; i--)
                {
                    var sessionUADObject = new UADObject
                    {
                        InfoType = features[i],
                        Value = timesFeaturedUsed[i].ToString()
                    };
                    uadObjects.Add(sessionUADObject);
                }
            }
            else
            {
                for (int i = features.Count - 1; i >= 0; i--)
                {
                    var sessionUADObject = new UADObject
                    {
                        InfoType = features[i],
                        Value = timesFeaturedUsed[i].ToString()
                    };
                    uadObjects.Add(sessionUADObject);
                }
            }
            return uadObjects;
        }

        /// <summary>
        /// Funcion that gets the average session duration over six months
        /// </summary>
        /// <param name="month">specific month to start from</param>
        /// <returns>the average session duration over six months</returns>
        public List<UADObject> GetAverageSessionMonthly(string month, int year)
        {
            var logID = new List<string>() { "EntryToWebsite", "ExitFromWebsite" };
            var averages = new List<string>();
            var monthIndex = months.IndexOf(month); 

            var entryToWebsite = new List<GNGLog>();
            var exitFromWebsite = new List<GNGLog>();
            var sessions = new List<GNGLog>();
            for (int i = 0; i < 6; i++)
            {
                entryToWebsite.AddRange(loglist);
                exitFromWebsite.AddRange(loglist);
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
                // Add the currently used month
                sessions = _uadService.PairStartAndEndLogs(entryToWebsite, exitFromWebsite);
                if (sessions.Count == 0)
                {
                    averages.Add("0");
                }
                else
                {
                    var average = _uadService.CalculateAverageSessionTime(sessions);
                    averages.Add(average.ToString());
                }
                var sessionUADObject = new UADObject
                {
                    InfoType = months[monthIndex],
                    Value = averages[i].ToString()
                };
                uadObjects.Add(sessionUADObject);
                monthIndex--;
                // Clear List for next month
                entryToWebsite.Clear();
                exitFromWebsite.Clear();
                sessions.Clear();
            }
            return uadObjects;
        }

        /// <summary>
        /// Function that returns the number of logins over six months
        /// </summary>
        /// <returns>number of logins over six months</returns>
        public List<UADObject> GetLoggedInMonthly(string month, int year)
        {
            var monthIndex = months.IndexOf(month);
            for (int i = 0; i < 6; i++)
            {
                if (monthIndex < 0)
                {
                    monthIndex = monthIndex + months.Count;
                    year = year - 1;
                }
                loglist = _gngLoggerService.ReadLogs();
                loglist = _uadService.GetLogsForMonthAndYear(loglist, months[monthIndex], year);
                var loginCount = _uadService.GetNumberofLogsID(loglist, "EntryToWebsite");
                var loginUADObject = new UADObject
                {
                    InfoType = months[monthIndex],
                    Value = loginCount.ToString()
                };
                uadObjects.Add(loginUADObject);
                monthIndex--;
            }
            return uadObjects;
        }
    }

}