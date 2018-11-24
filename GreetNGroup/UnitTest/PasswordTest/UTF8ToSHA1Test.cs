using System;
using GreetNGroup.Passwords;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTest.PasswordTest
{
    [TestClass]
    public class UTF8ToSHA1Test
    {
        [TestMethod]
        public void GetHash_Pass()
        {
            //Arrange
            UTF8ToSHA1 sha1 = new UTF8ToSHA1();
            string password = "password";

            //expected retrieved from online SHA1 generator
            string expected = "5BAA61E4C9B93F3F0682250B6CF8331B7EE68FD8";

            //Act
            string actual = sha1.ConvertToHash(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetHash_Fail()
        {
            //Arrange
            UTF8ToSHA1 sha1 = new UTF8ToSHA1();
            var password = "";

            //Act
            var actual = sha1.ConvertToHash(password);

            //Assert
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void GetHashNull_Fail()
        {
            //Arrange
            UTF8ToSHA1 sha1 = new UTF8ToSHA1();
            string password = null;
            string expected = null;

            //Act
            string actual = sha1.ConvertToHash(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
