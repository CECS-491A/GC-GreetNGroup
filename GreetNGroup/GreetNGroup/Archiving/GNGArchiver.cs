using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;

namespace GreetNGroup.Archiving
{
    public class GNGArchiver
    {
        private const string LOGS_FOLDERPATH = "C:\\Users\\Yuki\\Documents\\GitHub\\GreetNGroup\\GreetNGroup\\GreetNGroup\\Logs\\";
        private const string ARCHIVES_FOLDERPATH = "C:\\Users\\Yuki\\Documents\\GitHub\\GreetNGroup\\GreetNGroup\\GreetNGroup\\Archives\\";
        private const int MAX_LOG_LIFETIME = 30;

        private string[] GetLogsFilename()
        {
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
            string[] filepathsOfLogs = Directory.GetFiles(LOGS_FOLDERPATH);

            return filepathsOfLogs;
        }

        private bool isLogOlderThan30Days(string fileName)
        {
            bool isOld = false;
            string[] splitDate = fileName.Split('_');
            string logDate = splitDate[0];
            DateTime dateOfLog = DateTime.Parse(logDate);
            if ((DateTime.Now - dateOfLog).TotalDays > MAX_LOG_LIFETIME)
            {
                isOld = true;
            }
            return isOld;
        }

        private List<string> GetOldLogs()
        {
            string[] listOfLogs = GetLogsFilename();
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

        public bool ArchiveOldLogs()
        {
            bool isSuccessfulArchive = false;
            bool isSuccessfulDeletion = false;
            List<string> logsToArchive = GetOldLogs();
            string archiveFileName = DateTime.Now.ToString("dd-MM-yyyy") + "_gngarchive.zip";
            try
            {
                using (ZipArchive archiver = ZipFile.Open(archiveFileName, ZipArchiveMode.Create))
                {
                    foreach (string logDate in logsToArchive)
                    {
                        string logPath = LOGS_FOLDERPATH + logDate + "_gnglog.json";
                        archiver.CreateEntryFromFile(logPath, logDate + "_gnglog.json");
                        isSuccessfulDeletion = RemoveLog(logPath);
                    }
                    if (isSuccessfulDeletion == true)
                    {
                        archiver.Dispose();
                        isSuccessfulArchive = true;
                    }
                    
                }
 
            }catch(FileNotFoundException e)
            {
                isSuccessfulArchive = false;
            }

            return isSuccessfulArchive;
        }

        private bool RemoveLog(string logPath)
        {
            bool isSuccessfulRemoval = false;
            try
            {
                File.Delete(logPath);
                isSuccessfulRemoval = true;
            }catch(FileNotFoundException e)
            {
                isSuccessfulRemoval = false;
            }

            return isSuccessfulRemoval;
        }
            
    }
}