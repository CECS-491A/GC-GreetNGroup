using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GreetNGroup.Logging;

namespace UnitTest.LoggerTest
{
    /// <summary>
    /// Summary description for LoggerTests
    /// </summary>
    [TestClass]
    public class LoggerTests
    {
        public LoggerTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        GNGLogger logger = new GNGLogger();

        [TestMethod]
        public void LogClickEvent_Pass()
        {
            
            string userID = "10032";
            string entryPoint = "http://www.someentry.com";
            string endPoint = "http://www.someend.com";

            bool expected = true;
            bool actual = logger.LogClicksMade(entryPoint, endPoint, userID);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LogClickEvent2_Pass()
        {
            string userID = "10055";
            string entryPoint = "http://www.someentry.com";
            string endPoint = "http://www.someend.com";

            bool expected = true;
            bool actual = logger.LogClicksMade(entryPoint, endPoint, userID);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LogError_Pass()
        {
            string userID = "10055";
            string errorCode = "404";
            string urlOfErr = "http://www.404.com";

            bool expected = true;
            bool actual = logger.LogErrorsEncountered(userID, errorCode, urlOfErr);

            Assert.AreEqual(expected, actual);
        }

    }
}
