using System.Collections.Generic;
using GreetNGroup.Claim_Controls;
using GreetNGroup.Tokens;
using GreetNGroup.SiteUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.ClaimsTest
{
    [TestClass]
    public class ClaimTest
    {
        #region Testable Required Fields
        private List<ClaimsPool.Claims> _requireAdminRights = new List<ClaimsPool.Claims>
            {ClaimsPool.Claims.AdminRights};
        
        private List<ClaimsPool.Claims> _requireCreateAndViewEvents = new List<ClaimsPool.Claims>
            {ClaimsPool.Claims.CanCreateEvents, ClaimsPool.Claims.CanViewEvents};

        private List<ClaimsPool.Claims> _requireViewEvents = new List<ClaimsPool.Claims>
            {ClaimsPool.Claims.CanViewEvents};

        private string _userId1 = "19452746";
        private string _userId2 = "10294753";
        private string _userId3 = "45892987";
        /*
         * The following attributes are temporary
         * They represent the database holding users that will
         * be referenced by the token, in order to grab
         * user claims
         */
        private UserDatabaseTemp userDatabaseCreator;
        private Dictionary<string, List<ClaimsPool.Claims>> userData;
        //
        #endregion
        
        #region Pass Tests
        [TestMethod]
        public void RequireCreateAndViewClaimsPass()
        {
            // Arrange
            var expected = true;
            var actual = false;
            userDatabaseCreator = new UserDatabaseTemp();
            userDatabaseCreator.SetupTable();
            userData = userDatabaseCreator.ReturnUserTable();
            
            // Act
            var userToken1 = new Token(_userId1);
            
            /*
             * This if statement represents a query call to find the
             * specified user, and retrieve claims
             */
            if (userData.ContainsKey(userToken1.UserId))
            {
                userToken1.Claims = userData[userToken1.UserId];
            }
            
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
            userDatabaseCreator = new UserDatabaseTemp();
            userDatabaseCreator.SetupTable();
            userData = userDatabaseCreator.ReturnUserTable();

            // Act
            var userToken1 = new Token(_userId1);

            /*
             * This if statement represents a query call to find the
             * specified user, and retrieve claims
             */
            if (userData.ContainsKey(userToken1.UserId))
            {
                userToken1.Claims = userData[userToken1.UserId];
            }

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
            userDatabaseCreator = new UserDatabaseTemp();
            userDatabaseCreator.SetupTable();
            userData = userDatabaseCreator.ReturnUserTable();

            // Act
            var userToken2 = new Token(_userId2);

            /*
             * This if statement represents a query call to find the
             * specified user, and retrieve claims
             */
            if (userData.ContainsKey(userToken2.UserId))
            {
                userToken2.Claims = userData[userToken2.UserId];
            }

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
            userDatabaseCreator = new UserDatabaseTemp();
            userDatabaseCreator.SetupTable();
            userData = userDatabaseCreator.ReturnUserTable();

            // Act
            var userToken3 = new Token(_userId3);

            /*
             * This if statement represents a query call to find the
             * specified user, and retrieve claims
             */
            if (userData.ContainsKey(userToken3.UserId))
            {
                userToken3.Claims = userData[userToken3.UserId];
            }
            actual = ClaimsAuthorization.VerifyClaims(userToken3, _requireAdminRights);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        
        #endregion
    }
}