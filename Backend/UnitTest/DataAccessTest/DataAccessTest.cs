using System;
using DataAccessLayer.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace UnitTest.DataAccessTest
{
    [TestClass]
    public class DataAccessTest
    {
        #region Testable Required Fields
        private int _userId1 = 50;
        private int _userId2 = 52;
        private int _userId3 = 53;
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
            UserService userService = new UserService();

            // Act
            var firstName = "Example";
            var lastName = "Set";
            var userName = "e.e@fakemail.com";
            var city = "Long Beach";
            var state = "California";
            var country = "United States";
            var dob = DateTime.Parse("09/19/1999");

            var user = new User(_userId1, firstName, lastName, userName, city, state, country, dob, true);
            userService.InsertUser(user);

            actual = userService.IsUsernameFoundById(user.UserId);
            userService.DeleteUser(user);

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