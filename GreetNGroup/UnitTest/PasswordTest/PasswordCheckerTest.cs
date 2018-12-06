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
        public async Task PwnedPasswordExists_Pass()
        {
            //Arrange
            PasswordChecker pwCheck = new PasswordChecker();
            string password = "abc123";
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
            PasswordChecker pwCheck = new PasswordChecker();
            string password = "abc123";

            //Act
            var actual = await pwCheck.PasswordOccurrences(password);

            //Assert
            Assert.IsTrue(actual > 1);
        }

        [TestMethod]
        public async Task PwnedPasswordExists_Fail()
        {
            //Arrange
            PasswordChecker pwCheck = new PasswordChecker();
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
            PasswordChecker pwCheck = new PasswordChecker();
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
            PasswordChecker pwCheck = new PasswordChecker();
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
            PasswordChecker pwCheck = new PasswordChecker();
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
            PasswordChecker pwCheck = new PasswordChecker();
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
            PasswordChecker pwCheck = new PasswordChecker();
            string password = "password";

            //Act
            var actual = await pwCheck.PasswordOccurrences(password);

            //Assert
            Assert.IsFalse(actual <= 0);
        }

        [TestMethod]
        public async Task ResponseCode200_Pass()
        {
            //Arrange
            PasswordChecker pwCheck = new PasswordChecker();
            var path = "https://api.pwnedpasswords.com/range/" + "5baa6";

            //Act
            var actual = await pwCheck.GetResponseCode(path);

            //Assert
            Assert.IsTrue(actual.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task ResponseCodeUnsuccessful_Pass()
        {
            //Arrange
            PasswordChecker pwCheck = new PasswordChecker();
            var path = "https://api.pwnedpasswords.com/range/" + "helloworld";

            //Act
            var actual = await pwCheck.GetResponseCode(path);

            //Assert
            Assert.IsFalse(actual.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task PasswordOccurrenceArgumentExceptionNull_Pass()
        {
            //Arrange
            PasswordChecker pwCheck = new PasswordChecker();
            string password = null;
            var expected = -1;

            //Act
            var actual = await pwCheck.PasswordOccurrences(password);

            //Assert
            Assert.AreEqual(expected, actual);
        }


    }
}
