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
        LoggerService _gngLoggerService;
        UADService _uadService;
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
        }

        /// <summary>
        /// Function that gets the number of registered accounts and compared to login information(average logins, min, max)
        /// </summary>
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns>A list of objects that holds login information(average, min, max) and registered users for a month</returns>
        public List<UADObject> GetLoginComparedToRegistered(string month, int year)
        {
            loglist = _gngLoggerService.ReadLogsGivenMonthYear(month, year);
            var loginInfo = _uadService.GetLoginInfo(loglist);
            var registered = _userService.GetRegisteredUserCount();
            loginInfo.Add(registered.ToString("0.##"));
            string[] informationList = { "Average Logins", "Minimum Logins", "Maximum Logins", "Registered Accounts" };
            uadObjects = _uadService.ConvertListToUADObjects(informationList, loginInfo);
            return uadObjects;
        }
        /// <summary>
        /// Function that gets the number of successful logins compared to the number of Failed logins for a specfied month and year
        /// </summary>
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns></returns>
        public List<UADObject> GetLoginSuccessFail(string month, int year)
        {
            loglist = _gngLoggerService.ReadLogsGivenMonthYear(month, year);
            string[] logID = {"EntrytoWebsite", "FailedLogin"};
            string[] informationList = {"Sucessful Logins", "Unsucessful Logins"};
            var valueList = new List<string>();
            valueList.Add(_uadService.GetNumberofLogsID(loglist, logID[0]).ToString());
            valueList.Add(_uadService.GetNumberofLogsID(loglist, logID[1]).ToString());
            uadObjects = _uadService.ConvertListToUADObjects(informationList, valueList);
            return uadObjects;
        }
        /// <summary>
        /// Function that returns the average session duration for a month
        /// </summary>
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns>An object that holds the number the average session duration for a month</returns>
        public List<UADObject> GetAverageSessionDuration(string month, int year)
        {
            loglist = _gngLoggerService.ReadLogsGivenMonthYear(month, year);
            string[] logID = { "EntryToWebsite", "ExitFromWebsite" };
            var entryToWebsite = new List<GNGLog>(loglist);
            var exitFromWebsite = new List<GNGLog>(loglist);
            var sessions = new List<GNGLog>();
            var sessionInfo = new List<string> {"0", "0", "0"};
            string[] informationList = { "Average Session Duration", "Minimum Session Duration", "Maximum Session Duration"};
            // Get two seperate logs 
            entryToWebsite = _uadService.GetLogswithID(entryToWebsite, logID[0]);
            // Get exit from website logs
            exitFromWebsite = _uadService.GetLogswithID(exitFromWebsite, logID[1]);
            // Pair sessions beginning and ending logs
            sessions = _uadService.PairStartAndEndLogs(entryToWebsite, exitFromWebsite);
            if (sessions.Count > 0)
            {
                sessionInfo = _uadService.CalculateAverageSessionInformation(sessions);
            }
            uadObjects = _uadService.ConvertListToUADObjects(informationList, sessionInfo);
            return uadObjects;
        }

        /// <summary>
        /// Function that returns the top 5 page used for a month
        /// </summary>
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns>A list of objects that holds the top 5 page used for a month</returns>
        public List<UADObject> GetTop5AveragePageSession(string month, int year)
        {
            loglist = _gngLoggerService.ReadLogsGivenMonthYear(month, year);
            var entryToPage = new List<GNGLog>();
            var exitFromPage = new List<GNGLog>();
            var sessions = new List<GNGLog>();
            string[] urlPages =  { "https://www.greetngroup.com/search", "https://www.greetngroup.com/createevent", "https://www.greetngroup.com", "https://www.greetngroup.com/help", "https://www.greetngroup.com/faq" };
            var averageTimeSpent = new List<double>() ;

            // For every URL in the list get the average time spent on it
            for (int i = 0; i < urlPages.Length; i++)
            {
                entryToPage.AddRange(loglist);
                exitFromPage.AddRange(loglist);
                // Get logs where user enters a url
                _uadService.GetEntryLogswithURL(entryToPage, urlPages[i]);
                // Get Logs when user leaves a url
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
                    var average = _uadService.CalculateAverageSessionInformation(sessions);
                    averageTimeSpent.Add(Convert.ToDouble(average[0]));
                }
                entryToPage.Clear();
                exitFromPage.Clear();
                sessions.Clear();
            }
            //Sort Pages view time by shortest to longest
            _sortService.QuickSortDouble(averageTimeSpent, urlPages);
            // Incase there are more than 5 urls
            if (urlPages.Length >= 5)
            {
                for (int i = urlPages.Length - 1; i >= urlPages.Length - 5; i--)
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
                for (int i = urlPages.Length - 1; i >= 0; i--)
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
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns>A list of objects that holds the top 5 most used features</returns>
        public List<UADObject> GetTop5MostUsedFeature(string month, int year)
        {

            loglist = _gngLoggerService.ReadLogsGivenMonthYear(month, year);
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
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns>A list of objects that holds the average session duration over six months</returns>
        public List<UADObject> GetAverageSessionMonthly(string month, int year)
        {
            string[] logID = { "EntryToWebsite", "ExitFromWebsite" };
            var averages = new List<string>();
            var monthIndex = months.IndexOf(month); 

            var entryToWebsite = new List<GNGLog>();
            var exitFromWebsite = new List<GNGLog>();
            var sessions = new List<GNGLog>();
            for (int i = 0; i < 6; i++)
            {
                if (monthIndex < 0)
                {
                    monthIndex = monthIndex + months.Count;
                    year = year - 1;
                }
                loglist = _gngLoggerService.ReadLogsGivenMonthYear(months[monthIndex], year);
                entryToWebsite.AddRange(loglist);
                exitFromWebsite.AddRange(loglist);
                // Get entry to website logs
                entryToWebsite = _uadService.GetLogswithID(entryToWebsite, logID[0]);
                // Get exit from website logs
                exitFromWebsite = _uadService.GetLogswithID(exitFromWebsite, logID[1]);
                // Add the currently used month
                sessions = _uadService.PairStartAndEndLogs(entryToWebsite, exitFromWebsite);
                if (sessions.Count == 0)
                {
                    averages.Add("0");
                }
                else
                {
                    var average = _uadService.CalculateAverageSessionInformation(sessions);
                    averages.Add(average[0]);
                }
                var sessionUADObject = new UADObject
                {
                    InfoType = months[monthIndex],
                    Value = averages[i]
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
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns>A list of objects that holds the number of logins over six months</returns>
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
                loglist = _gngLoggerService.ReadLogsGivenMonthYear(months[monthIndex], year);
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