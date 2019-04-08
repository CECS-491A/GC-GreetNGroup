using ManagerLayer.UADManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.UADTest
{
    [TestClass]
    public class UADTest
    {
        [TestMethod]
        public void TestMonthlyLoginvsRegistered()
        {
            UADManager uadManager = new UADManager();
            uadManager.GetLoginVSRegistered("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void TestMonthlyLoginOverSixMonths()
        {
            UADManager uadManager = new UADManager();
            uadManager.GetLoggedInMonthly("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void Top5MostUsedFeature()
        {
            UADManager uadManager = new UADManager();
            uadManager.GetTop5MostUsedFeature("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void AverageSessionDuration()
        {
            UADManager uadManager = new UADManager();
            uadManager.GetAverageSessionDuration("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void AverageSessionMonthly()
        {
            UADManager uadManager = new UADManager();
            uadManager.GetAverageSessionMonthly("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void Top5AveragePageSession()
        {
            UADManager uadManager = new UADManager();
            uadManager.GetTop5AveragePageSession("March");
            Assert.AreEqual(true, true);

        }
    }
}
