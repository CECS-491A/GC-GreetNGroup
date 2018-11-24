using System.Collections.Generic;
using GreetNGroup.Claim_Controls;
using GreetNGroup.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.ClaimsTest
{
    [TestClass]
    public class ClaimTest
    {
        #region Testable Required Fields
        private List<ClaimsPool.Claims> _createAndViewEvents = new List<ClaimsPool.Claims>
            {ClaimsPool.Claims.CanCreateEvents, ClaimsPool.Claims.CanViewEvents};

        private List<ClaimsPool.Claims> _ViewEvents = new List<ClaimsPool.Claims>
            {ClaimsPool.Claims.CanViewEvents};

        private List<ClaimsPool.Claims> _adminRights = new List<ClaimsPool.Claims>
            {ClaimsPool.Claims.AdminRights};
        
        private List<ClaimsPool.Claims> _requireAdminRights = new List<ClaimsPool.Claims>
            {ClaimsPool.Claims.AdminRights};
        
        private List<ClaimsPool.Claims> _requireCreateAndViewEvents = new List<ClaimsPool.Claims>
            {ClaimsPool.Claims.CanCreateEvents, ClaimsPool.Claims.CanViewEvents};

        private List<ClaimsPool.Claims> _requireViewEvents = new List<ClaimsPool.Claims>
            {ClaimsPool.Claims.CanViewEvents};

        private string _userId1 = "a3f7&jl";
        private string _userId2 = "g2j7!4D";
        #endregion

        #region Pass Tests
        [TestMethod]
        public void RequireCreateAndViewClaimsPass()
        {
            // Arrange
            var expected = true;
            var actual = false;
            
            // Act
            var userToken1 = new Token(_userId1, _createAndViewEvents);
            actual = ClaimsAuthorization.VerifyClaims(userToken1, _requireCreateAndViewEvents);
            
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RequireCreateAndViewClaimsPass2()
        {
            // Arrange
            var expected = true;
            var actual = false;

            // Act
            var userToken1 = new Token(_userId1, _createAndViewEvents);
            actual = ClaimsAuthorization.VerifyClaims(userToken1, _requireViewEvents);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Fail Tests
        [TestMethod]
        public void RequireCreateAndViewClaimsFail()
        {
            // Arrange
            var expected = false;
            var actual = true;

            // Act
            var userToken2 = new Token(_userId2, _ViewEvents);
            actual = ClaimsAuthorization.VerifyClaims(userToken2, _requireCreateAndViewEvents);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RequireAdminRightsFail()
        {
            // Arrange
            var expected = false;
            var actual = true;

            // Act
            var userToken1 = new Token(_userId1, _ViewEvents);
            actual = ClaimsAuthorization.VerifyClaims(userToken1, _requireAdminRights);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}