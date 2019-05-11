using Gucci.DataAccessLayer.Tables;
using Gucci.ManagerLayer.ProfileManagement;
using Gucci.ServiceLayer.Model;
using Gucci.ServiceLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using ManagerLayer.UserManagement;

namespace UnitTest
{
    [TestClass]
    public class UserManagerUT
    {
        TestingUtils tu;
        UserManager userMan;
        UserService _userService;
        JWTService _jwtService;

        public UserManagerUT()
        {
            tu = new TestingUtils();
            userMan = new UserManager();
            _userService = new UserService();
            _jwtService = new JWTService();
        }

        [TestMethod]
        public void DoesUserExist_Pass()
        {
            //Arrange
            var expected = true;

            //Act
            var actual = userMan.DoesUserExists(1);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DoesUserExist_Fail_UserNotInDB()
        {
            //Arrange
            var newUser = new User
            {
                UserId = _userService.GetNextUserID()
            };
            var expected = false;

            //Act
            var actual = userMan.DoesUserExists(newUser.UserId);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetEmail_Pass()
        {
            // Arrange
            var JwtToken = _jwtService.CreateToken("bobvong@gmail.com", 1);

            var expected = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("test@gmail.com")
            };

            // Act
            var actual = userMan.GetEmail(JwtToken);

            // Assert
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [TestMethod]
        public void GetEmail_Fail_InvalidJwtToken()
        {
            // Arrange
            var JwtToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9" +
                ".eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9" +
                ".2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPGIRs8ZLjWB0";

            var expected = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("Session is invalid")
            };

            // Act
            var actual = userMan.GetEmail(JwtToken);

            // Assert
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [TestMethod]
        public void DeleteUserUsingSSO_Pass()
        {

        }

        [TestMethod]
        public void DeleteUserUsingSSO_Fail_InvalidRequest()
        {

        }

        [TestMethod]
        public void DeleteUser_Pass()
        {
            // Arrange
            var newUser = tu.CreateUser();
            var expected = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("User was deleted from GreetNGroup")
            };

            // Act
            var actual = userMan.DeleteUser(newUser.UserName);

            // Assert
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }

        [TestMethod]
        public void DeleteUser_Fail_UserNotInDB()
        {
            // Arrange
            var expected = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("User not found in system")
            };
            // Act
            var actual = userMan.DeleteUser("qwertyswag@gmail.com");

            // Assert
            Assert.AreEqual(expected.StatusCode, actual.StatusCode);
        }
    }
}
