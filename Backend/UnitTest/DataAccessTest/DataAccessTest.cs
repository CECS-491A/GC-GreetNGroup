using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ManagerLayer.UserManagement;
using ServiceLayer.Services;

namespace UnitTest.DataAccessTest
{
    [TestClass]
    public class DataAccessTest
    {
        #region Testable Required Fields
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
            UserService userManager = new UserService();

            // Act
            var firstName = "Example";
            var lastName = "Set";
            var userName = "e.e@fakemail.com";
            var city = "Long Beach";
            var state = "California";
            var country = "United States";
            var dob = DateTime.Parse("09/19/1999");

            var user = userManager.CreateUser(firstName, lastName, userName, city, state, country, dob);
            actual = userManager.UserExist(user.UserId);
            userManager.DeleteUserByID(user.UserId);

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
            ClaimService claimService = new ClaimService();

            // Act
            var cName = "TestClaim";

            claimService.InsertClaim(_cId, cName);
            actual = claimService.IsClaimInTable(_cId);
            claimService.DeleteClaimById(_cId);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Fail Tests
        #endregion
    }
}