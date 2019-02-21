using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GreetNGroup.DataAccess.Queries;

namespace UnitTest.DataAccessTest
{
    [TestClass]
    public class DataAccessTest
    {
        #region Testable Required Fields
        private string _userId1 = "19452746";
        private string _userId2 = "10294753";
        private string _userId3 = "45892987";
        #endregion
        
        #region Pass Tests

        [TestMethod]
        public void TestInsertUserPass()
        {
            // Arrange
            const bool expected = true;
            var actual = false;

            // Act
            var uId = "p01dj9wjd99u3u";
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
            actual = DbCheck.FindUsername("p01dj9wjd99u3u");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /*
        public void RequireViewAndSystemAdminPass()
        {
            // Arrange
            const bool expected = false;
            var actual = true; 
            const string id = "p01dj9wjd99u3u";
            List<string> claimTest = new List<string>(){"CanViewEvents","SystemAdmin"};
            
            // Act
            var userToken1 = new Token(id);
            actual = claimTest.Except(userToken1.Claims).Any();
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void RequireFriendAndBlackList()
        {
            // Arrange
            const bool expected = false;
            var actual = true; 
            const string id = "p0499dj238e92j2";
            List<string> claimTest = new List<string>(){"CanFriendUsers","CanBlackListUsers"};
            
            // Act
            var userToken1 = new Token(id);
            actual = claimTest.Except(userToken1.Claims).Any();
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
        */
        #endregion

        #region Fail Tests

        /*
        [TestMethod]
        public void RequireCreateAndViewClaimsFail()
        {
            // Arrange
            const bool expected = true;
            var actual = false; 
            const string id = "p0499dj238e92j2";
            List<string> claimTest = new List<string>(){"CanViewEvents","CanCreateEvents"};
            
            // Act
            var userToken1 = new Token(id);
            actual = claimTest.Except(userToken1.Claims).Any();
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void RequireSystemAdminRightsFail()
        {
            // Arrange
            const bool expected = true;
            var actual = false; 
            const string id = "p0499dj238e92j2";
            List<string> claimTest = new List<string>(){"SystemAdmin"};
            
            // Act
            var userToken1 = new Token(id);
            actual = claimTest.Except(userToken1.Claims).Any();
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
        */
        #endregion
    }
}