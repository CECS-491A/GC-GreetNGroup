using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagerLayer.UserManager;

namespace BkUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        #region Testable Required Fields
        private int _userId1 = 1;
        private int _userId2 = 2;
        private int _userId3 = 3;
        #endregion

        [TestMethod]
        public void TestInsertUserPass()
        {
            // Arrange
            const bool expected = true;
            var actual = false;
            UserManager userManager = new UserManager();

            // Act
            var uId = 5;
            var firstName = "Example";
            var lastName = "Set";
            var userName = "e.e@fakemail.com";
            var password = "badpass9!";
            var city = "Long Beach";
            var state = "California";
            var country = "United States";
            var dob = DateTime.Parse("09/19/1999");
            var securityQ = "What is your favorite soda?";
            var securityA = "Coke";
            var isActivated = true;

            userManager.InsertUser(uId, firstName, lastName, userName, password, city, state, country, dob, securityQ,
                securityA, isActivated);
            actual = userManager.IsUsernameFoundById(_userId1);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
