using Gucci.DataAccessLayer.Models;
using Gucci.ManagerLayer.UADManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gucci.ServiceLayer.Interface;
using Gucci.ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.UADTest
{
    [TestClass]
    public class UADTest
    {
        private UADManager uadManager = new UADManager();
        private UADService _uadService = new UADService();
        private LoggerService _loggerService = new LoggerService();
        private UserService userService = new UserService();
        #region ManagerTest
        [TestMethod]
        public void GetLoginComparedToRegistered_Pass()
        {
            // Arrange
            bool expected = true;
            bool actual = false;
            var expectedAverageLogins = "1.5";
            var expectedMinLogin = "1";
            var expectedMaxLogin = "2";
            var expectedRegistered = userService.GetRegisteredUserCount();
            // Act
            var test = uadManager.GetLoginComparedToRegistered("April", 1);
            if (test[0].Value.Equals(expectedAverageLogins) && test[1].Value.Equals(expectedMinLogin) && test[2].Value.Equals(expectedMaxLogin))
            {
                actual = true;
            }
            for(int i = 0; i < test.Count; i++)
            {
                Console.WriteLine(test[i].Date + ' ' + test[i].InfoType + ' ' + test[i].Value);
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetLoginSuccessFail_Pass()
        {
            // Arrange
            bool expected = true;
            bool actual = false;
            var expectedSuccessfulLogins = "3";
            var expectedFailLogins = "1";
            // Act
            var test = uadManager.GetLoginSuccessFail("June", 1);
            if (test[4].Value.CompareTo(expectedSuccessfulLogins) == 0 && test[5].Value.CompareTo(expectedFailLogins) == 0)
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetLoggedinMonthly_Pass()
        {
            // Arrange
            bool expected = true;
            bool actual = false;
            var expectedDates = new List<string> { "June 1", "May 1", "April 1", "March 1", "February 1", "January 1" };
            var expectedLogins = new List<string> { "0", "0", "3", "2", "1", "0" };
            var actualDates = new List<string>();
            var actualLogin = new List<string>();
            // Act
            var test = uadManager.GetLoggedInMonthly("June", 1);
            for (int i = 0; i < test.Count; i++)
            {
                actualDates.Add(test[i].Date);
                actualLogin.Add(test[i].Value);
            }
            if(expectedDates.SequenceEqual(actualDates) && expectedLogins.SequenceEqual(actualLogin))
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetTop5MostUsedFeature_Pass()
        {
            // Arrange
            bool expected = true;
            bool actual = false;
            var expectedFeatures = new List<string> {"SearchAction", "EventCreated", "EventJoined", "UserRatings", "FindEventForMe", };
            var expectedCount = new List<string> {"11", "3",  "1", "0", "0" };
            var actualFeatures = new List<string>();
            var actualCount = new List<string>();
            // Act
            var test = uadManager.GetTop5MostUsedFeature("April", 1);
            for (int i = 0; i < test.Count; i++)
            {
                Console.WriteLine(test[i].Date + ' ' + test[i].InfoType + ' ' + test[i].Value);
                actualFeatures.Add(test[i].InfoType);
                actualCount.Add(test[i].Value);
            }
            if (expectedFeatures.SequenceEqual(actualFeatures) && expectedCount.SequenceEqual(actualCount))
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);

        }

        [TestMethod]
        public void GetAverageSessionDuration_Pass()
        {
            // Arrange
            bool expected = true;
            bool actual = false;
            var expectedAverageSession = "180";
            var expectedMinSession = "60";
            var expectedMaxSession = "300";

            // Act
            var test = uadManager.GetAverageSessionDuration("June", 1);
            if(expectedAverageSession == test[6].Value && expectedMinSession == test[7].Value && expectedMaxSession == test[8].Value)
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetAverageSessionMonthly_Pass()
        {
            // Assign
            bool expected = true;
            bool actual = false;
            var expectedDates = new List<string> { "June 1", "May 1", "April 1", "March 1", "February 1", "January 1" };
            var expectedDuration = new List<string> { "0", "0", "180", "956.73", "533.47", "0" };
            var actualDuration = new List<string>();
            var actualMonths = new List<string>();
            var actualDates = new List<string>();
            // Act
            var test = uadManager.GetAverageSessionMonthly("June", 1);
            for (int i = 0; i < test.Count; i++)
            {
                actualDates.Add(test[i].Date);
                actualDuration.Add(test[i].Value);
            }
            if (expectedDuration.SequenceEqual(actualDuration) && expectedDates.SequenceEqual(actualDates))
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetTop5AveragePageSession_Pass()
        {
            // Assign
            bool expected = true;
            bool actual = false;
            var expectedUrls = new List<string> { "https://www.greetngroup.com/search", "https://www.greetngroup.com/createevent", "https://www.greetngroup.com", "https://www.greetngroup.com/faq", "https://www.greetngroup.com/help" };
            var expectedDuration = new List<string> { "150", "60", "60", "0", "0" };
            var actualUrls = new List<string>();
            var actualDuration = new List<string>();

            // Act
            var test = uadManager.GetTop5AveragePageSession("June", 1);
            for (int i = 0; i < test.Count; i++)
            {
                actualUrls.Add(test[i].InfoType);
                actualDuration.Add(test[i].Value);
            }
            if (actualUrls[10].CompareTo(expectedUrls[0]) == 0 && actualUrls[11].CompareTo(expectedUrls[1]) == 0 && actualUrls[12].CompareTo(expectedUrls[2]) == 0 && actualUrls[13].CompareTo(expectedUrls[3]) == 0 && actualUrls[14].CompareTo(expectedUrls[4]) == 0 &&
                actualDuration[10].CompareTo(expectedDuration[0]) == 0 && actualDuration[11].CompareTo(expectedDuration[1]) == 0 && actualDuration[12].CompareTo(expectedDuration[2]) == 0 && actualDuration[13].CompareTo(expectedDuration[3]) == 0 && actualDuration[14].CompareTo(expectedDuration[4]) == 0)
            {
                actual = true;
            }
            // Assert
            Assert.AreEqual(actual, expected);
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
            LoggerService _loggerService = new LoggerService();
            bool expected = true;
            bool actual = false;

            // Read logs from this path
            string path = "C:\\Users\\Midnightdrop\\Documents\\GitHub\\GreetNGroup\\Backend\\UnitTest\\UADTest\\TestLogs\\ServiceLogs\\PassLogs_SessionTimes";
            List<GNGLog> logList = new List<GNGLog>();

            // Act
            logList = _loggerService.ReadLogsPath(path);
            var averageTime = _uadService.CalculateSessionInformation(logList);
            if(averageTime[0].CompareTo("60") == 0)
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
            var newLog = _uadService.GetEntryLogswithURL(logList, entryLog);
            if (newLog.Count == 3)
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
            var newLog = _uadService.GetExitLogswithURL(logList, entryLog);
            if (newLog.Count == 4)
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
            string logID = "EventCreated";
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
        public void GetLogswithID_Pass_IDNotLogged()
        {
            //Arrange
            bool expected = true;
            bool actual = false;
            string logID = "EventDeleted";
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
            var newLog = _uadService.GetEntryLogswithURL(logList, entryLog);
            if (newLog.Count == 0)
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
            var newLog = _uadService.GetExitLogswithURL(logList, entryLog);
            if (newLog.Count == 0)
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
            var averageTime = _uadService.CalculateSessionInformation(logList);
            Console.WriteLine(averageTime);
            if(averageTime[0].CompareTo("60") == 0)
            {
                actual = true;
            }
            // Assert
            Assert.AreNotEqual(actual, expected);
        }
        #endregion
    }
}
