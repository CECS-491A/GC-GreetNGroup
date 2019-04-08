using DataAccessLayer.Models;
using ServiceLayer.Interface;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;

namespace ManagerLayer.UADManagement
{
    public class UADManager
    {
        private static List<GNGLog> loglist = new List<GNGLog>();
        IGNGLoggerService _gngLoggerService = new GNGLoggerService();
        IUADService _uadService = new UADService();
        UserService userManager = new UserService();
        public UADManager()
        {
            loglist = _gngLoggerService.ReadLogs();
        }

        /// <summary>
        /// Function that returns the number of registered accounts vs the number of logins for a month
        /// </summary>
        /// <param name="month">specific month</param>
        /// <returns>the number of logins vs registered users for a month</returns>
        public string GetLoginVSRegistered(string month)
        {
            string registerID = "1004";
            loglist = _uadService.GetLogsFortheMonth(loglist, month);
            int loginCount = _uadService.GetNumberofLogs(loglist, registerID);
            int registered = userManager.GetRegisteredUserCount();
            string loginVSregistered = "Logins: " + loginCount +
                                    "\nRegistered Accounts: " + registered;
            Console.WriteLine(loginVSregistered);
            return loginVSregistered;
        }

        /// <summary>
        /// Function that returns the average session duration for a month
        /// </summary>
        /// <returns>the average session duration for a month</returns>
        public string GetAverageSessionDuration(string month)
        {
            List<string> logID = new List<string>() { "1004", "1005" };
            List<GNGLog> entryToWebsite = new List<GNGLog>();
            List<GNGLog> exitFromWebsite = new List<GNGLog>();
            List<GNGLog> sessions = new List<GNGLog>();

            //Seprate logs into when they enter the website and when they leave the website
            for (int i = 0; i < logID.Count; i++)
            {
                if (i == 0)
                {
                    entryToWebsite = _gngLoggerService.ReadLogs();
                    entryToWebsite = _uadService.GetLogsFortheMonth(entryToWebsite, month);
                    entryToWebsite = _uadService.GetLogswithID(entryToWebsite, logID[i]);
                }
                else
                {
                    exitFromWebsite = _gngLoggerService.ReadLogs();
                    exitFromWebsite = _uadService.GetLogsFortheMonth(exitFromWebsite, month);
                    exitFromWebsite = _uadService.GetLogswithID(exitFromWebsite, logID[i]);
                }
            }
            //Pair session begginging and ending logs
            for (int k = 0; k < exitFromWebsite.Count; k++)
            {
                bool notFound = true;
                int index = 0;
                sessions.Add(exitFromWebsite[k]);
                while (notFound == true)
                {
                    if (string.Compare(entryToWebsite[index].userID, exitFromWebsite[k].userID) == 0)
                    {
                        sessions.Add(entryToWebsite[index]);
                        entryToWebsite.Remove(entryToWebsite[index]);
                        notFound = false;
                    }
                    else
                    {
                        index++;
                    }
                }
            }
            double average = _uadService.CalculateAverageSessionTime(sessions);
            string sAverage = String.Format("{0:0.00}", average);
            string averageSession = "Average Session Time for " + month + ": " + sAverage + " minutes";
            Console.WriteLine(averageSession);
            return averageSession;
        }

        /// <summary>
        /// Function that returns the top 5 page used for a month
        /// </summary>
        /// <param name="month">specific month</param>
        /// <returns>top 5 page used for a month</returns>
        public string GetTop5AveragePageSession(string month)
        {
            List<String> urlPages = new List<String>() { "https://www.greetngroup.com/create", "https://www.greetngroup.com/join", "https://www.greetngroup.com/searchUser", "https://www.greetngroup.com/searchEvent", "https://www.greetngroup.com/help", "https://www.greetngroup.com/faq" };
            double[] averageTimeSpent = { 0, 0, 0, 0, 0, 0 };
            
            for (int i = 0; i < urlPages.Count; i++)
            {
                List<GNGLog> entryToPage = new List<GNGLog>();
                List<GNGLog> exitFromPage = new List<GNGLog>();
                List<GNGLog> sessions = new List<GNGLog>();
                //Seperate logs into entrence to a page and when they exit a page
                for (int j = 0; j < 2; j++)
                {
                    if (j == 0)
                    {
                        entryToPage = _gngLoggerService.ReadLogs();
                        entryToPage = _uadService.GetLogsFortheMonth(entryToPage, month);
                        entryToPage = _uadService.GetLogswithID(entryToPage, "1001");
                        _uadService.GetEntryLogswithURL(entryToPage, urlPages[i]);
                    }
                    else
                    {
                        exitFromPage = _gngLoggerService.ReadLogs();
                        exitFromPage = _uadService.GetLogsFortheMonth(exitFromPage, month);
                        _uadService.GetExitLogswithURL(exitFromPage, urlPages[i]);
                    }

                }
                //Pair the exit from ages with the entrence to a page
                for (int k = 0; k < exitFromPage.Count; k++)
                {
                    bool notFound = true;
                    int pos = 0;
                    sessions.Add(exitFromPage[k]);
                    while (notFound == true)
                    {
                        if (string.Compare(entryToPage[pos].userID, exitFromPage[k].userID) == 0)
                        {
                            sessions.Add(entryToPage[pos]);
                            entryToPage.Remove(entryToPage[pos]);
                            notFound = false;
                        }
                        else
                        {
                            pos++;
                        }
                    }
                }
                //Calculate the average time spent on the page
                double average = _uadService.CalculateAverageSessionTime(sessions);
                averageTimeSpent[i] = average;
            }
            _uadService.QuickSortDouble(averageTimeSpent, urlPages);

            string pageUsed = 
                              "\n" + urlPages[5] + " average time spent: " + averageTimeSpent[5] +
                              "\n" + urlPages[4] + " average time spent: " + averageTimeSpent[4] +
                              "\n" + urlPages[3] + " average time spent: " + averageTimeSpent[3] +
                              "\n" + urlPages[2] + " average time spent: " + averageTimeSpent[2] +
                              "\n" + urlPages[1] + " average time spent: " + averageTimeSpent[1];
            Console.WriteLine(pageUsed);
            return pageUsed;
        }

