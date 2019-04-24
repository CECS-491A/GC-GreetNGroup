using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gucci.ManagerLayer.ArchiverManager;

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
            ArchiverManager archiver = new ArchiverManager();
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
            ArchiverManager archiver = new ArchiverManager();
            bool expected = false;
            //Act
            bool actual = archiver.ArchiveOldLogs();

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
