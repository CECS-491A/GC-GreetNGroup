using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace ServiceLayer.Services
{
    public interface IUADService
    {
        int GetNumberofLogs(List<GNGLog> logs, string logID);
        List<GNGLog> GetLogsFortheMonth(List<GNGLog> logs, string month);
        List<GNGLog> GetLogswithID(List<GNGLog> logs, string ID);
        void QuickSortInteger(List<int> usedCounts, List<string> logID, int left, int right);
        int PartitionInteger(List<int> usedCounts, List<string> logID, int left, int right);
        void QuickSortDouble<T>(T[] data, List<string> urls) where T : IComparable<T>;
        void PartitionDouble<T>(T[] data, List<string> urls, int left, int right) where T : IComparable<T>;
        double CalculateAverageSessionTime(List<GNGLog> session);
        void GetEntryLogswithURL(List<GNGLog> logs, string url);
        void GetExitLogswithURL(List<GNGLog> logs, string url);
    }
}
