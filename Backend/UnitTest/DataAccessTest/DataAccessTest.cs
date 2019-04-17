using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace UnitTest.DataAccessTest
{
    [TestClass]
    public class DataAccessTest
    {
        #region Testable Required Fields

        private const int UserId1 = 40;     // unused UId
        private const int TId1 = 2;         // indoor tag
        private const int TId2 = 10;        // food tag
        private const int CId = 30;         // claim id
        private const int EventId1 = 60;    // unused EventId

        #endregion

        #region Pass Tests

        #region Retrieve Tests

        [TestMethod]
        public void TestRetrieveEventTags()
        {
            // Arrange
            List<string> expected = new List<string> {"Indoors", "Food"};
            List<string> actual;
            const bool desiredResult = true;
            var result = false;
            EventTagService eventTagService = new EventTagService();
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

            // Creates temp event
            var eventTime = DateTime.Parse("5/20/2028");
            var eventName = "TestIn Party";
            var place = "CSULB";
            var newEvent = new Event(UserId1, EventId1, eventTime, eventName, place, "");

            eventService.InsertMadeEvent(newEvent);
            var foundEvent = eventService.GetEventById(EventId1);

            // Adds event Tags to event
            eventTagService.InsertEventTag(EventId1, TId1);
            eventTagService.InsertEventTag(EventId1, TId2);

            // Test
            actual = eventTagService.ReturnEventTagsOfEvent(EventId1);
            if (!expected.Except(actual).Any()) result = true;

            // Cleanup
            userService.DeleteUser(user);
            eventTagService.DeleteEventTag(EventId1, TId1);
            eventTagService.DeleteEventTag(EventId1, TId2);
            if (foundEvent != null) eventService.DeleteEvent(EventId1);

            // Assert
            Assert.AreEqual(desiredResult, result);
        }

        // Attempts to retrieve an Event object from the database
        [TestMethod]
        public void TestRetrieveEvent()
        {
            // Arrange
            const string expected = "TestIn Party";
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
            var eventTime = DateTime.Parse("5/20/2028");
            var place = "CSULB";
            var newEvent = new Event(UserId1, EventId1, eventTime, expected, place, "");

            eventService.InsertMadeEvent(newEvent);
            var foundEvent = eventService.GetEventById(EventId1);

            // Cleanup
            userService.DeleteUser(user);
            if (foundEvent != null) eventService.DeleteEvent(EventId1);

            // Assert
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

        [TestMethod]
        public void TestInsertNewEvent_Pass()
        {
            //Arrange
            var userId = 1;
            var eventName = "Testing Party";
            var todaysDate = DateTime.Now;
            var address = "123 Park Dr";
            var city = "El Paso";
            var state = "Texas";
            var zip = "99999";
            var description = "Hi";
            EventService es = new EventService();
            List<string> eventTags = new List<string>();
            eventTags.Add("Indoors");
            eventTags.Add("Music");
            
            //Act
            Event actual = es.InsertEvent(userId, todaysDate, eventName, address, city, state, zip
                , eventTags, description);

            //Assert
            Assert.IsNotNull(actual);

        }

        [TestMethod]
        public void InsertEventTags_Pass()
        {

        }

        #endregion

        #endregion

        #region Fail Tests

        // Attempts to add an existing user to the database 
        [TestMethod]
        public void TestInsertUserFail_Duplicate()
        {
            // Arrange 
            const bool expected = false;
            var actual = true;
            var initial = false;
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
            // Creates initial user
            initial = userService.InsertUser(user);
            // Attempts to duplicate user
            actual = userService.InsertUser(user);

            // Cleans up database of test data
            if (initial) userService.DeleteUser(user);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}