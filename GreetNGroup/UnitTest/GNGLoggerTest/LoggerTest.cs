using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GreetNGroup.Logging;

namespace UnitTest.GNGLoggerTest
{
    /// <summary>
    /// Summary description for LoggerTests
    /// </summary>
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void LogClickEvent_Pass()
        {
            string userID = "1";
            string startPoint = "https://www.greetngroup.com/join";
            string endPoint = "https://www.greetngroup.com/create";
            string ipAddress = "1.103.23.102";

            bool actual = GNGLogger.LogClicksMade(startPoint, endPoint, userID, ipAddress);

            Assert.AreEqual(actual, true);
        }

        [TestMethod]
        public void LogEntrytoWebsite()
        {
            string userID = "1";
            string url = "https://www.startpoint.com";
            string ipAddress = "1.1.1.1";

            bool actual = GNGLogger.LogEntryToWebsite(userID, url, ipAddress);

            Assert.AreEqual(actual, true);
        }

        [TestMethod]
        public void LogExitFromWebsite()
        {
            string userID = "1";
            string url = "https://www.endpoint.com";
            string ipAddress = "1.1.1.1";

            bool actual = GNGLogger.LogExitFromWebsite(userID, url, ipAddress);

            Assert.AreEqual(actual, true);
        }

        [TestMethod]
        public void LogLogGNGEventsCreated()
        {
            string userID = "1";
            string eventID = "1";
            string ipAddress = "1.1.1.1";
            bool actual = true;
            for(int i = 0; i < 2; i++)
            {
                actual = GNGLogger.LogGNGEventsCreated(userID, eventID, ipAddress);
            }

            Assert.AreEqual(actual, true);
        }

    }
}
