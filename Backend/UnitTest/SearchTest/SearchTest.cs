using System;
using System.Collections.Generic;
using Gucci.DataAccessLayer.DataTransferObject;
using Gucci.DataAccessLayer.Tables;
using Gucci.ManagerLayer.SearchManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gucci.ServiceLayer.Services;

namespace UnitTest.SearchTest
{
    [TestClass]
    public class SearchTest
    {
        #region Testable Required Fields

        // Event Fields
        private const int EventId1 = 60;
        private const int EventId2 = 61;
        private const string EventName = "TestIn Party";
        private const string EventName2 = "TestIn Event";
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
        public void PassTestSortByTagsAndDateRange()
        {
            // Arrange
            var eventFinderService = new EventFinderService();
            var userService = new UserService();
            var eventService = new EventService();
            var eventTagService = new EventTagService();

            const bool expected = true;
            var result = false;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var user = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            userService.InsertUser(user);

            var eventTime = DateTime.Parse("5/25/2028");
            var expectedEvent = new Event(UserId1, EventId1, eventTime, EventName, Place, "");
            eventService.InsertMadeEvent(expectedEvent);

            var eventTime2 = DateTime.Parse("5/20/2028");
            var expectedEvent2 = new Event(UserId1, EventId2, eventTime2, EventName2, Place, "");
            eventService.InsertMadeEvent(expectedEvent2);

            // TagId 4 -> Art
            eventTagService.InsertEventTag(EventId1, 4);
            eventTagService.InsertEventTag(EventId2, 4);
            var foundEvents = eventFinderService.FindEventByEventTags(new List<string>() { "Art" });
            var sortedEvents = eventFinderService.CullEventListByDateRange(foundEvents, eventTime2, eventTime);

            if (sortedEvents[0].EventId == expectedEvent2.EventId && sortedEvents[1].EventId == expectedEvent.EventId)
            {
                result = true;
            }

            // Cleanup
            userService.DeleteUser(user);
            eventTagService.DeleteEventTag(EventId1, 4);
            eventTagService.DeleteEventTag(EventId2, 4);
            eventService.DeleteEvent(EventId1);
            eventService.DeleteEvent(EventId2);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void PassTestFindEventsWithinDateRange()
        {
            // Arrange
            var eventFinderService = new EventFinderService();
            var userService = new UserService();
            var eventService = new EventService();

            const bool expected = true;
            var result = false;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var user = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            userService.InsertUser(user);

            var eventTime = DateTime.Parse("5/25/2028");
            var expectedEvent = new Event(UserId1, EventId1, eventTime, EventName, Place, "");
            eventService.InsertMadeEvent(expectedEvent);

            var eventTime2 = DateTime.Parse("5/20/2028");
            var expectedEvent2 = new Event(UserId1, EventId2, eventTime2, EventName2, Place, "");
            eventService.InsertMadeEvent(expectedEvent2);

            var sortedEvents = eventFinderService.FindEventsByDateRange("5/20/2028", "5/25/2028");
            if (sortedEvents[0].EventId == expectedEvent2.EventId && sortedEvents[1].EventId == expectedEvent.EventId)
            {
                result = true;
            }

            // Cleanup
            userService.DeleteUser(user);
            eventService.DeleteEvent(EventId1);
            eventService.DeleteEvent(EventId2);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void PassTestFindEventsByTag()
        {
            // Arrange
            var eventFinderService = new EventFinderService();
            var userService = new UserService();
            var eventService = new EventService();
            var eventTagService = new EventTagService();

            const bool expected = true;
            var result = false;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var user = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            userService.InsertUser(user);

            var eventTime = DateTime.Parse("5/20/2028");
            var expectedEvent = new Event(UserId1, EventId1, eventTime, EventName, Place, "");
            eventService.InsertMadeEvent(expectedEvent);

            // TagId 4 -> Art
            eventTagService.InsertEventTag(EventId1, 4);
            var foundEvents = eventFinderService.FindEventByEventTags(new List<string>(){"Art"});

            foreach (var events in foundEvents)
            {
                if (events.EventId.Equals(expectedEvent.EventId))
                {
                    result = true;
                }
            }

            // Cleanup
            userService.DeleteUser(user);
            eventTagService.DeleteEventTag(EventId1, 4);
            eventService.DeleteEvent(EventId1);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void PassTestFindEventsByName()
        {
            // Arrange
            var searchManager = new SearchManager();
            var userService = new UserService();
            var eventService = new EventService();

            const bool expected = true;
            var result = false;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var user = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            userService.InsertUser(user);

            var eventTime = DateTime.Parse("5/20/2028");
            var expectedEvent = new Event(UserId1, EventId1, eventTime, EventName, Place, "");
            var expectedEventDto = new DefaultEventSearchDto(UserId1, EventName, Place, eventTime); 
            eventService.InsertMadeEvent(expectedEvent);

            // Retrieves list based on input -- includes partial match
            var resultList = searchManager.GetEventListByName(EventName);
            foreach (var r in resultList)
            {
                // if expected result is within the list -- success
                if (r.CompareDefaultEventSearchDto(expectedEventDto))
                {
                    result = true;
                }
            }

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
            var result = false;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var expectedUser = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            var expectedUserDto = new DefaultUserSearchDto(UserName);
            userService.InsertUser(expectedUser);
            
            var foundUser = searchManager.GetUserByUsername(UserName);
            foreach (DefaultUserSearchDto u in foundUser)
            {
                if (expectedUserDto.CompareDefaultUserDto(u))
                {
                    result = true;
                }
            }

            // Cleanup
            userService.DeleteUser(expectedUser);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void PassTestFindUserByUid()
        {
            // Arrange
            var searchManager = new SearchManager();
            var userService = new UserService();

            const bool expected = true;
            var result = false;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var user = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            var expectedUserDto = new DefaultUserSearchDto(UserName);
            userService.InsertUser(user);

            var foundUser = searchManager.GetUserByUserId(UserId1);
            foreach (DefaultUserSearchDto u in foundUser)
            {
                if (expectedUserDto.CompareDefaultUserDto(u))
                {
                    result = true;
                }
            }

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

            const bool expected = true;
            var result = true;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var user = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            userService.InsertUser(user);

            var eventTime = DateTime.Parse("5/20/2028");
            var expectedEvent = new Event(UserId1, EventId1, eventTime, EventName, Place, "");
            var expectedEventDto = new DefaultEventSearchDto(UserId1, EventName, Place, eventTime);
            eventService.InsertMadeEvent(expectedEvent);

            // Retrieves list based on input -- includes partial match
            var resultList = searchManager.GetEventListByName(EventName + "ahhkbfsdf");
            foreach (var r in resultList)
            {
                // if expected result is within the list -- success
                if (r.CompareDefaultEventSearchDto(expectedEventDto))
                {
                    result = false;
                }
            }

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

            const bool expected = true;
            var result = true;

            // Act
            var dob = DateTime.Parse("09/19/1999");
            var expectedUser = new User(UserId1, FirstName, LastName, UserName, City, State, Country, dob, true);
            var expectedUserDto = new DefaultUserSearchDto(UserName);
            userService.InsertUser(expectedUser);

            var foundUser = searchManager.GetUserByUsername(UserName + "akhfbrusybvs");
            foreach (DefaultUserSearchDto u in foundUser)
            {
                if (expectedUserDto.CompareDefaultUserDto(u))
                {
                    result = false;
                }
            }

            // Cleanup
            userService.DeleteUser(expectedUser);

            // Assert
            Assert.AreEqual(expected, result);
        }

        #endregion
    }
}
