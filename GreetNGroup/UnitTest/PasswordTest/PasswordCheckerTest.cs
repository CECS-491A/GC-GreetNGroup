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
        PasswordChecker pwCheck = new PasswordChecker();

        [TestMethod]
        public async Task PwnedPasswordExists_Pass()
        {
            //Arrange
            string password = "password";
            var expected = true;

            //Act
            var actual = await pwCheck.IsPasswordPwned(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task PwnedPasswordOccurence_Pass()
        {
            //Arrange
            string password = "password";

            //Act
            var actual = await pwCheck.PasswordOccurrences(password);

            //Assert
            Assert.IsTrue(actual > 1);
        }

        [TestMethod]
        public async Task PwnedPasswordExists_Fail()
        {
            //Arrange
            string password = "password";
            var expected = false;

            //Act
            var actual = await pwCheck.IsPasswordPwned(password);

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
            var actual = await pwCheck.IsPasswordPwned(password);

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
            var actual = await pwCheck.IsPasswordPwned(password);

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public async Task NotPwnedPasswordOccurence_Pass()
        {
            //Arrange
            string password = "#S@suqu3Uch1h4";

            //Act
            var actual = await pwCheck.PasswordOccurrences(password);

            //Assert
            Assert.IsTrue(actual <= 0);
        }

        [TestMethod]
        public async Task NotPwnedPasswordOccurence_Fail()
        {
            //Arrange
            string password = "#S@suqu3Uch1h4";

            //Act
            var actual = await pwCheck.PasswordOccurrences(password);

            //Assert
            Assert.IsFalse(actual > 1);
        }

        [TestMethod]
        public async Task PwnedPasswordOccurence_Fail()
        {
            //Arrange
            string password = "password";
            var expected = 1;

            //Act
            var actual = await pwCheck.PasswordOccurrences(password);

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public async Task ResponseCode200_Pass()
        {
            //Arrange
            var password = "password";

            //Act
            var actual = await pwCheck.GetResponseCode(password);

            //Assert
            Assert.IsTrue(actual.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task ResponseCodeUnsuccessful_Pass()
        {
            //Arrange
            var password = "";

            //Act
            var actual = await pwCheck.GetResponseCode(password);

            //Assert
            Assert.IsFalse(actual.IsSuccessStatusCode);
        }
    }
}
