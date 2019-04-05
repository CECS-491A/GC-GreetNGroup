using System;
using ManagerLayer.ClaimManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagerLayer.UserManagement;

namespace UnitTest.DataAccessTest
{
    [TestClass]
    public class DataAccessTest
    {
        #region Testable Required Fields
        private int _userId1 = 50;
        private int _userId2 = 2;
        private int _userId3 = 3;
        private int _cId = 30;
        #endregion

        #region Pass Tests

        /// <summary>
        /// Adds a user into the database and checks if user exists inside the table
        /// Database is cleaned afterwards with a deletion of the inserted user
        /// </summary>
        [TestMethod]
        public void TestInsertUserPass()
        {
            // Arrange
            const bool expected = true;
            var actual = false;
            UserManager userManager = new UserManager();

            // Act
            var firstName = "Example";
            var lastName = "Set";
            var userName = "e.e@fakemail.com";
            var city = "Long Beach";
            var state = "California";
            var country = "United States";
            var dob = DateTime.Parse("09/19/1999");
            var isActivated = true;

            userManager.InsertUser(_userId1, firstName, lastName, userName, city, state, country, dob, isActivated);
            actual = userManager.IsUsernameFoundById(_userId1);
            userManager.DeleteUserById(_userId1);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Creates claim and tests if claim exists in db
        /// </summary>
        [TestMethod]
        public void TestInsertClaimPass()
        {
            // Arrange
            const bool expected = true;
            var actual = false;
            ClaimManager claimManager = new ClaimManager();

            // Act
            var cName = "TestClaim";

            claimManager.InsertClaim(_cId, cName);
            actual = claimManager.IsClaimInTable(_cId);
            claimManager.DeleteClaimById(_cId);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Fail Tests
        #endregion
    }
}