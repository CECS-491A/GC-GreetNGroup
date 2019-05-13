using Gucci.ServiceLayer.Interface;
using System;
using System.Collections.Generic;

namespace Gucci.ServiceLayer.Services
{
    public class SortService : ISortService
    {
        /// <summary>
        /// Quicksort function that recursively calls iteslf to order the list of integers and strings
        /// </summary>
        /// <param name="usedCounts">List of number of uses</param>
        /// <param name="logID">List of function ids</param>
        /// <param name="left">Left position</param>
        /// <param name="right">Right position</param>
        public void QuickSortInteger(List<int> usedCounts, List<string> logID, int left, int right)
        {
            if (left < right)
            {
                int pivot = PartitionInteger(usedCounts, logID, left, right);

                if (pivot > 1)
                {
                    QuickSortInteger(usedCounts, logID, left, pivot - 1);
                }
                if (pivot + 1 < right)
                {
                    QuickSortInteger(usedCounts, logID, pivot + 1, right);
                }
            }
        }

        /// <summary>
        /// Functions that partitions the list for quicksort and orders the list correctly
        /// </summary>
        /// <param name="usedCounts">List of integers representing the number of times a function was used</param>
        /// <param name="logID">List of log ids</param>
        /// <param name="left">Left index</param>
        /// <param name="right">right index</param>
        /// <returns>Next index to be used</returns>
        public int PartitionInteger(List<int> usedCounts, List<string> logID, int left, int right)
        {
            int pivot = usedCounts[left];
            while (true)
            {
                while (usedCounts[left] < pivot)
                {
                    left++;
                }
                while (usedCounts[right] > pivot)
                {
                    right--;
                }
                if (left < right)
                {
                    int temp = usedCounts[left];
                    usedCounts[left] = usedCounts[right];
                    usedCounts[right] = temp;

                    string tempID = logID[left];
                    logID[left] = logID[right];
                    logID[right] = tempID;

                    if (usedCounts[left] == usedCounts[right])
                    {
                        left++;
                    }
                }
                else
                {
                    return right;
                }
            }
        }

        /// <summary>
        /// Quicksort functions that sorts the average sessions times per page
        /// </summary>
        /// <param name="sessiontimes">array of session times</param>
        /// <param name="urls">list of urls</param>
        public void QuickSortDouble<T>(List<T> sessiontimes, List<string> urls) where T : IComparable<T>
        {
            PartitionDouble(sessiontimes, urls, 0, sessiontimes.Count - 1);
        }

        /// <summary>
        /// Function that partitions the list of doubles and strings and orders it from least to greatest
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessiontimes">List of session times</param>
        /// <param name="urls">list of urls</param>
        /// <param name="left">left index of array</param>
        /// <param name="right">right index of array</param>
        public void PartitionDouble<T>(List<T> sessiontimes, List<string> urls, int left, int right) where T : IComparable<T>
        {
            int i, j;
            T pivot, temp;
            i = left;
            j = right;
            pivot = sessiontimes[(left + right) / 2];

            do
            {
                while ((sessiontimes[i].CompareTo(pivot) < 0))
                {
                    i++;
                }
                while ((pivot.CompareTo(sessiontimes[j]) < 0))
                {
                    j--;
                }
                if (i <= j)
                {
                    temp = sessiontimes[i];
                    sessiontimes[i] = sessiontimes[j];
                    sessiontimes[j] = temp;

                    string tempUrl = urls[i];
                    urls[i] = urls[j];
                    urls[j] = tempUrl;

                    i++;
                    j--;
                }
            } while (i <= j);

            if (left < j)
            {
                PartitionDouble(sessiontimes, urls, left, j);
            }

            if (i < right)
            {
                PartitionDouble(sessiontimes, urls, i, right);
            }
        }
    }
}
