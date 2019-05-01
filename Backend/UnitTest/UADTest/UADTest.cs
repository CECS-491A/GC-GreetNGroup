using Gucci.DataAccessLayer.Models;
using Gucci.ManagerLayer.UADManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Services;
using System;
using System.Collections.Generic;

namespace UnitTest.UADTest
{
    [TestClass]
    public class UADTest
    {
        private UADManager uadManager = new UADManager();
        private UADService _uadService = new UADService();
        private LoggerService _loggerService = new LoggerService();
        #region ManagerTest
        [TestMethod]
        public void GetLoginComparedToRegistered_Pass()
        {
            
            var test = uadManager.GetLoginComparedToRegistered("April", 2019);
            for(int i = 0; i < test.Count; i++)
            {
                Console.WriteLine(test[i].InfoType + ' ' + test[i].Value);
            }
            // Assert
            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void GetMonthlyLoginOverSixMonths_Pass()
        {
            var test = uadManager.GetLoggedInMonthly("April", 2019);
            for (int i = 0; i < test.Count; i++)
            {
                Console.WriteLine(test[i].InfoType + ' ' + test[i].Value);
            }
            // Assert
            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void GetTop5MostUsedFeature_Pass()
        {
            var test = uadManager.GetTop5MostUsedFeature("April", 2019);
            for (int i = 0; i < test.Count; i++)
            {
                Console.WriteLine(test[i].InfoType + ' ' + test[i].Value);
            }
            // Assert
            Assert.IsNotNull(test);

        }

        [TestMethod]
        public void GetAverageSessionDuration_Pass()
        {
            var test = uadManager.GetAverageSessionDuration("April", 2019);
            Console.WriteLine(test.InfoType + ' ' + test.Value);
            // Assert
            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void GetAverageSessionMonthly_Pass()
        {
            var test = uadManager.GetAverageSessionMonthly("April", 2019);
            for (int i = 0; i < test.Count; i++)
            {
                Console.WriteLine(test[i].InfoType + ' ' + test[i].Value);
            }
            // Assert
            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void GetTop5AveragePageSession_Pass()
        {
            var test = uadManager.GetTop5AveragePageSession("April", 2019);
            for (int i = 0; i < test.Count; i++)
            {
                Console.WriteLine(test[i].InfoType + ' ' + test[i].Value);
            }
            // Assert
            Assert.IsNotNull(test);
        }
        #endregion

        #region Passed Test
        [TestMethod]
        public void GetNumberofLogsID_Pass()
        {
            // Arrange
            bool expected = true;
            bool actual = false;
            string logID = "ClickEvent";
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs";
            List<GNGLog> logList = new List<GNGLog>();

            // Act
            logList = _loggerService.ReadLogsPath(path);
            int numOfLogs = _uadService.GetNumberofLogsID(logList, logID);
            if(numOfLogs == 4)
            {
                actual = true;
            }
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetLogsFortheMonth_Pass()
        {
            // Arrange
            bool expected = true;
            bool actual = false;
            string month = "March";
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs";
            List<GNGLog> logList = new List<GNGLog>();

            // Act
            logList = _loggerService.ReadLogsPath(path);
            logList = _uadService.GetLogsFortheMonth(logList, month);
            if (logList.Count == 3)
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetLogswithID_Pass()
        {
            // Arrange
            IUADService _uadService = new UADService();
            LoggerService _loggerService = new LoggerService();
            bool expected = true;
            bool actual = false;
            string logID = "ClickEvent";

            // Read testlogs from this path
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs";
            List<GNGLog> logList = new List<GNGLog>();

            // Act
            logList = _loggerService.ReadLogsPath(path);
            logList = _uadService.GetLogswithID(logList, logID);
            if (logList.Count == 4)
            {
                actual = true;
            }

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void CalculateAverageSessionTime_Pass()
        {
            // Arrange
            IUADService _uadService = new UADService();
            LoggerService _loggerService = new LoggerService();
            bool expected = true;
            bool actual = false;

            // Read logs from this path
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs_SessionTimes";
            List<GNGLog> logList = new List<GNGLog>();

            // Act
            logList = _loggerService.ReadLogsPath(path);
            double averageTime = _uadService.CalculateAverageSessionTime(logList);
            if(averageTime == 60)
            {
                actual = true;
            }
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetEntryLogswithURL_Pass()
        {
            // Arrange
            string entryLog = "https://www.endpoint.com";
            bool expected = true;
            bool actual = false;
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs";
            List<GNGLog> logList = new List<GNGLog>();

            // Act
            logList = _loggerService.ReadLogsPath(path);
            _uadService.GetEntryLogswithURL(logList, entryLog);
            if (logList.Count == 3)
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetExitLogswithURL_Pass()
        {
            // Arrange
            string entryLog = "https://www.startpoint.com";
            bool expected = true;
            bool actual = false;
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs";
            List<GNGLog> logList = new List<GNGLog>();

            // Act
            logList = _loggerService.ReadLogsPath(path);
            _uadService.GetExitLogswithURL(logList, entryLog);
            if (logList.Count == 4)
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }
        
        [TestMethod]
        public void GetNumberofLogsID_Pass_IDNotLogged()
        {
            // Arrange
            bool expected = true;
            bool actual = false;
            string logID = "1010";
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs";
            List<GNGLog> logList = new List<GNGLog>();

            // Act
            logList = _loggerService.ReadLogsPath(path);
            int numOfLogs = _uadService.GetNumberofLogsID(logList, logID);
            if (numOfLogs == 0)
            {
                actual = true;
            }
            // Assert
            Assert.AreNotSame(actual, expected);
        }

        [TestMethod]
        public void GetLogsFortheMonth_Pass_MonthNotLogged()
        {
            // Arrange
            bool expected = true;
            bool actual = false;
            string month = "December";
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs";
            List<GNGLog> logList = new List<GNGLog>();

            // Act
            logList = _loggerService.ReadLogsPath(path);
            logList = _uadService.GetLogsFortheMonth(logList, month);
            if (logList.Count == 0)
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetLogswithID_Pass_IDNotLogged()
        {
            //Arrange
            bool expected = true;
            bool actual = false;
            string logID = "1011";
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs";
            List<GNGLog> logList = new List<GNGLog>();

            //Act
            logList = _loggerService.ReadLogsPath(path);
            logList = _uadService.GetLogswithID(logList, logID);
            if (logList.Count == 0)
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetEntryLogswithURL_Pass_URLNotLogged()
        {
            //Arrange
            IUADService _uadService = new UADService();
            LoggerService _loggerService = new LoggerService();
            string entryLog = "https://www.test.com";
            bool expected = true;
            bool actual = false;
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs";
            List<GNGLog> logList = new List<GNGLog>();

            //Act
            logList = _loggerService.ReadLogsPath(path);
            _uadService.GetEntryLogswithURL(logList, entryLog);
            if (logList.Count == 0)
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetExitLogswithURL_Pass_URLNotLogged()
        {
            //Arrange
            string entryLog = "https://www.test.com";
            bool expected = true;
            bool actual = false;

            //Read logs from this path
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs";
            List<GNGLog> logList = new List<GNGLog>();

            //Act
            logList = _loggerService.ReadLogsPath(path);
            _uadService.GetExitLogswithURL(logList, entryLog);
            if (logList.Count == 0)
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        
        #endregion

        #region Failed Test
        [TestMethod]
        public void CalculateAverageSessionTime_Fail_LogsNotInRightOrder()
        {
            //Arrange
            bool expected = true;
            bool actual = false;
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\FailedLogs";
            List<GNGLog> logList = new List<GNGLog>();

            //Act
            logList = _loggerService.ReadLogsPath(path);
            double averageTime = _uadService.CalculateAverageSessionTime(logList);
            Console.WriteLine(averageTime);
            if (averageTime == 60)
            {
                actual = true;
            }
            // Assert
            Assert.AreNotEqual(actual, expected);
        }
        #endregion
    }
}
