using ServiceLayer.Services;
using System;
using System.IO;
using System.IO.Compression;
using ServiceLayer.Interface;
using ServiceLayer.Model;

namespace ManagerLayer.ArchiverManager
{
    public class GNGArchiverManager
    {
        private IGNGArchiverService _gngArchiverService;
        private IErrorHandlerService _errorHandlerService;
        private Configurations configurations;

        public GNGArchiverManager()
        {
            _gngArchiverService = new GNGArchiverService();
            _errorHandlerService = new ErrorHandlerService();
            configurations = new Configurations();
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
            var isSuccessfulArchive = false;
            var isSuccessfulDeletion = false;
            var logsToArchive = _gngArchiverService.GetOldLogs();
            var archiveFileName = DateTime.Now.ToString(configurations.GetDateTimeFormat()) + 
                configurations.GetArchiveExtention();

            if(logsToArchive.Count == 0)
            {
                isSuccessfulArchive = false;
                return isSuccessfulArchive;
            }
            try
            {
                /* Using filestream to allow fileshare to alleviate potential
                * denial of access due to multiple calls to archive method
                */
                using (var fstream = new FileStream((configurations.GetArchivesDirectory() + archiveFileName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (var archiver = new ZipArchive(fstream, ZipArchiveMode.Update))
                    {
                        foreach (var logDate in logsToArchive)
                        {
                            var logPath = configurations.GetLogDirectory() + logDate;
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
                return isSuccessfulArchive;
            }
            /* Catch this error explicitly to see if archiver cannot find the .zip file
             * Let other errors bubble up
             */
            catch (FileNotFoundException e)
            {
                isSuccessfulArchive = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
                return isSuccessfulArchive;
            }
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
            var isSuccessfulRemoval = false;
            try
            {
                File.Delete(logPath);
                isSuccessfulRemoval = true;
            }
            /* Catch this error explicitly to see if archiver cannot find the log file
             * Let other errors bubble up
            */ 
            catch (FileNotFoundException e)
            {
                isSuccessfulRemoval = false;
                _errorHandlerService.IncrementErrorOccurrenceCount(e.ToString());
            }

            return isSuccessfulRemoval;
        }
    }
}
