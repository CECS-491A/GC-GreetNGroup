using Gucci.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using Gucci.DataAccessLayer.Models;

namespace ServiceLayer.Services
{
    public interface IUADService
    {
        int GetNumberofLogsID(List<GNGLog> logs, string logID);
        List<GNGLog> GetLogsFortheMonth(List<GNGLog> logs, string month);
        List<GNGLog> GetLogswithID(List<GNGLog> logs, string ID);
        double CalculateAverageSessionTime(List<GNGLog> session);
        void GetEntryLogswithURL(List<GNGLog> logs, string url);
        void GetExitLogswithURL(List<GNGLog> logs, string url);
    }
}
