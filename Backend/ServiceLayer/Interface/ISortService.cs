using System;
using System.Collections.Generic;

namespace ServiceLayer.Interface
{
    interface ISortService
    {
        void QuickSortInteger(List<int> usedCounts, List<string> logID, int left, int right);
        int PartitionInteger(List<int> usedCounts, List<string> logID, int left, int right);
        void QuickSortDouble<T>(List<T> data, List<string> urls) where T : IComparable<T>;
        void PartitionDouble<T>(List<T> data, List<string> urls, int left, int right) where T : IComparable<T>;
    }
}
