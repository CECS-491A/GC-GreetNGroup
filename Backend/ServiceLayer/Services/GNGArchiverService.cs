using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace ServiceLayer.Services
{
    public class GNGArchiverService : IGNGArchiverService
    {
        IGNGLoggerService _gngLoggerService = new GNGLoggerService();

        private const int MAX_LOG_LIFETIME = 30;
        private readonly string ARCHIVES_FOLDERPATH = Path.Combine(
             Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
             @"GreetNGroup\GreetNGroup\Archives\");

        /// <summary>
        /// Method GetLogsFilename retrieves the log file names inside the Logs folder
        /// </summary>
        /// <returns>List of log file names as strings</returns>
        public List<string> GetLogsFilename()
        {
            string logFolderPath = _gngLoggerService.GetLogsFolderpath();
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            string[] filepathsOfLogs = Directory.GetFiles(logFolderPath);
            List<string> listOfLogs = new List<string>();
            foreach (string filepath in filepathsOfLogs)
            {
                listOfLogs.Add(Path.GetFileName(filepath));
            }
            return listOfLogs;
        }

        /// <summary>
        /// Method GetOldLogs retrieves log file names that are older than the maximum lifetime
        /// and puts them in a list.
        /// </summary>
        /// <returns>Returns list of old log file names as a string</returns>
        public List<string> GetOldLogs()
        {
            List<string> listOfLogs = GetLogsFilename();
            List<string> listOfOldLogs = new List<string>();
            foreach (string logFilename in listOfLogs)
            {
                if (IsLogOlderThan30Days(logFilename) == true)
                {
                    listOfOldLogs.Add(logFilename);
                }
            }

            return listOfOldLogs;
        }

        /// <summary>
        /// Method IsLogOlderThan30Days checks if the log is older than 30 days to determine
        /// if the log should be archived or not
        /// </summary>
        /// <param name="fileName">log being checked</param>
        /// <returns>True or false depending if the log is older than 30 days</returns>
        public bool IsLogOlderThan30Days(string fileName)
        {
            bool isOld = false;
            string[] splitDate = fileName.Split('_');
            string logDate = splitDate[0];
            DateTime.TryParseExact(logDate, "dd-MM-yyyy", new CultureInfo("en-US"),
                DateTimeStyles.None, out DateTime dateOfLog);
            if ((DateTime.Now - dateOfLog).TotalDays > MAX_LOG_LIFETIME)
            {
                isOld = true;
            }
            return isOld;
        }

        public string GetArchiveFolderpath()
        {
            return ARCHIVES_FOLDERPATH;
        }
    }
}
