using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GreetNGroup.DataAccess.Queries;

namespace UnitTest.DataAccessTest
{
    [TestClass]
    public class DataAccessTest
    {
        #region Testable Required Fields
        private int _userId1 = 1;
        private int _userId2 = 2;
        private int _userId3 = 3;
        #endregion
        
        #region Pass Tests

        /// <summary>
        /// Creates user and tests if user exists within db
        /// </summary>
        [TestMethod]
        public void TestInsertUserPass()
        {
            // Arrange
            const bool expected = true;
            var actual = false;

            // Act
            var uId = 1;
            var firstName = "Eric";
            var lastName = "Lee";
            var userName = "eric.lee@fakemail.com";
            var password = "badpassword5!";
            var city = "Long Beach";
            var state = "California";
            var country = "United States";
            var dob = DateTime.Parse("09/19/1997");
            var securityQ = "What is your favorite soda?";
            var securityA = "Dr. Peps";
            var isActivated = true;

            DbInsert.InsertUser(uId, firstName, lastName, userName, password, city, state, country, dob, securityQ,
                securityA, isActivated);
            actual = DbCheck.IsUsernameFound(_userId1);

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

            // Act
            var cId = 1;
            var cName = "AdminRights";

            DbInsert.InsertClaim(cId, cName);
            actual = DbCheck.IsClaimInTable(_userId1);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Fail Tests
        #endregion
    }
}