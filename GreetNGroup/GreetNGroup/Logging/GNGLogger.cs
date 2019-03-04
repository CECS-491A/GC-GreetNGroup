using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace GreetNGroup.Logging
{
    public class GNGLogger
    {
        private const string LOGS_FOLDERPATH = "../GreetNGroup/Logs/";
        public GNGLogger()
        {

        }

        public void CreateNewLog()
        {
            bool logExists = CheckForExistingLog();
            if(logExists == false)
            {
                string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
                try
                {
                    File.Create(LOGS_FOLDERPATH + currentDate + "_gnglog.json");
                }
                catch (IOException e)
                {
                    
                }
                
            }
        }

        public bool CheckForExistingLog()
        {
            string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
            bool logExists = File.Exists(LOGS_FOLDERPATH + currentDate + "_gnglog.json");

            return logExists;
        }

        [HttpPost]
        public void LogClicksMade(string startPoint, string endpoint)
        {
            
        }

        [HttpPost]
        public void LogErrorsEncountered()
        {

        }

        [HttpPost]
        public void LogGNGEventsCreated()
        {

        }

        [HttpPost]
        public void LogEntryToWebsite()
        {

        }

        [HttpPost]
        public void LogExitFromWebsite()
        {

        }

        [HttpPost]
        public void LogAccountDeletion()
        {

        }

    }
}