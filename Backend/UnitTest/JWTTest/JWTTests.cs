using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gucci.ServiceLayer.Services;
using Gucci.DataAccessLayer.Tables;
using Gucci.DataAccessLayer.Context;
using System.Collections.Generic;
using System;
using System.Linq;
using Gucci.ServiceLayer.Services;

namespace UnitTest.JWTTest
{
    [TestClass]
    public class JWTTests
    {
        JWTService jwtService = new JWTService();
        UserService userService = new UserService();
        UserClaimsService userClaimsService = new UserClaimsService();
        GreetNGroupContext ctx = new GreetNGroupContext();

        #region Passing Tests

        [TestMethod]
        public void AssignJwt_Pass()
        {
            //Assign
            var testUId = 99;
            var newTestUser = "test@gmail.com";
            
            //Act
            var jwtString = jwtService.CreateToken(newTestUser, testUId);
            Console.WriteLine(jwtString);
            //Assert
            Assert.IsNotNull(jwtString);
        }

        [TestMethod]
        public void JwtIsTampered_Pass()
        {
            var tamperedJwtString = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9.2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPG";
            var expected = true;

            var actual = jwtService.IsJWTSignatureTampered(tamperedJwtString);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void JwtIsUntampered_Pass()
        {
            var jwtString = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9.2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPGIRs8ZLjWB0";
            var expected = false;

            var actual = jwtService.IsJWTSignatureTampered(jwtString);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RetrieveUIdFromJwt_Pass()
        {
            var jwtString = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9.2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPGIRs8ZLjWB0";
            var expected = 99;

            var actual = jwtService.GetUserIDFromToken(jwtString);
            Console.WriteLine(actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RetrieveUsernameFromJwt_Pass()
        {
            var jwtString = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9.2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPGIRs8ZLjWB0";
            var expected = "test@gmail.com";

            var actual = jwtService.GetUsernameFromToken(jwtString);
            Console.WriteLine(actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RetrieveUsersClaims_Pass()
        {
            //Assign
            var newTestUser = "test@gmail.com";
            var expectedList = new List<Claim>();
            expectedList.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(1)));
            expectedList.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(2)));
            expectedList.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(5)));
            var expected = true;

            //Act
            var actualList = jwtService.RetrieveClaims(newTestUser);
            var comparison = expectedList.Except(actualList);
            var actual = comparison.Any();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfUserHasTheseClaims_Pass()
        {
            //Assign
            var jwtString = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9.2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPGIRs8ZLjWB0";
            var claimsToCheck = new List<string>();
            claimsToCheck.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(1)).ClaimName);
            claimsToCheck.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(2)).ClaimName);
            var expected = true;

            //Act
            var actual = jwtService.CheckUserClaims(jwtString, claimsToCheck);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Failing Tests
        [TestMethod]
        public void RetrieveUsersClaims_Fail()
        {
            //Assign
            var newTestUser = "test@gmail.com";
            var expectedList = new List<Claim>();
            expectedList.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(4)));
            expectedList.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(3)));
            var expected = false;

            //Act
            var actualList = jwtService.RetrieveClaims(newTestUser);
            var comparison = expectedList.Except(actualList);
            var actual = comparison.Any();

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void RetrieveUsernameFromJwt_Fail()
        {
            var jwtString = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9.2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPGIRs8ZLjWB0";
            var expected = "bobross@gmail.com";

            var actual = jwtService.GetUsernameFromToken(jwtString);
            Console.WriteLine(actual);

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void RetrieveUIdFromJwt_Fail()
        {
            var jwtString = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9.2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPGIRs8ZLjWB0";
            var expected = 4;

            var actual = jwtService.GetUserIDFromToken(jwtString);
            Console.WriteLine(actual);

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void JwtIsTampered_Fail()
        {
            var tamperedJwtString = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9.2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPG";
            var expected = false;

            var actual = jwtService.IsJWTSignatureTampered(tamperedJwtString);

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void JwtIsUntampered_Fail()
        {
            var jwtString = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9.2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPGIRs8ZLjWB0";
            var expected = true;

            var actual = jwtService.IsJWTSignatureTampered(jwtString);

            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfUserHasTheseClaims_Fail()
        {
            //Assign
            var jwtString = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJDYW5WaWV3RXZlbnRzIjoiOTkiLCJDYW5DcmVhdGVFdmVudHMiOiI5OSIsIk92ZXIxOCI6Ijk5IiwiZXhwIjoxNTU1NjcyMDEyLCJpc3MiOiJncmVldG5ncm91cC5jb20iLCJhdWQiOiJ0ZXN0QGdtYWlsLmNvbSJ9.2qSi4OwEFrbTD9GG3hx6fqZFuYVIjUzPGIRs8ZLjWB0";
            var claimsToCheck = new List<string>();
            claimsToCheck.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(4)).ClaimName);
            claimsToCheck.Add(ctx.Claims.FirstOrDefault(c => c.ClaimId.Equals(3)).ClaimName);
            var expected = true;

            //Act
            var actual = jwtService.CheckUserClaims(jwtString, claimsToCheck);

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        #endregion
    }
}
