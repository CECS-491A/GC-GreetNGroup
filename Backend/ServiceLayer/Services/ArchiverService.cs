using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Model;

namespace Gucci.ServiceLayer.Services
{
    public class ArchiverService : IArchiverService
    {
        private ILoggerService _gngLoggerService;
        private Configurations configurations;

        public ArchiverService()
        {
            _gngLoggerService = new LoggerService();
            configurations = new Configurations();
        }

        /// <summary>
        /// Method GetLogsFilename retrieves the log file names inside the Logs folder
        /// </summary>
        /// <returns>List of log file names as strings</returns>
        public List<string> GetLogsFilename()
        {
            //Format current day like this to prevent errors with using forward slashes
            var currentDate = DateTime.Now.ToString(configurations.GetDateTimeFormat());
            var filepathsOfLogs = Directory.GetFiles(configurations.GetLogDirectory());
            var listOfLogs = new List<string>();
            foreach (var filepath in filepathsOfLogs)
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
            //TryParseExact to properly retrieve the date that is the name of the log
            DateTime.TryParseExact(logDate, configurations.GetDateTimeFormat(), 
                new CultureInfo(configurations.GetCultureInfo()), DateTimeStyles.None, out DateTime dateOfLog);
            if ((DateTime.Now - dateOfLog).TotalDays > configurations.GetMaxLogLifetime())
            {
                isOld = true;
            }
            return isOld;
        }

    }
}
