using System.Collections.Generic;

namespace ServiceLayer.Services
{
    public interface IGNGArchiverService
    {
        List<string> GetLogsFilename();
        bool IsLogOlderThan30Days(string filename);
        List<string> GetOldLogs();
        string GetArchiveFolderpath();
    }
}