        /// <summary>
        /// Function that returns the top 5 most used features of the website
        /// </summary>
        /// <param name="month">specific month</param>
        /// <returns>the top 5 most used features</returns>
        public string GetTop5MostUsedFeature(string month)
        {
            loglist = _uadService.GetLogsFortheMonth(loglist, month);
            List<String> features = new List<String>() { "EventCreated", "EventJoined", "SearchForUser", "FindEventForMe", "UserRatings", "ViewHistory" };
            List<int> timesFeaturedUsed = new List<int>() { 0, 0, 0, 0, 0, 0 };
            Dictionary<string, int> listOfIDs = _gngLoggerService.GetLogIDs();
            for (int i = 0; i < features.Count; i++)
            {
                listOfIDs.TryGetValue(features[i], out int clickLogID);
                string logID = clickLogID.ToString();
                int timesUsed = _uadService.GetNumberofLogs(loglist, logID);
                timesFeaturedUsed[i] = timesUsed;
            }
            _uadService.QuickSortInteger(timesFeaturedUsed, features, 0, features.Count - 1);
            string mostUsed =
                              "\n" + features[5] + " times used: " + timesFeaturedUsed[5] +
                              "\n" + features[4] + " times used: " + timesFeaturedUsed[4] +
                              "\n" + features[3] + " times used: " + timesFeaturedUsed[3] +
                              "\n" + features[2] + " times used: " + timesFeaturedUsed[2] +
                              "\n" + features[1] + " times used: " + timesFeaturedUsed[1];
            Console.WriteLine(mostUsed);
            return mostUsed;

        }

        /// <summary>
        /// Funcion that gets the average session duration over six months
        /// </summary>
        /// <param name="month">specific month to start from</param>
        /// <returns>the average session duration over six months</returns>
        public string GetAverageSessionMonthly(string month)
        {
            List<string> months = new List<string>() { "March", "April", "May", "June", "July", "August" };
            List<string> logID = new List<string>() { "1004", "1005" };
            List<string> averages = new List<string>() { };
            for (int index = 0; index < months.Count; index++)
            {
                List<GNGLog> entryToWebsite = new List<GNGLog>();
                List<GNGLog> exitFromWebsite = new List<GNGLog>();
                List<GNGLog> sessions = new List<GNGLog>();
                for (int id = 0; id < logID.Count; id++)
                {
                    if (id == 0)
                    {
                        entryToWebsite = _gngLoggerService.ReadLogs();
                        entryToWebsite = _uadService.GetLogsFortheMonth(entryToWebsite, months[index]);
                        entryToWebsite = _uadService.GetLogswithID(entryToWebsite, logID[id]);
                    }
                    else
                    {
                        exitFromWebsite = _gngLoggerService.ReadLogs();
                        exitFromWebsite = _uadService.GetLogsFortheMonth(exitFromWebsite, months[index]);
                        exitFromWebsite = _uadService.GetLogswithID(exitFromWebsite, logID[id]);
                    }
                }
                for (int k = 0; k < exitFromWebsite.Count; k++)
                {
                    bool notFound = true;
                    int pos = 0;
                    sessions.Add(exitFromWebsite[k]);
                    while (notFound == true)
                    {
                        if (string.Compare(entryToWebsite[pos].userID, exitFromWebsite[k].userID) == 0)
                        {
                            sessions.Add(entryToWebsite[pos]);
                            entryToWebsite.Remove(entryToWebsite[pos]);
                            notFound = false;
                        }
                        else
                        {
                            pos++;
                        }
                    }
                }
                double average = _uadService.CalculateAverageSessionTime(sessions);
                string sAverage = String.Format("{0:0.00}", average);
                averages.Add(sAverage);

            }
            string monthlyAverages =
                                  "\nMarch: " + averages[0] +
                                  "\nApril: " + averages[1] +
                                  "\nMay: " + averages[2] +
                                  "\nJune: " + averages[3] +
                                  "\nJuly: " + averages[4] +
                                  "\nAugust: " + averages[5];

            Console.WriteLine(monthlyAverages);

            return monthlyAverages;
        }

        /// <summary>
        /// Function that returns the number of logins over six months
        /// </summary>
        /// <returns>number of logins over six months</returns>
        public string GetLoggedInMonthly(string month)
        {
            List<string> months = new List<string>() { "March", "April", "May", "June", "July", "August" };
            List<int> logins = new List<int>() { };
            for (int index = 0; index < months.Count; index++)
            {
                loglist = _gngLoggerService.ReadLogs();
                loglist = _uadService.GetLogsFortheMonth(loglist, months[index]);
                int loginCount = _uadService.GetNumberofLogs(loglist, "1004");
                logins.Add(loginCount);
            }
            string monthlyLogin =
                                  "\nMarch: " + logins[0] +
                                  "\nApril: " + logins[1] +
                                  "\nMay: " + logins[2] +
                                  "\nJune: " + logins[3] +
                                  "\nJuly: " + logins[4] +
                                  "\nAugust: " + logins[5];
            Console.WriteLine(monthlyLogin);
            return monthlyLogin;
        }


    }

}