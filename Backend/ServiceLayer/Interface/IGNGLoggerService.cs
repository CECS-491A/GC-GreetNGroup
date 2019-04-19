using DataAccessLayer.Models;
using System.Collections.Generic;

namespace ServiceLayer.Interface
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
        bool LogGNGInternalErrors(string exception);
        Dictionary<string, int> GetLogIDs();
        bool WriteGNGLogToFile(List<GNGLog> logList);
    }
}
