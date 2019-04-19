using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Tables;
using ManagerLayer.SearchManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Services;

namespace UnitTest.SearchTest
{
    [TestClass]
    public class SearchTest
    {
        #region Testable Required Fields

        // Event Fields
        private const int EventId1 = 60;
        private const string EventName = "TestIn Party";
        private const string Place = "CSULB";

        // User Fields
        private const int UserId1 = 40;
        private const string FirstName = "Example";
        private const string LastName = "Set";
        private const string UserName = "e.e@fakemail.com";
        private const string City = "Long Beach";
        private const string State = "California";
        private const string Country = "United States";

        #endregion

        #region Pass Tests

        [TestMethod]
        public void PassTestFindEventsByName()
        {
            // Arrange
            var searchManager = new SearchManager();
            var userService = new UserService();
            var eventService = new EventService();

            const bool expected = true;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var user = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            userService.InsertUser(user);

            var eventTime = DateTime.Parse("5/20/2028");
            var newEvent = new Event(UserId1, EventId1, eventTime, EventName, Place, "");
            eventService.InsertMadeEvent(newEvent);

            var comparisonList = new List<Event>(){ newEvent };

            var resultList = searchManager.GetEventListByName(EventName);
            var result = comparisonList[0].EventId == resultList[0].EventId;

            // Cleanup
            userService.DeleteUser(user);
            eventService.DeleteEvent(EventId1);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void PassTestFindUserByName()
        {
            // Arrange
            var searchManager = new SearchManager();
            var userService = new UserService();

            const bool expected = true;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var user = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            userService.InsertUser(user);

            bool result;
            var foundUser = searchManager.GetUserByUsername(UserName);
            if (foundUser != null) result = user.UserId == foundUser.UserId;
            else result = false;

            // Cleanup
            userService.DeleteUser(user);

            // Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Fail Tests

        [TestMethod]
        public void FailTestFindEventsByWrongName()
        {
            // Arrange
            var searchManager = new SearchManager();
            var userService = new UserService();
            var eventService = new EventService();

            const bool expected = false;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var user = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            userService.InsertUser(user);

            var eventTime = DateTime.Parse("5/20/2028");
            var newEvent = new Event(UserId1, EventId1, eventTime, EventName, Place, "");
            eventService.InsertMadeEvent(newEvent);

            var comparisonList = new List<Event>() { newEvent };

            var resultList = searchManager.GetEventListByName(EventName + "x");
            bool result;
            if (resultList.Any()) result = comparisonList[0].EventId == resultList[0].EventId;
            else result = false;

            // Cleanup
            userService.DeleteUser(user);
            eventService.DeleteEvent(EventId1);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FailTestFindUserByName()
        {
            // Arrange
            var searchManager = new SearchManager();
            var userService = new UserService();

            const bool expected = false;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var user = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            userService.InsertUser(user);

            bool result;
            var foundUser = searchManager.GetUserByUsername(UserName + "x");
            if (foundUser != null) result = user.UserId == foundUser.UserId;
            else result = false;

            // Cleanup
            userService.DeleteUser(user);

            // Assert
            Assert.AreEqual(expected, result);
        }

        #endregion
    }
}
