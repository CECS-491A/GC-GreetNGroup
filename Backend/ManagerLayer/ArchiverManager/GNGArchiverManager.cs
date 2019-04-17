using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ServiceLayer.Interface;

namespace ManagerLayer.ArchiverManager
{
    public class GNGArchiverManager
    {
        private string archivesFolderPath;

        IGNGArchiverService _gngArchiverService = new GNGArchiverService();
        IErrorHandlerService _errorHandlerService = new ErrorHandlerService();
        IGNGLoggerService _gngLoggerService = new GNGLoggerService();

        public GNGArchiverManager()
        {
            archivesFolderPath = _gngArchiverService.GetArchiveFolderpath();
        }

        /// <summary>
        /// Method ArchiveOldLogs puts the logs that are 30 days old or older inside
        /// a zip file for compression. The zip file will be located in the Archives folder.
        /// If an error occurs, the error counter is incremented and the error handler will
        /// contact the system admin if errors persist to 100 or more errors.
        /// </summary>
        /// <returns>Return true or false depending if the archiving process was successful</returns>
        public bool ArchiveOldLogs()
        {
            bool isSuccessfulArchive = false;
            bool isSuccessfulDeletion = false;
            List<string> logsToArchive = _gngArchiverService.GetOldLogs();
            string archiveFileName = DateTime.Now.ToString("dd-MM-yyyy") + "_gngarchive.zip";
            try
            {
                using (FileStream fstream = new FileStream((archivesFolderPath + archiveFileName), FileMode.OpenOrCreate))
                {
                    using (ZipArchive archiver = new ZipArchive(fstream, ZipArchiveMode.Update))
                    {
                        foreach (string logDate in logsToArchive)
                        {
                            string logPath = _gngLoggerService.GetLogsFolderpath() + logDate;
                            ZipArchiveEntry logEntry = archiver.CreateEntryFromFile(logPath, logDate);

                            isSuccessfulDeletion = RemoveLog(logPath);
                        }
                        if (isSuccessfulDeletion == true)
                        {
                            archiver.Dispose();
                            isSuccessfulArchive = true;
                        }

                    }
                    fstream.Close();
                }

            }
            catch (FileNotFoundException e)
            {
                isSuccessfulArchive = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }

            return isSuccessfulArchive;
        }

        /// <summary>
        /// Method removeLog removes the log from the logs folder once the log has been
        /// successfully archived into the zip file. If an error occurs, the error counter
        /// will increment and the error handler will contact the system admin if the errors
        /// persist to 100 or more occurrences.
        /// </summary>
        /// <param name="logPath">The log to be removed from the logs folder</param>
        /// <returns>Returns true or false depending on if the removal was successful</returns>
        private bool RemoveLog(string logPath)
        {
            bool isSuccessfulRemoval = false;
            try
            {
                File.Delete(logPath);
                isSuccessfulRemoval = true;
            }
            catch (FileNotFoundException e)
            {
                isSuccessfulRemoval = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }

            return isSuccessfulRemoval;
        }
    }
}
