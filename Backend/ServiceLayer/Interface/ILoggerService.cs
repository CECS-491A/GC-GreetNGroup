using DataAccessLayer.Models;
using System.Collections.Generic;

namespace Gucci.ServiceLayer.Interface
{
    public interface ILoggerService
    {
        bool CreateNewLog(string fileName, string directory);
        List<GNGLog> FillCurrentLogsList();
        List<GNGLog> ReadLogs();
        bool CheckForExistingLog(string fileName, string directory);
        bool LogGNGInternalErrors(string exception);
        Dictionary<string, int> GetLogIDs();
        bool WriteGNGLogToFile(List<GNGLog> logList);
        string GetCurrentLogPath();
    }
}
