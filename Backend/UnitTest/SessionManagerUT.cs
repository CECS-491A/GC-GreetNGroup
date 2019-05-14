using Gucci.ManagerLayer;
using Gucci.ServiceLayer.Requests;
using ManagerLayer.UserManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;

namespace UnitTest
{
    [TestClass]
    public class SessionManagerUT
    {
        SessionManager sessionMan;
        UserManager userMan;

        public SessionManagerUT()
        {
            sessionMan = new SessionManager("5E5DDBD9B984E4C95BBFF621DF91ABC9A5318DAEC0A3B231B4C1BC8FE0851610");
            userMan = new UserManager();
        }
        /*
        [TestMethod]
        public void Login_Pass()
        {
            // Arrange
            SSOController controller = new SSOController();
            SSOUserRequest request = new SSOUserRequest
            {
                ssoUserId = "b33ae8eb-9cfa-4c7d-91cc-b0e2ecc74792",
                email = "tester427@mail.com",
                timestamp = "1556421373491",
                signature = "2d+xE3d0PegywQ812+BBn8TjA4FS/GC/06yMci4OVNU="
            };

            var expected = new HttpResponseMessage(HttpStatusCode.SeeOther);

            // Act
            var actual = sessionMan.Login(controller, request);

            // Assert
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [TestMethod]
        public void Login_Fail_InvalidRequest()
        {
            // Arrange
            SSOController controller = new SSOController();
            SSOUserRequest request = new SSOUserRequest
            {
                ssoUserId = "b33ae8eb-9cfa-4c7d-91cc-b0e2ecc74792",
                email = "tester427@gmail.com",
                timestamp = "1556421373491",
                signature = "2d+xE3d0PegywQ812+BBn8TjA4FS/GC/06yMci4OVNU"
            };

            var expected = new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Act
            var actual = sessionMan.Login(controller, request);

            // Assert
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }
        */

        [TestMethod]
        public void Logout_Pass()
        {
            // Arrange
            SSOUserRequest request = new SSOUserRequest
            {
                ssoUserId = "b33ae8eb-9cfa-4c7d-91cc-b0e2ecc74792",
                email = "tester427@mail.com",
                timestamp = "1556421373491",
                signature = "2d+xE3d0PegywQ812+BBn8TjA4FS/GC/06yMci4OVNU="
            };
            var expected = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("User has logged out of GreetNGroup")
            };

            // Act
            var actual = sessionMan.Logout(request);

            // Assert
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [TestMethod]
        public void Logout_Fail_InvalidRequest()
        {
            // Arrange
            SSOUserRequest request = new SSOUserRequest
            {
                ssoUserId = "b33ae8eb-9cfa-4c7d-91cc-b0e2ecc74792",
                email = "tester427@gmail.com",
                timestamp = "1556421373491",
                signature = "2d+xE3d0PegywQ812+BBn8TjA4FS/GC/06yMci4OVNU"
            };
            var expected = new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Act
            var actual = sessionMan.Logout(request);

            // Assert
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [TestMethod]
        public void Logout_Fail_UserNotInDB()
        {
            // Arrange
            SSOUserRequest request = new SSOUserRequest
            {
                ssoUserId = "b33ae8eb-9cfa-4c7d-91cc-b0e2ecc74792",
                email = "tester427@gmail.com",
                timestamp = "1556421373491",
                signature = "2d+xE3d0PegywQ812+BBn8TjA4FS/GC/06yMci4OVNU"
            };

            userMan.DeleteUser("tester427@gmail.com");

            var expected = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("User Does Not Exist")
            };

            // Act
            var actual = sessionMan.Logout(request);

            // Assert
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }
    }
}
