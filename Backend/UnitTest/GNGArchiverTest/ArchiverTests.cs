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
        public ArchiverTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

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
