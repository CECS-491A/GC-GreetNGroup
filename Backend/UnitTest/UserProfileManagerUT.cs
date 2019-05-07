using Gucci.DataAccessLayer.Tables;
using Gucci.ManagerLayer.ProfileManagement;
using Gucci.ServiceLayer.Model;
using Gucci.ServiceLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace UnitTest
{
    [TestClass]
    public class UserProfileManagerUT
    {
        TestingUtils tu;
        UserProfileManager profileMan;
        UserService _userService;

        public UserProfileManagerUT()
        {
            tu = new TestingUtils();
            profileMan = new UserProfileManager();
            _userService = new UserService();
        }

        [TestMethod]
        public void GetUserRating_Pass()
        {
            // Arrange
            var expected = true;
            var actual = false;

            // Act
            if(profileMan.GetUserRating(1) != null)
            {
                actual = true;
            }
            
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUserRating_Fail_UserNotInDB()
        {
            // Arrange
            var expected = false;
            var actual = true;

            // Act
            if (profileMan.GetUserRating(1) == null)
            {
                actual = false;
            }

            // Assert
            Assert.AreEqual(expected, actual);
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
                DoB = newUser.DoB.ToString(),
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
        public void UpdateUserProfile_Fail_InvalidJwtToken()
        {

        }

        [TestMethod]
        public void UpdateUserProfile_Fail_Under18()
        {

        }

        [TestMethod]
        public void UpdateUserProfile_Fail_FieldIsNull()
        {

        }

        [TestMethod]
        public void IsProfileActivated_Pass()
        {

        }

        [TestMethod]
        public void IsProfileActivated_Fail_UserNotInDB()
        {

        }
    }
}
