using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GreetNGroup.Passwords;
using System.Threading.Tasks;

namespace UnitTest.PasswordTest
{
    /// <summary>
    /// PasswordCheckerTest holds all test methods for PasswordChecker class to ensure
    /// method returns the proper values
    /// </summary>
    [TestClass]
    public class PasswordCheckerTest
    {
        
        [TestMethod]
        public void GetFirst5Chars_Pass()
        {
            //Arrange
            string password = "password";
            string expected = "5BAA6";

            //Act
            string actual = PasswordChecker.GetFirst5HashChars(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPassSuffix_Pass()
        {
            //Arrange
            string password = "password";
            string expected = "1E4C9B93F3F0682250B6CF8331B7EE68FD8";

            //Act
            string actual = PasswordChecker.GetHashSuffix(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task PwnedPasswordExists_Pass()
        {
            //Arrange
            string password = "password";
            var expected = true;

            //Act
            var actual = await PasswordChecker.IsPasswordPwned(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task PwnedPasswordOccurence_Pass()
        {
            //Arrange
            string password = "password";
            var expected = 3533661;

            //Act
            var actual = await PasswordChecker.PasswordOccurrences(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        //Fail Tests
        [TestMethod]
        public void GetFirst5Chars_Fail()
        {
            //Arrange
            string password = "password";
            string expected = "384FW";

            //Act
            string actual = PasswordChecker.GetFirst5HashChars(password);

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void GetPassSuffix_Fail()
        {
            //Arrange
            string password = "password";
            string expected = "28FG4B93F3F0682250B6CF8331B7EE68FD8";

            //Act
            string actual = PasswordChecker.GetHashSuffix(password);

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public async Task PwnedPasswordExists_Fail()
        {
            //Arrange
            string password = "password";
            var expected = false;

            //Act
            var actual = await PasswordChecker.IsPasswordPwned(password);

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public async Task NotPwnedPassword_Pass()
        {
            //Arrange
            string password = "#S@suqu3Uch1h4";
            var expected = false;

            //Act
            var actual = await PasswordChecker.IsPasswordPwned(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task NotPwnedPassword_Fail()
        {
            //Arrange
            string password = "#S@suqu3Uch1h4";
            var expected = true;

            //Act
            var actual = await PasswordChecker.IsPasswordPwned(password);

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public async Task NotPwnedPasswordOccurence_Pass()
        {
            //Arrange
            string password = "#S@suqu3Uch1h4";
            var expected = 0;

            //Act
            var actual = await PasswordChecker.PasswordOccurrences(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task NotPwnedPasswordOccurence_Fail()
        {
            //Arrange
            string password = "#S@suqu3Uch1h4";
            var expected = 1;

            //Act
            var actual = await PasswordChecker.PasswordOccurrences(password);

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public async Task PwnedPasswordOccurence_Fail()
        {
            //Arrange
            string password = "password";
            var expected = 1;

            //Act
            var actual = await PasswordChecker.PasswordOccurrences(password);

            //Assert
            Assert.AreNotEqual(expected, actual);
        }
    }
}
