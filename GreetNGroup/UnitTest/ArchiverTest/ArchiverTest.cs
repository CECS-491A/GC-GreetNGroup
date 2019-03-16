using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GreetNGroup.Archiving;

namespace UnitTest.ArchiverTest
{
    /// <summary>
    /// Summary description for ArchiverTest
    /// </summary>
    [TestClass]
    public class ArchiverTest
    {
        [TestMethod]
        public void ArchiveLogs_Pass()
        {
            bool actual = GNGArchiver.ArchiveOldLogs();
            Assert.AreEqual(actual, true);
        }
    }
}
