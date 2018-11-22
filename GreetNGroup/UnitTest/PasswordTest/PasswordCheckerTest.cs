using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GreetNGroup.Passwords;
using System.Threading.Tasks;

namespace UnitTest.PasswordTest
{
    /// <summary>
    /// Summary description for PasswordCheckerTest
    /// </summary>
    [TestClass]
    public class PasswordCheckerTest
    {
        [TestMethod]
        public void GetFirst5Chars_Pass()
        {
            string password = "password";
            string expected = "5BAA6";

            string actual = PasswordChecker.GetFirst5HashChars(password);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPassSuffix_Pass()
        {
            string password = "password";
            string expected = "1E4C9B93F3F0682250B6CF8331B7EE68FD8";

            string actual = PasswordChecker.GetHashSuffix(password);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task GetPwnedPassword_Pass()
        {
            //Arrange
            string password = "password";
            var expected = true;

            //Act
            var actual = await PasswordChecker.IsPasswordPwned(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
