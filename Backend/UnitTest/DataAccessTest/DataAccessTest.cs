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

        private const int UserId1 = 50;
        private const int UserId2 = 52;
        private const int UserId3 = 53;
        private const int CId = 30;
        private const int EventId1 = 60;

        #endregion

        #region Pass Tests

        [TestMethod]
        public void TestRetrieveEvent()
        {
            // Arrange
            const string expected = "Pizza Party";
            EventService eventService = new EventService();
            UserService userService = new UserService();

            // Act

            // Creates temp user
            var firstName = "Example";
            var lastName = "Set";
            var userName = "e.e@fakemail.com";
            var city = "Long Beach";
            var state = "California";
            var country = "United States";
            var dob = DateTime.Parse("09/19/1999");

            var user = new User(UserId1, firstName, lastName, userName, city, state, country, dob, true);
            userService.InsertUser(user);

            // Creates temp event under the user
            var eventTime = DateTime.Parse("10/20/2020");
            var eventName = "Pizza Party";
            var place = "CSULB";
            var newEvent = new Event(UserId1, EventId1, eventTime, expected, place);

            eventService.InsertMadeEvent(newEvent);
            var foundEvent = eventService.GetEventById(EventId1);

            Assert.AreEqual(expected, foundEvent.EventName);
        }

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

            var user = new User(UserId1, firstName, lastName, userName, city, state, country, dob, true);
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

            claimService.InsertClaim(CId, cName);
            actual = claimService.IsClaimInTable(CId);
            claimService.DeleteClaimById(CId);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Fail Tests
        #endregion
    }
}