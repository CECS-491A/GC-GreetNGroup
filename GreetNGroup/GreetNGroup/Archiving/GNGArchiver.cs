using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Globalization;

namespace GreetNGroup.Archiving
{
    public class GNGArchiver
    {
        private static string LOGS_FOLDERPATH = Path.Combine(
             Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
             @"GreetNGroup\GreetNGroup\Logs\");
        private static string ARCHIVES_FOLDERPATH = Path.Combine(
             Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
             @"GreetNGroup\GreetNGroup\Archives\");
        private const int MAX_LOG_LIFETIME = 30;
        private static int errorCounter = 0;

        private static List<string> GetLogsFilename()
        {
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            string[] filepathsOfLogs = Directory.GetFiles(LOGS_FOLDERPATH);
            List<string> listOfLogs = new List<string>();
            foreach (string filepath in filepathsOfLogs)
            {
                listOfLogs.Add(Path.GetFileName(filepath));
            }
            return listOfLogs;
        }

        private static bool isLogOlderThan30Days(string fileName)
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

        private static List<string> GetOldLogs()
        {
            List<string> listOfLogs = GetLogsFilename();
            List<string> listOfOldLogs = new List<string>();
            foreach(string logFilename in listOfLogs)
            {
                if(isLogOlderThan30Days(logFilename) == true)
                {
                    listOfOldLogs.Add(logFilename);
                }
            }

            return listOfOldLogs;
        }

        public static bool ArchiveOldLogs()
        {
            bool isSuccessfulArchive = false;
            bool isSuccessfulDeletion = false;
            List<string> logsToArchive = GetOldLogs();
            string archiveFileName = DateTime.Now.ToString("dd-MM-yyyy") + "_gngarchive.zip";
            try
            {
                using (FileStream fstream = new FileStream((ARCHIVES_FOLDERPATH + archiveFileName), FileMode.OpenOrCreate))
                {
                    using (ZipArchive archiver = new ZipArchive(fstream, ZipArchiveMode.Update))
                    {
                        foreach (string logDate in logsToArchive)
                        {
                            string logPath = LOGS_FOLDERPATH + logDate;
                            ZipArchiveEntry logEntry = archiver.CreateEntryFromFile(logPath, logDate);
                            
                            isSuccessfulDeletion = RemoveLog(logPath);
                        }
                        if (isSuccessfulDeletion == true)
                        {
                            archiver.Dispose();
                            isSuccessfulArchive = true;
                        }

                    }
                }

            }catch(FileNotFoundException e)
            {
                isSuccessfulArchive = false;
                errorCounter++;
            }

            return isSuccessfulArchive;
        }

        private static bool RemoveLog(string logPath)
        {
            bool isSuccessfulRemoval = false;
            try
            {
                File.Delete(logPath);
                isSuccessfulRemoval = true;
            }catch(FileNotFoundException e)
            {
                isSuccessfulRemoval = false;
                errorCounter++;
            }

            return isSuccessfulRemoval;
        }

        public static void errorHandler()
        {
            if (errorCounter >= 100)
            {
                //Contact system admin
                errorCounter = 0;
            }
        }

    }
}