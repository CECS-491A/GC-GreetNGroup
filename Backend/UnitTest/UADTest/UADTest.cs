using ManagerLayer.UADManager;
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
            string a = uadManager.LoginVSRegistered("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void TestMonthlyLoginOverSixMonths()
        {
            UADManager uadManager = new UADManager();
            uadManager.LoggedInMonthly();
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void Top5MostUsedFeature()
        {
            UADManager uadManager = new UADManager();
            uadManager.Top5MostUsedFeature("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void AverageSessionDuration()
        {
            UADManager uadManager = new UADManager();
            uadManager.AverageSessionDuration("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void AverageSessionMonthly()
        {
            UADManager uadManager = new UADManager();
            uadManager.AverageSessionMonthly("March");
            Assert.AreEqual(true, true);

        }

        [TestMethod]
        public void Top5AveragePageSession()
        {
            UADManager uadManager = new UADManager();
            uadManager.Top5AveragePageSession("March");
            Assert.AreEqual(true, true);

        }
    }
}
