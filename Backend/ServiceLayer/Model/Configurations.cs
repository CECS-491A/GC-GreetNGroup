using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Model
{
    public class Configurations
    {
        private const string DATE_TIME_FORMAT = "yyyy_MM_dd";
        private const int MAX_LOG_LIFETIME = 30;
        private const string CULTURE_INFO = "en-US";
        private const string ARCHIVE_EXTENTION = "_gngarchive.zip";
        private const string LOG_EXTENTION = "_gnglog.json";
        private const string LOG_DIRECTORY = @"C:\Users\Yuki\Documents\GitHub\GreetNGroup\Backend\Logs\";
        private const string ARCHIVES_DIRECTORY = @"C:\Users\Yuki\Documents\GitHub\GreetNGroup\Backend\Archives\";

        public string GetDateTimeFormat()
        {
            return DATE_TIME_FORMAT;
        }

        public int GetMaxLogLifetime()
        {
            return MAX_LOG_LIFETIME;
        }

        public string GetCultureInfo()
        {
            return CULTURE_INFO;
        }

        public string GetArchiveExtention()
        {
            return ARCHIVE_EXTENTION;
        }

        public string GetLogExtention()
        {
            return LOG_EXTENTION;
        }

        public string GetLogDirectory()
        {
            return LOG_DIRECTORY;
        }

        public string GetArchivesDirectory()
        {
            return ARCHIVES_DIRECTORY;
        }
    }
}
