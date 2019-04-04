using DataAccessLayer.Models;
using System.Collections.Generic;

namespace ServiceLayer.Services
{
    public interface IGNGLoggerService
    {
        string CreateNewLog();
        List<GNGLog> FillCurrentLogsList();
        List<GNGLog> ReadLogs();
        bool CheckForExistingLog();
        string GetLogsFolderpath();
        string GetLogsExtentionName();
        string GetCurrentLogPath();
        Dictionary<string, int> GetLogIDs();
    }
}
