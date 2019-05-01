using Gucci.DataAccessLayer.Models;
using System.Collections.Generic;

namespace Gucci.ServiceLayer.Interface
{
    public interface IUADService
    {
        int GetNumberofLogsID(List<GNGLog> logs, string logID);
        List<GNGLog> GetLogsFortheMonth(List<GNGLog> logs, string month);
        List<GNGLog> GetLogswithID(List<GNGLog> logs, string ID);
        double CalculateAverageSessionTime(List<GNGLog> session);
        void GetEntryLogswithURL(List<GNGLog> logs, string url);
        void GetExitLogswithURL(List<GNGLog> logs, string url);
        List<GNGLog> GetLogsForMonthAndYear(List<GNGLog> logs, string month, int year);
        List<GNGLog> PairStartAndEndLogs(List<GNGLog> startLogs, List<GNGLog> endLogs);
    }
}
