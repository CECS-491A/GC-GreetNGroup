using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagerLayer.GNGLogManager;

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
            GNGLogManager logManager = new GNGLogManager();
            string userID = "1";
            string startPoint = "https://www.greetngroup.com/join";
            string endPoint = "https://www.greetngroup.com/create";
            string ipAddress = "1.103.23.102";

            bool actual = logManager.LogClicksMade(startPoint, endPoint, userID, ipAddress);

            Assert.AreEqual(actual, true);
        }

        [TestMethod]
        public void LogEntrytoWebsite()
        {
            GNGLogManager logManager = new GNGLogManager();
            string userID = "1";
            string url = "https://www.startpoint.com";
            string ipAddress = "1.1.1.1";

            bool actual = logManager.LogEntryToWebsite(userID, url, ipAddress);

            Assert.AreEqual(actual, true);
        }

        [TestMethod]
        public void LogExitFromWebsite()
        {
            GNGLogManager logManager = new GNGLogManager();
            string userID = "1";
            string url = "https://www.endpoint.com";
            string ipAddress = "1.1.1.1";

            bool actual = logManager.LogExitFromWebsite(userID, url, ipAddress);

            Assert.AreEqual(actual, true);
        }

        [TestMethod]
        public void LogLogGNGEventsCreated()
        {
            GNGLogManager logManager = new GNGLogManager();
            string userID = "1";
            string eventID = "1";
            string ipAddress = "1.1.1.1";
            bool actual = true;
            actual = logManager.LogGNGEventsCreated(userID, eventID, ipAddress);

            Assert.AreEqual(actual, true);
        }

    }
}
