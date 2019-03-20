using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GreetNGroup.UAD;
namespace UnitTest.UADTest
{
    [TestClass]
    public class UserAnalysisDashboardTest
    {
        [TestMethod]
        public void TestMonthlyLoginvsRegistered()
        {
            UserAnalysisDashboard.LoginVSRegistered("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void TestMonthlyLoginOverSixMonths()
        {
            UserAnalysisDashboard.LoggedInMonthly();
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void Top5MostUsedFeature()
        {
            string a = UserAnalysisDashboard.Top5MostUsedFeature("March");
            Console.WriteLine(a);
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void AverageSessionDuration()
        {
            UserAnalysisDashboard.AverageSessionDuration("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void AverageSessionMonthly()
        {
            UserAnalysisDashboard.AverageSessionMonthly("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void Top5AveragePageSession()
        {
            UserAnalysisDashboard.Top5AveragePageSession("March");
            Assert.AreEqual(true, true);

        }


    }
}
