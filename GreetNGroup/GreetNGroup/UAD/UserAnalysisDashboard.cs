using GreetNGroup.DataAccess.Queries;
using GreetNGroup.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
namespace GreetNGroup.UAD
{

    public class UserAnalysisDashboard
    {
        
        public UserAnalysisDashboard()
        {
        }
        /// <summary>
        /// Reads all json files in directory and deserializes into GNGlog and puts them all into a Arraylist
        /// </summary>
        /// <returns></returns>
        private static void ReadLogs(List<GNGLog> loglist)
        {
            string LOGS_FOLDERPATH = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\GreetNGroup\\GreetNGroup\\Logs\\";
            DirectoryInfo di = new DirectoryInfo(LOGS_FOLDERPATH);
            string[] dirs = Directory.GetFiles(LOGS_FOLDERPATH, "*.json");
            
            foreach (string dir in dirs)
            {
                using (StreamReader r = new StreamReader(dir))
                {
                    string json = r.ReadToEnd();
                    List<GNGLog> logs = JsonConvert.DeserializeObject<List<GNGLog>>(json);
                    for(int index = 0; index < logs.Count; index++)
                    {
                        loglist.Add(logs[index]);
                    }
                }
                
               
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string LoginVSRegistered(string month)
        {

            List<GNGLog> loglist = new List<GNGLog>();
            UserAnalysisDashboard.ReadLogs(loglist);
            UADHelperFunctions.LogsFortheMonth(loglist, month);
            int loginCount = UADHelperFunctions.NumberofLogs(loglist, "1004");

            int registered = DbRetrieve.GetUsersRegistered();

            string failedVsSuccess = "Logins: " + loginCount +
                                    "\nRegistered Accounts: " + registered;

            Console.WriteLine(failedVsSuccess);
            return failedVsSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string AverageSessionDuration(string month)
        {
            List<string> logID = new List<string>() { "1004", "1005" };
            List<GNGLog> entryToWebsite = new List<GNGLog>();
            List<GNGLog> exitFromWebsite = new List<GNGLog>();
            List<GNGLog> sessions = new List<GNGLog>();
           
            for(int i = 0; i < logID.Count; i++)
            {

                if (i == 0)
                {
                    UserAnalysisDashboard.ReadLogs(entryToWebsite);
                    UADHelperFunctions.LogsFortheMonth(entryToWebsite, month);
                    UADHelperFunctions.LogswithID(entryToWebsite, logID[i]);
                }
                else
                {
                    UserAnalysisDashboard.ReadLogs(exitFromWebsite);
                    UADHelperFunctions.LogsFortheMonth(exitFromWebsite, month);
                    UADHelperFunctions.LogswithID(exitFromWebsite, logID[i]);
                }
                
            }
            
            for (int k = 0; k < exitFromWebsite.Count; k++)
            {
                bool notFound = true;
                int index = 0;
                sessions.Add(exitFromWebsite[k]);
                while(notFound == true)
                {
                    if(string.Compare(entryToWebsite[index].userID, exitFromWebsite[k].userID) == 0)
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
            double average = UADHelperFunctions.FindAverage(sessions);
            string sAverage = String.Format("{0:0.00}", average);

            string averageSession = "Average Session Time for " + month + ": " + sAverage + " minutes";

            Console.WriteLine(averageSession);
            return averageSession;

        }
        /// <summary>
        /// 
        /// </summary>
        public static string Top5AveragePageSession(string month)
        {
            List<String> urlPages = new List<String>() { "https://www.greetngroup.com/create", "https://www.greetngroup.com/join", "https://www.greetngroup.com/searchUser", "https://www.greetngroup.com/searchEvent", "https://www.greetngroup.com/help", "https://www.greetngroup.com/faq" };
            double[] averageTimeSpent = { 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < urlPages.Count; i++)
            {
                //List<string> logID = new List<string>() { "1001", "1005"};
                List<GNGLog> entryToPage = new List<GNGLog>();
                List<GNGLog> exitFromPage = new List<GNGLog>();
                List<GNGLog> sessions = new List<GNGLog>();

                for (int j = 0; j < 2; j++)
                {

                    if (j == 0)
                    {
                        UserAnalysisDashboard.ReadLogs(entryToPage);
                        UADHelperFunctions.LogsFortheMonth(entryToPage, month);
                        UADHelperFunctions.EntryLogswithURL(entryToPage, urlPages[i]);
                    }
                    else
                    {
                        UserAnalysisDashboard.ReadLogs(exitFromPage);
                        UADHelperFunctions.LogsFortheMonth(exitFromPage, month);
                        UADHelperFunctions.ExitLogswithURL(exitFromPage, urlPages[i]);
                    }

                }
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
                double average = UADHelperFunctions.FindAverage(sessions);
                averageTimeSpent[i] = average;
            }
            UADHelperFunctions.Quick_SortD(averageTimeSpent, urlPages, 0, urlPages.Count - 1);

            string pageUsed = "Top 5 average page sessions (in minutes)" +
                              "\n" + urlPages[5] + " average time spent: " + averageTimeSpent[5] +
                              "\n" + urlPages[4] + " average time spent: " + averageTimeSpent[4] +
                              "\n" + urlPages[3] + " average time spent: " + averageTimeSpent[3] +
                              "\n" + urlPages[2] + " average time spent: " + averageTimeSpent[2] +
                              "\n" + urlPages[1] + " average time spent: " + averageTimeSpent[1];
            Console.WriteLine(pageUsed);
            return pageUsed;
        }

        /// <summary>
        /// Calculates the 5 most used features of the website
        /// </summary>
        public static string Top5MostUsedFeature(string month)
        {
            List<GNGLog> loglist = new List<GNGLog>();
            UserAnalysisDashboard.ReadLogs(loglist);
            UADHelperFunctions.LogsFortheMonth(loglist, month);
            List<String> features = new List<String>() { "EventCreated", "EventJoined", "SearchForUser", "FindEventForMe", "UserRatings", "ViewHistory" };
            List<int> timesFeaturedUsed = new List<int>() {0, 0, 0, 0, 0, 0};
            Dictionary<string, int> listOfIDs = LogIDGenerator.GetLogIDs();
            for (int i = 0; i < features.Count; i++ )
            {
                listOfIDs.TryGetValue(features[i], out int clickLogID);
                string logID = clickLogID.ToString();
                int timesUsed = UADHelperFunctions.NumberofLogs(loglist, logID);
                timesFeaturedUsed[i] = timesUsed;
            }
            UADHelperFunctions.Quick_Sort(timesFeaturedUsed, features, 0, features.Count - 1);

            string mostUsed;
            mostUsed = "Top 5 most used features" +
                              "\n" + features[5] + " times used: " + timesFeaturedUsed[5] +
                              "\n" + features[4] + " times used: " + timesFeaturedUsed[4] +
                              "\n" + features[3] + " times used: " + timesFeaturedUsed[3] +
                              "\n" + features[2] + " times used: " + timesFeaturedUsed[2] +
                              "\n" + features[1] + " times used: " + timesFeaturedUsed[1];
            Console.WriteLine(mostUsed);
            return mostUsed;

        }
        /// <summary>
        /// Calcualtes the average user session per month for six months
        /// </summary>
        public static string AverageSessionMonthly(string month)
        {
            List<string> months = new List<string>() { "March", "April", "May", "June", "July", "August" };
            List<string> averages = new List<string>() {};
            for (int index = 0; index < months.Count; index++)
            {
                List<string> logID = new List<string>() { "1004", "1005" };
                List<GNGLog> entryToWebsite = new List<GNGLog>();
                List<GNGLog> exitFromWebsite = new List<GNGLog>();
                List<GNGLog> sessions = new List<GNGLog>();

                for (int i = 0; i < logID.Count; i++)
                {

                    if (i == 0)
                    {
                        UserAnalysisDashboard.ReadLogs(entryToWebsite);
                        UADHelperFunctions.LogsFortheMonth(entryToWebsite, months[index]);
                        UADHelperFunctions.LogswithID(entryToWebsite, logID[i]);
                    }
                    else
                    {
                        UserAnalysisDashboard.ReadLogs(exitFromWebsite);
                        UADHelperFunctions.LogsFortheMonth(exitFromWebsite, months[index]);
                        UADHelperFunctions.LogswithID(exitFromWebsite, logID[i]);
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
                double average = UADHelperFunctions.FindAverage(sessions);
                string sAverage = String.Format("{0:0.00}", average);
                averages.Add(sAverage);

            }
            string monthlyAverages = "Average session duration per month (in minutes)" +
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
        /// Calculates the number of times a user logged in for six months
        /// </summary>
        public static string LoggedInMonthly()
        {
            List<string> months = new List<string>() { "March", "April", "May", "June", "July", "August" };
            List<int> logins = new List<int>() {};
            for(int index = 0; index < months.Count; index++)
            {
                List<GNGLog> loglist = new List<GNGLog>();
                UserAnalysisDashboard.ReadLogs(loglist);
                UADHelperFunctions.LogsFortheMonth(loglist, months[index]);
                int loginCount = UADHelperFunctions.NumberofLogs(loglist, "1004");
                logins.Add(loginCount);
            }
            string monthlyLogin = "Total amount of users per month" +
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