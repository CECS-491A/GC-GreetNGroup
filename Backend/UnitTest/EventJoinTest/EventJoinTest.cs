using System;
using Gucci.ServiceLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gucci.ManagerLayer.AttendeeManagement;
namespace UnitTest.EventJoinTest
{
    [TestClass]
    public class EventJoinTest
    {
        private JWTService jwtService = new JWTService();
        private AttendeeManager attendeeManager = new AttendeeManager();
        [TestMethod]
        public void JoinEvent_Pass_ValidEverything()
        {
            // assign
            var testUId = 100;
            var newTestUser = "julianpoyo+22@gmail.com";
            var jwtString = jwtService.CreateToken(newTestUser, testUId);
            var eventid = 0;

            // act 
            var result = attendeeManager.JoinEvent(eventid, jwtString);

            Console.WriteLine(result);

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void LeaveEvent_Pass_ValidEverything()
        {
            // assign
            var testUId = 100;
            var newTestUser = "julianpoyo+22@gmail.com";
            var jwtString = jwtService.CreateToken(newTestUser, testUId);
            var eventid = 0;

            // act 
            var result = attendeeManager.LeaveEvent(eventid, jwtString);

            Console.WriteLine(result);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void JoinEvent_Fail_NotValidClaims()
        {
            // assign
            var testUId = 52;
            var newTestUser = "winnmoo@gmail.com";
            var jwtString = jwtService.CreateToken(newTestUser, testUId);
            var eventid = 0;

            // act 
            var result = attendeeManager.JoinEvent(eventid, jwtString);

            Console.WriteLine(result);

            Assert.IsNotNull(result);
        }
    }
}
