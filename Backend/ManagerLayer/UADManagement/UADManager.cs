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
        private LoggerService _gngLoggerService = new LoggerService();
        private UADService _uadService = new UADService();
        private UserService _userService = new UserService();
        private SortService _sortService = new SortService();
        private static List<string> months = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private List<string> monthsUsed = new List<string>();
        private List<UADObject> uadObjects = new List<UADObject>();

        public UADManager()
        {
        }
        /// <summary>
        /// Function that gets the number of registered accounts and compared to login information(average logins, min, max)
        /// </summary>
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns>A list of objects that holds login information(average, min, max) and registered users for a month</returns>
        public List<UADObject> GetLoginComparedToRegistered(string month, int year)
        {
            var registered = _userService.GetRegisteredUserCount();
            var monthIndex = months.IndexOf(month);
            string[] informationList = { "Average Logins", "Minimum Logins", "Maximum Logins", "Registered Accounts" };
            
            loglist = _gngLoggerService.ReadLogsGivenMonthYear(months[monthIndex], year);
            var loginInfo = _uadService.GetLoginInfo(loglist);
            loginInfo.Add(registered.ToString("0.##"));
            var date = months[monthIndex] + " " + year;
            monthsUsed.Add(date);
            uadObjects.AddRange(_uadService.ConvertListToUADObjects(monthsUsed, informationList, loginInfo));
            monthsUsed.Clear();

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
            string[] logID = { "EntryToWebsite", "FailedLogin" };
            string[] informationList = { "Successful Logins", "Unsuccessful Logins" };
            var valueList = new List<string>();
            var monthIndex = months.IndexOf(month);

            loglist = _gngLoggerService.ReadLogsGivenMonthYear(months[monthIndex], year);
            valueList.Add(_uadService.GetNumberofLogsID(loglist, logID[0]).ToString());
            valueList.Add(_uadService.GetNumberofLogsID(loglist, logID[1]).ToString());
            var date = months[monthIndex] + " " + year;
            monthsUsed.Add(date);
            uadObjects.AddRange(_uadService.ConvertListToUADObjects(monthsUsed, informationList, valueList));
            monthsUsed.Clear();

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
            string[] logID = { "EntryToWebsite", "ExitFromWebsite" };
            string[] informationList = { "Average Session Duration", "Minimum Session Duration", "Maximum Session Duration" };
            var sessions = new List<GNGLog>();
            var sessionInfo = new List<string> { "0", "0", "0" };

            loglist = _gngLoggerService.ReadLogsGivenMonthYear(month, year);
            // Get two seperate logs 
            var entryToWebsite = _uadService.GetLogswithID(loglist, logID[0]);
            // Get exit from website logs
            var exitFromWebsite = _uadService.GetLogswithID(loglist, logID[1]);
            // Pair sessions beginning and ending logs
            sessions = _uadService.PairStartAndEndLogs(entryToWebsite, exitFromWebsite);
            if (sessions.Count > 0)
            {
                sessionInfo = _uadService.CalculateSessionInformation(sessions);
            }
            var date = month + " " + year;
            monthsUsed.Add(date);
            uadObjects.AddRange(_uadService.ConvertListToUADObjects(monthsUsed, informationList, sessionInfo));
            monthsUsed.Clear();

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
            var sessions = new List<GNGLog>();
            var urlPages =  new List<string> { "https://www.greetngroup.com/search", "https://www.greetngroup.com/createevent", "https://www.greetngroup.com", "https://www.greetngroup.com/help", "https://www.greetngroup.com/faq" };
            var averageTimeSpent = new List<double>() ;
            var monthIndex = months.IndexOf(month);

            var loglist = _gngLoggerService.ReadLogsGivenMonthYear(months[monthIndex], year);
            // For every URL in the list get the average time spent on it
            for (int j = 0; j < urlPages.Count; j++)
            {
                // Get logs where user enters a url
                var entryToPage = _uadService.GetEntryLogswithURL(loglist, urlPages[j]);
                // Get Logs when user leaves a url
                var exitFromPage = _uadService.GetExitLogswithURL(loglist, urlPages[j]);
                // Pair the exit from pages with the entrance to a page
                sessions = _uadService.PairStartAndEndLogs(entryToPage, exitFromPage);
                if (sessions.Count == 0)
                {
                    averageTimeSpent.Add(0);
                }
                else
                {
                    // Get average time spent on the page
                    var average = _uadService.CalculateSessionInformation(sessions);
                    averageTimeSpent.Add(Convert.ToDouble(average[0]));
                }
                sessions.Clear();
            }
            //Sort Pages view time by shortest to longest
            _sortService.QuickSortDouble(averageTimeSpent, urlPages);
            // Transform list into UADObjects
            if (urlPages.Count >= 5)
            {
                for (int k = urlPages.Count - 1; k >= urlPages.Count - 5; k--)
                {
                    var sessionUADObject = new UADObject
                    {
                        Date = months[monthIndex] + ' ' + year,
                        InfoType = urlPages[k],
                        Value = averageTimeSpent[k].ToString()
                    };
                    uadObjects.Add(sessionUADObject);
                }
            }
            else
            {
                for (int k = urlPages.Count - 1; k >= 0; k--)
                {
                    var sessionUADObject = new UADObject
                    {
                        Date = month + ' ' + year,
                        InfoType = urlPages[k],
                        Value = averageTimeSpent[k].ToString()
                    };
                    uadObjects.Add(sessionUADObject);
                }
            }
            monthsUsed.Clear();

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
            var features = new List<String>() { "EventCreated", "EventJoined", "SearchAction", "FindEventForMe", "UserRatings" };
            var timesFeaturedUsed = new List<int>() { };

            loglist = _gngLoggerService.ReadLogsGivenMonthYear(month, year);
            // List of features wanting to be analyzed

            // For each feature get the amount of times they were used
            for (int j = 0; j < features.Count; j++)
            {
                // Get the log id for the feature
                var timesUsed = _uadService.GetNumberofLogsID(loglist, features[j]);
                timesFeaturedUsed.Add(timesUsed);
            }
            // Sort Features from 
            _sortService.QuickSortInteger(timesFeaturedUsed, features, 0, features.Count - 1);
            if (features.Count >= 5)
            {
                for (int k = features.Count - 1; k >= features.Count - 5; k--)
                {
                    var sessionUADObject = new UADObject
                    {
                        Date = month + ' ' + year,
                        InfoType = features[k],
                        Value = timesFeaturedUsed[k].ToString()
                    };
                    uadObjects.Add(sessionUADObject);
                }
            }
            else
            {
                for (int k = features.Count - 1; k >= 0; k--)
                {
                    var sessionUADObject = new UADObject
                    {
                        Date = month + ' ' + year,
                        InfoType = features[k],
                        Value = timesFeaturedUsed[k].ToString()
                    };
                    uadObjects.Add(sessionUADObject);
                }
            }

            return uadObjects;
        }
        /// <summary>
        /// Funcion that gets the average session duration over six previous months
        /// </summary>
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns>A list of objects that holds the average session duration over six months</returns>
        public List<UADObject> GetAverageSessionMonthly(string month, int year)
        {
            string[] logID = { "EntryToWebsite", "ExitFromWebsite" };
            var averages = new List<string>();
            var sessions = new List<GNGLog>();

            loglist = _gngLoggerService.ReadLogsGivenMonthYear(month, year);
            // Get entry to website logs
            var entryToWebsite = _uadService.GetLogswithID(loglist, logID[0]);
            // Get exit from website logs
            var exitFromWebsite = _uadService.GetLogswithID(loglist, logID[1]);
            // Add the currently used month
            sessions = _uadService.PairStartAndEndLogs(entryToWebsite, exitFromWebsite);
            if (sessions.Count == 0)
            {
                averages.Add("0");
            }
            else
            {
                var average = _uadService.CalculateSessionInformation(sessions);
                averages.Add(average[0]);
            }
            var sessionUADObject = new UADObject
            {
                Date = month + ' ' + year,
                InfoType = "Average Session",
                Value = averages[0]
            };
            uadObjects.Add(sessionUADObject);

            return uadObjects;
        }

        /// <summary>
        /// Function that returns the number of logins over six previous months
        /// </summary>
        /// <param name="month">referenced month</param>
        /// <param name="year">referenced year</param>
        /// <returns>A list of objects that holds the number of logins over six months</returns>
        public List<UADObject> GetLoggedInMonthly(string month, int year)
        {
            loglist = _gngLoggerService.ReadLogsGivenMonthYear(month, year);
            var loginCount = _uadService.GetNumberofLogsID(loglist, "EntryToWebsite");

            var loginUADObject = new UADObject
            {
                Date = month + ' ' + year,
                InfoType = "Logins",
                Value = loginCount.ToString()
            };
            uadObjects.Add(loginUADObject);

            return uadObjects;
        }
    }

}