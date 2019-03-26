using System.Collections.Generic;

namespace ServiceLayer.Services
{
    public interface IGNGLoggerService
    {
        void CreateNewLog();
        bool CheckForExistingLog();
        string GetLogsFolderpath();
        string GetLogsExtentionName();
        string GetCurrentLogPath();
        Dictionary<string, int> GetLogIDs();
    }
}
