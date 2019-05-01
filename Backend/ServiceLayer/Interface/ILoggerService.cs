using Gucci.DataAccessLayer.Models;
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
        bool LogBadRequest(string usersID, string ip, string url, string exception);
        bool LogErrorsEncountered(string usersID, string errorCode, string urlOfErr, string errDesc, string ip);
        bool LogGNGSearchAction(string usersID, string searchedItem, string ip);
        bool WriteGNGLogToFile(List<GNGLog> logList);
        string GetCurrentLogPath();
    }
}
