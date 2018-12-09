using System.Collections.Generic;
using System.Linq;
using GreetNGroup.Claim_Controls;
using GreetNGroup.Tokens;
using GreetNGroup.SiteUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GreetNGroup;
using GreetNGroup.Data_Access;

namespace UnitTest.ClaimsTest
{
    [TestClass]
    public class ClaimTest
    {
        #region Testable Required Fields
        private string _userId1 = "19452746";
        private string _userId2 = "10294753";
        private string _userId3 = "45892987";
        #endregion
        
        #region Pass Tests
        [TestMethod]
        public void RequireViewAndSystemAdminPass()
        {
            // Arrange
            const bool expected = true;
            var actual = false; 
            const string id = "p01dj9wjd99u3u";
            var claims = DataBaseQueries.FindClaimsFromUser(id);
            List<string> claimTest = new List<string>(){"CanViewEvents","SystemAdmin"};
            
            // Act
            var userToken1 = new Token(_userId1);
            if (!(claimTest.Except(claims).Any()))
            {
                actual = true;
            }
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void RequireFriendAndBlackList()
        {
            // Arrange
            const bool expected = true;
            var actual = false; 
            const string id = "p0499dj238e92j2";
            var claims = DataBaseQueries.FindClaimsFromUser(id);
            List<string> claimTest = new List<string>(){"CanFriendUsers","CanBlackListUsers"};
            
            // Act
            var userToken1 = new Token(_userId1);
            if (!(claimTest.Except(claims).Any()))
            {
                actual = true;
            }
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
        
        #region Fail Tests
        [TestMethod]
        public void RequireCreateAndViewClaimsFail()
        {
            // Arrange
            const bool expected = false;
            var actual = true; 
            const string id = "p0499dj238e92j2";
            var claims = DataBaseQueries.FindClaimsFromUser(id);
            List<string> claimTest = new List<string>(){"CanViewEvents","CanCreateEvents"};
            
            // Act
            var userToken1 = new Token(_userId1);
            if (claimTest.Except(claims).Any())
            {
                actual = false;
            }
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void RequireSystemAdminRightsFail()
        {
            // Arrange
            const bool expected = false;
            var actual = true; 
            const string id = "p0499dj238e92j2";
            var claims = DataBaseQueries.FindClaimsFromUser(id);
            List<string> claimTest = new List<string>(){"SystemAdmin"};
            
            // Act
            var userToken1 = new Token(_userId1);
            if (claimTest.Except(claims).Any())
            {
                actual = false;
            }
            
            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        #endregion
    }
}