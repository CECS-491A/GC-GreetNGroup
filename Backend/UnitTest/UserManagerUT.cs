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
    public class ProfileManagerUT
    {
        TestingUtils tu;
        UserManager userMan;
        UserService _userService;

        public ProfileManagerUT()
        {
            tu = new TestingUtils();
            userMan = new UserManager();
            _userService = new UserService();
        }

        [TestMethod]
        public void DoesUserExist_Pass()
        {
            //Arrange
            var newUser = tu.CreateUser();
            var userID = newUser.UserId;
            var expected = true;

            //Act
            var actual = userMan.DoesUserExists(userID);
            tu.DeleteUserFromDB(newUser);

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
        public void GetUserRating_Pass()
        {

        }

        [TestMethod]
        public void GetUserRating_Fail_UserNotInDB()
        {

        }

        [TestMethod]
        public void GetUser_Pass()
        {
            //Arrange
            var newUser = tu.CreateUser();
            var userID = newUser.UserId;
            UserProfile userPro = new UserProfile
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                UserName = newUser.UserName,
                DoB = newUser.DoB,
                City = newUser.City,
                State = newUser.State,
                Country = newUser.Country,
                EventCreationCount = newUser.EventCreationCount,
                Rating = profileMan.GetUserRating(userID)
            };
            var expected = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(userPro))
            };

            //Act
            var actual = profileMan.GetUser(Convert.ToString(userID));
            tu.DeleteUserFromDB(newUser);

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetUser_Fail_UserNotInDB()
        {
            //Arrange
            User newUser = new User
            {
                UserId = _userService.GetNextUserID()
            };
            var expected = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("User does not exist")
            };

            //Act
            var actual = profileMan.GetUser(Convert.ToString(newUser.UserId));

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UpdateUserProfile_Pass()
        {

        }

        [TestMethod]
        public void UpdateUserProfile_Fail()
        {

        }

        [TestMethod]
        public void GetEmail_Pass()
        {

        }

        [TestMethod]
        public void GetEmail_Fail()
        {
            
        }
    }
}
