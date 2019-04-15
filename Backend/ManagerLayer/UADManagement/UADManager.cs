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
        private static List<string> months = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "Decemeber" };
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
            string loginID = "1004";
            loglist = _uadService.GetLogsFortheMonth(loglist, month);
            int registered = userManager.GetRegisteredUserCount();
            int loginCount = _uadService.GetNumberofLogsID(loglist, loginID);
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
            entryToWebsite = _gngLoggerService.ReadLogs();
            entryToWebsite = _uadService.GetLogsFortheMonth(entryToWebsite, month);
            entryToWebsite = _uadService.GetLogswithID(entryToWebsite, logID[0]);

            exitFromWebsite = _gngLoggerService.ReadLogs();
            exitFromWebsite = _uadService.GetLogsFortheMonth(exitFromWebsite, month);
            exitFromWebsite = _uadService.GetLogswithID(exitFromWebsite, logID[1]);

            //Might Need to turn into function
            //Pair sessions beginning and ending logs
            for (int k = 0; k < exitFromWebsite.Count; k++)
            {
                bool notFound = true;
                int index = 0;
                while (notFound == true)
                {
                    if (string.Compare(entryToWebsite[index].userID, exitFromWebsite[k].userID) == 0)
                    {
                        sessions.Add(exitFromWebsite[k]);
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
            string averageSession = "Average Session Time for " + month + ": " + sAverage;
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
            List<GNGLog> entryToPage = new List<GNGLog>();
            List<GNGLog> exitFromPage = new List<GNGLog>();
            List<GNGLog> sessions = new List<GNGLog>();

            for (int i = 0; i < urlPages.Count; i++)
            {
                //Get logs where user enters a url
                entryToPage = _gngLoggerService.ReadLogs();
                entryToPage = _uadService.GetLogsFortheMonth(entryToPage, month);
                entryToPage = _uadService.GetLogswithID(entryToPage, "1001");
                _uadService.GetEntryLogswithURL(entryToPage, urlPages[i]);

                //Get Logs when user leaves a url
                exitFromPage = _gngLoggerService.ReadLogs();
                exitFromPage = _uadService.GetLogsFortheMonth(exitFromPage, month);
                _uadService.GetExitLogswithURL(exitFromPage, urlPages[i]);

                //Might need to turn into function
                //Pair the exit from pages with the entrance to a page
                for (int k = 0; k < exitFromPage.Count; k++)
                {
                    bool notFound = true;
                    int index = 0;
                    while (notFound == true)
                    {
                        if (string.Compare(entryToPage[index].userID, exitFromPage[k].userID) == 0)
                        {
                            sessions.Add(exitFromPage[k]);
                            sessions.Add(entryToPage[index]);
                            entryToPage.Remove(entryToPage[index]);
                            notFound = false;
                        }
                        else
                        {
                            index++;
                        }
                    }
                }

                //Get average time spent on the page
                double average = _uadService.CalculateAverageSessionTime(sessions);
                averageTimeSpent[i] = average;
            }
            //Sort Pages view time by shortest to longest
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
            List<int> timesFeaturedUsed = new List<int>() {};
            Dictionary<string, int> listOfIDs = _gngLoggerService.GetLogIDs();
            //For each feature get the amount of times they were used
            for (int i = 0; i < features.Count; i++)
            {
                //Get the log id for the feature
                listOfIDs.TryGetValue(features[i], out int clickLogID);
                string logID = clickLogID.ToString();
                int timesUsed = _uadService.GetNumberofLogsID(loglist, logID);
                timesFeaturedUsed.Add(timesUsed);
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
            List<string> logID = new List<string>() { "1004", "1005" };
            List<string> averages = new List<string>() { };
            List<GNGLog> entryToWebsite = new List<GNGLog>();
            List<GNGLog> exitFromWebsite = new List<GNGLog>();
            List<GNGLog> sessions = new List<GNGLog>();
            List<string> monthsUsed = new List<string> { };
            int monthIndex = 0;

            for (int index = 0; index < months.Count; index++)
            {
                if (monthIndex >= 0)
                {
                    monthIndex = months.IndexOf(month) - index;
                }
                else
                {
                    monthIndex = monthIndex + months.Count;
                }

                entryToWebsite = _gngLoggerService.ReadLogs();
                entryToWebsite = _uadService.GetLogsFortheMonth(entryToWebsite, months[monthIndex]);
                entryToWebsite = _uadService.GetLogswithID(entryToWebsite, logID[0]);

                exitFromWebsite = _gngLoggerService.ReadLogs();
                exitFromWebsite = _uadService.GetLogsFortheMonth(exitFromWebsite, months[monthIndex]);
                exitFromWebsite = _uadService.GetLogswithID(exitFromWebsite, logID[1]);

                monthsUsed.Add(months[monthIndex]);
                //Might Need to turn into function
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
                                  monthsUsed[0] + ": " + averages[0] +
                                  monthsUsed[1] + ": " + averages[1] +
                                  monthsUsed[2] + ": " + averages[2] +
                                  monthsUsed[3] + ": " + averages[3] +
                                  monthsUsed[4] + ": " + averages[4] +
                                  monthsUsed[5] + ": " + averages[5];

            Console.WriteLine(monthlyAverages);
            return monthlyAverages;
        }

        /// <summary>
        /// Function that returns the number of logins over six months
        /// </summary>
        /// <returns>number of logins over six months</returns>
        public string GetLoggedInMonthly(string month)
        {
            
            List<int> logins = new List<int>() { };
            List<string> monthsUsed = new List<string> {};
            int monthIndex = 0;
            for (int index = 0; index < 6; index++)
            {
                if(monthIndex >= 0)
                {
                    monthIndex = months.IndexOf(month) - index;
                }
                else
                {
                    monthIndex = monthIndex + months.Count;
                }
                loglist = _gngLoggerService.ReadLogs();
                loglist = _uadService.GetLogsFortheMonth(loglist, months[monthIndex]);
                monthsUsed.Add(months[monthIndex]);
                int loginCount = _uadService.GetNumberofLogsID(loglist, "1004");
                logins.Add(loginCount);
            }
            string monthlyLogin =
                                  monthsUsed[0] + ": " + logins[0] +
                                  monthsUsed[1] + ": " + logins[1] +
                                  monthsUsed[2] + ": " + logins[2] +
                                  monthsUsed[3] + ": " + logins[3] +
                                  monthsUsed[4] + ": " + logins[4] +
                                  monthsUsed[5] + ": " + logins[5];
            Console.WriteLine(monthlyLogin);
            return monthlyLogin;
        }


    }

}