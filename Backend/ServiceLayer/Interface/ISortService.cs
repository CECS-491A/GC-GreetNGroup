using System;
using System.Collections.Generic;

namespace Gucci.ServiceLayer.Interface
{
    public interface ISortService
    {
        void QuickSortInteger(List<int> usedCounts, List<string> logID, int left, int right);
        int PartitionInteger(List<int> usedCounts, List<string> logID, int left, int right);
        void QuickSortDouble<T>(List<T> sessiontimes, List<string> urls) where T : IComparable<T>;
        void PartitionDouble<T>(List<T> sessiontimes, List<string> urls, int left, int right) where T : IComparable<T>;
    }
}
