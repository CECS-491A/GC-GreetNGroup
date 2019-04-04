using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace ServiceLayer.Services
{
    public interface IUADService
    {
        int NumberofLogs(List<GNGLog> logs, string logID);
        List<GNGLog> LogsFortheMonth(List<GNGLog> logs, string month);
        List<GNGLog> LogswithID(List<GNGLog> logs, string ID);
        void Quick_Sort(List<int> usedCounts, List<string> logID, int left, int right);
        int Partition(List<int> usedCounts, List<string> logID, int left, int right);
        void QuickSortD<T>(T[] data, List<string> urls) where T : IComparable<T>;
        void Quick_SortD<T>(T[] data, List<string> urls, int left, int right) where T : IComparable<T>;
        double FindAverage(List<GNGLog> session);
        void FindEntryLogswithURL(List<GNGLog> logs, string url);
        void FindExitLogswithURL(List<GNGLog> logs, string url);
    }
}
