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
            string userID = "56FDIGH4920";
            string startPoint = "https://www.startpoint.com";
            string endPoint = "https://www.endpoint.com";
            string ipAddress = "1.103.23.102";

            bool actual = GNGLogger.LogClicksMade(startPoint, endPoint, userID, ipAddress);

            Assert.AreEqual(actual, true);
        }
    }
}
