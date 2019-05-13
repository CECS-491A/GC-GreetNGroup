using Gucci.DataAccessLayer.Models;
using System.Collections.Generic;

namespace Gucci.ServiceLayer.Interface
{
    public interface IUADService
    {
        int GetNumberofLogsID(List<GNGLog> logs, string logID);
        List<GNGLog> GetLogswithID(List<GNGLog> logs, string ID);
        List<GNGLog> GetEntryLogswithURL(List<GNGLog> logs, string url);
        List<GNGLog> GetExitLogswithURL(List<GNGLog> logs, string url);
        List<GNGLog> PairStartAndEndLogs(List<GNGLog> startLogs, List<GNGLog> endLogs);
    }
}
