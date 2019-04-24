using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagerLayer.ArchiverManager;

namespace UnitTest.GNGArchiverTest
{
    /// <summary>
    /// Summary description for ArchiverTests
    /// </summary>
    [TestClass]
    public class ArchiverTests
    {

        [TestMethod]
        public void ArchiveLogs_Pass()
        {
            //Arrange
            GNGArchiverManager archiver = new GNGArchiverManager();
            bool expected = true;
            //Act
            bool actual = archiver.ArchiveOldLogs();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ArchiveLogs_Fail()
        {
            //Arrange
            GNGArchiverManager archiver = new GNGArchiverManager();
            bool expected = false;
            //Act
            bool actual = archiver.ArchiveOldLogs();

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
