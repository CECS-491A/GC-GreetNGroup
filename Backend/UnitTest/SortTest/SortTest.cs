using System;
using System.Collections.Generic;
using Gucci.ServiceLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.SortTest
{
    [TestClass]
    public class SortTest
    {
        SortService sortServicer = new SortService();
        [TestMethod]
        public void QuickSortInteger_Pass()
        {
            //Arrange
            var _uadService = new UADService();
            bool expected = true;
            bool actual = false;
            List<int> numList = new List<int> { 5, 4, 3, 2, 1 };
            List<string> wordList = new List<string> { "E", "D", "C", "B", "A" };

            //Act
            sortServicer.QuickSortInteger(numList, wordList, 0, wordList.Count - 1);
            if ((numList[0] == 1 && wordList[0].Equals("A")) && (numList[1] == 2 && wordList[1].Equals("B")) && (numList[2] == 3 && wordList[2].Equals("C")) && (numList[3] == 4 && wordList[3].Equals("D")) && (numList[4] == 5 && wordList[4].Equals("E")))
            {
                actual = true;
            }
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void QuickSortInteger_Pass_DuplicateNumbers()
        {
            // Arrange
            var _uadService = new UADService();
            bool expected = true;
            bool actual = false;
            List<int> numList = new List<int> { 2, 3, 5, 3, 1 };
            List<string> wordList = new List<string> { "E", "D", "C", "B", "A" };

            // Act
            sortServicer.QuickSortInteger(numList, wordList, 0, wordList.Count - 1);
            if ((numList[0] == 1 && wordList[0].Equals("A")) && (numList[1] == 2 && wordList[1].Equals("E")) && (numList[2] == 3 && wordList[2].Equals("B")) && (numList[3] == 3 && wordList[3].Equals("D")) && (numList[4] == 5 && wordList[4].Equals("C")))
            {
                actual = true;
            }
            Assert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void QuickSortDouble_Pass()
        {
            // Arrange
            var _uadService = new UADService();
            bool expected = true;
            bool actual = false;
            List<double> numList = new List<double> { 5.5, 4.4, 3.3, 2.2, 1.1 };
            string[] wordList = { "E", "D", "C", "B", "A" };

            // Act
            sortServicer.QuickSortDouble(numList, wordList);
            for(int i = 0; i < numList.Count; i++)
            {
                Console.WriteLine(numList[i]);
                Console.WriteLine(wordList[i]);
            }
            if ((numList[0] == 1.1 && wordList[0].Equals("A")) && (numList[1] == 2.2 && wordList[1].Equals("B")) && (numList[2] == 3.3 && wordList[2].Equals("C")) && (numList[3] == 4.4 && wordList[3].Equals("D")) && (numList[4] == 5.5 && wordList[4].Equals("E")))
            {
                actual = true;
            }
            Assert.AreEqual(actual, expected);
        }
        [TestMethod]
        public void QuickSortInteger_Fail_LessWordsThanNumbers()
        {
            //Arrange
            var _uadService = new UADService();
            bool expected = true;
            bool actual = false;
            List<int> numList = new List<int> { 5, 4, 3, 2, 1 };
            List<string> wordList = new List<string> { "E", "D", "C", "B" };

            //Act
            try
            {
                sortServicer.QuickSortInteger(numList, wordList, 0, wordList.Count - 1);
                if (wordList[0].Equals("A") == true && wordList[1].Equals("B") == true && wordList[2].Equals("C") == true && wordList[3].Equals("D") == true && wordList[4].Equals("E") == true)
                {
                    actual = true;
                }
            }
            catch (Exception all)
            {
                actual = true;
            }

            Assert.AreNotEqual(actual, expected);
        }

        [TestMethod]
        public void QuickSortInteger_Fail_LessNumbersThanWords()
        {
            //Arrange
            var _uadService = new UADService();
            bool expected = true;
            bool actual = false;
            List<int> numList = new List<int> { 5, 4, 3, 2 };
            List<string> wordList = new List<string> { "E", "D", "C", "B", "A" };

            //Act
            try
            {
                sortServicer.QuickSortInteger(numList, wordList, 0, wordList.Count - 1);
                if (wordList[0].Equals("A") == true && wordList[1].Equals("B") == true && wordList[2].Equals("C") == true && wordList[3].Equals("D") == true && wordList[4].Equals("E") == true)
                {
                    actual = true;
                }
            }
            catch (Exception all)
            {
                actual = false ;
            }
            Assert.AreNotEqual(actual, expected);
        }

        [TestMethod]
        public void QuickSortInteger_Fail_MoreWordsThanNumbers()
        {
            // Arrange
            var _uadService = new UADService();
            bool expected = true;
            bool actual = false;
            List<int> numList = new List<int> { 5, 4, 3, 2, 1 };
            List<string> wordList = new List<string> { "E", "D", "C", "B", "A", "Z" };

            // Act
            try
            {
                sortServicer.QuickSortInteger(numList, wordList, 0, wordList.Count - 1);
                if (wordList[0].Equals("A") == true && wordList[1].Equals("B") == true && wordList[2].Equals("C") == true && wordList[3].Equals("D") == true && wordList[4].Equals("E") == true)
                {
                    actual = true;
                }
            }
            catch (Exception all)
            {
                actual = false;
            }

            Assert.AreNotEqual(actual, expected);
        }

    }
}
