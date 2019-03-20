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
        private static string LOGS_FOLDERPATH = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
            @"GreetNGroup\GreetNGroup\Logs\");
        private static DirectoryInfo di = new DirectoryInfo(LOGS_FOLDERPATH);
        private static string[] dirs = Directory.GetFiles(LOGS_FOLDERPATH, "*.json");
        private static Dictionary<string, int> listOfIDs = LogIDGenerator.GetLogIDs();

        public UserAnalysisDashboard()
        {

        }
        /// <summary>
        /// Reads all json files in directory and deserializes into GNGlog and puts them all into a Arraylist
        /// </summary>
        /// <returns></returns>
        private static void ReadLogs(List<GNGLog> loglist)
        {
           
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
            int loginCount = 0;
            int registered = 0;
            List<GNGLog> loglist = new List<GNGLog>();
            UserAnalysisDashboard.ReadLogs(loglist);
            UADHelperFunctions.LogsFortheMonth(loglist, month);
            loginCount = UADHelperFunctions.NumberofLogs(loglist, "1004");

            registered = DbRetrieve.GetUsersRegistered();

            string failedVsSuccess = "Logins: " + loginCount +
                                    "\nRegistered Accounst: " + registered;

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

            string averageSession = "Average Session Time: " + sAverage + " minutes";

            Console.WriteLine(averageSession);

            

            return averageSession;


        }
        /// <summary>
        /// 
        /// </summary>
        public static void Top5AveragePageSession(string month)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public static string Top5MostUsedFeature(string month)
        {
            List<GNGLog> loglist = new List<GNGLog>();
            UserAnalysisDashboard.ReadLogs(loglist);
            UADHelperFunctions.LogsFortheMonth(loglist, month);
            List<String> features = new List<String>() { "EventCreated", "EventJoined", "SearchForUser", "FindEventForMe", "UserRatings", "ViewHistory" };
            List<int> timesFeaturedUsed = new List<int>() {0, 0, 0, 0, 0, 0};
            
            for (int i = 0; i < loglist.Count; i++ )
            {
                listOfIDs.TryGetValue(features[i], out int clickLogID);
                string logID = clickLogID.ToString();
                int timesUsed = UADHelperFunctions.NumberofLogs(loglist, logID);
                timesFeaturedUsed[i] = timesUsed;
            }
            UADHelperFunctions.Quick_Sort(timesFeaturedUsed, features, 0, features.Count - 1);
            string mostUsed = "1." + features[5] + " times used: " + timesFeaturedUsed[5] +
                              "\n2." + features[5] + " times used: " + timesFeaturedUsed[4] +
                              "\n3." + features[5] + " times used: " + timesFeaturedUsed[3] +
                              "\n4." + features[5] + " times used: " + timesFeaturedUsed[2] +
                              "\n5." + features[5] + " times used: " + timesFeaturedUsed[1];
            Console.WriteLine(mostUsed);
            return mostUsed;

        }
        /// <summary>
        /// 
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
            string monthlyAverages = "March: " + averages[0] +
                                  "\nApril: " + averages[1] +
                                  "\nMay: " + averages[2] +
                                  "\nJune: " + averages[3] +
                                  "\nJuly: " + averages[4] +
                                  "\nAugust: " + averages[5];

            Console.WriteLine(monthlyAverages);

            return monthlyAverages;
        }
        /// <summary>
        /// 
        /// </summary>
        public static string LoggedInMonthly()
        {
            List<string> months = new List<string>() { "March", "April", "May", "June", "July", "August" };
            List<int> logins = new List<int>() { 0, 0, 0, 0, 0, 0};
            for(int index = 0; index < months.Count; index++)
            {
                List<GNGLog> loglist = new List<GNGLog>();
                UserAnalysisDashboard.ReadLogs(loglist);
                UADHelperFunctions.LogsFortheMonth(loglist, months[index]);
                int loginCount = UADHelperFunctions.NumberofLogs(loglist, "1004");
                logins.Add(loginCount);
            }
            string monthlyLogin = "March: " + logins[0] +
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