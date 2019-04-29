using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gucci.ManagerLayer.LogManagement;

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
            //Arrange
            LogManager logManager = new LogManager();
            int userID = 1;
            string startPoint = "https://www.greetngroup.com/join";
            string endPoint = "https://www.greetngroup.com/create";
            string ipAddress = "1.103.23.102";

            //Act
            //bool actual = logManager.LogClicksMade(startPoint, endPoint, userID.ToString(), ipAddress);

            //Assert
            //Assert.AreEqual(actual, true);
        }

        [TestMethod]
        public void LogEntrytoWebsite_Pass()
        {
            //Arrange
            LogManager logManager = new LogManager();
            int userID = 1;
            string url = "https://www.startpoint.com";
            string ipAddress = "1.1.1.1";

            //Act
            //bool actual = logManager.LogEntryToWebsite(userID.ToString(), url, ipAddress);

            //Assert
            //Assert.AreEqual(actual, true);
        }

        [TestMethod]
        public void LogExitFromWebsite_Pass()
        {
            //Arrange
            LogManager logManager = new LogManager();
            string userID = "1";
            string url = "https://www.endpoint.com";
            string ipAddress = "1.1.1.1";

            //Act
            //bool actual = logManager.LogExitFromWebsite(userID, url, ipAddress);

            //Assert
            //Assert.AreEqual(actual, true);
        }

    }
}
