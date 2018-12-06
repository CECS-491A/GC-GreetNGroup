using System;
using GreetNGroup.SiteUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UserTest
    {
        #region Required Fields
        UserAccount Dylan = new UserAccount("dylanchhin123@gmail.com", "123", "Dylan", "Chin", "Lakewood", "CA", "USA", new DateTime(1996, 12, 25),
                                "What is your favorite book?", "Cat in the Hat", "1a2s3d4f", 1, true);
        #endregion

        #region Pass Tests
        [TestMethod]
        public void UserAccountGetMethods_Pass()
        {
            //Arrange
            //Expected Data
            string expectedUserName = "dylanchhin123@gmail.com";
            string expectedPassword = "123";
            string expectedFirstName = "Dylan";
            string expectedLastName = "Chin";
            string expectedCity = "Lakewood";
            string expectedState = "CA";
            string expectedCountry = "USA";
            DateTime expectedDOB = new DateTime(1996, 12, 25);
            string expectedSecurityQ = "What is your favorite book?";
            string expectedSecurityA = "Cat in the Hat";
            string expectedUserID = "1a2s3d4f";
            Boolean expected = true;
            //Actual Data
            string actualUserName;
            string actualPassword;
            string actualFirstName;
            string actualLastName;
            string actualCity;
            string actualState;
            string actualCountry;
            DateTime actualDOB;
            string actualSecurityQ;
            string actualSecurityA;
            string actualUserID;
            Boolean actual = false;

            //Act
            actualUserName = Dylan.Username;
            actualPassword = Dylan.Password;
            actualFirstName = Dylan.Firstname;
            actualLastName = Dylan.Lastname;
            actualCity = Dylan.Cityloc;
            actualState = Dylan.Stateloc;
            actualCountry = Dylan.Countryloc;
            actualDOB = Dylan.DOB;
            actualSecurityQ = Dylan.SecurityQ;
            actualSecurityA = Dylan.SecurityA;
            actualUserID = Dylan.UserID;

            //Act
            if (actualUserName.Equals(expectedUserName) && actualPassword.Equals(expectedPassword) && actualFirstName.Equals(expectedFirstName) &&
               actualLastName.Equals(expectedLastName) && actualCity.Equals(expectedCity) && actualState.Equals(expectedState) && actualCountry.Equals(expectedCountry) &&
               actualDOB.Equals(expectedDOB) && actualSecurityQ.Equals(expectedSecurityQ) && actualSecurityA.Equals(expectedSecurityA) && actualUserID.Equals(expectedUserID))
            {
                actual = true;
            }

            //Assert

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UserAccountSetMethods_Pass()
        {
            //Arrange
            //Expected Data
            string expectedUserName = "BobJoe@yahoo.com"; ;
            string expectedPassword = "qwerty";
            string expectedFirstName = "Bob";
            string expectedLastName = "Joe";
            string expectedCity = "Las Vegas";
            string expectedState = "Nevada";
            string expectedCountry = "Canada";
            DateTime expectedDOB = new DateTime(1996, 12, 25);
            string expectedSecurityQ = "What state were you born in?";
            string expectedSecurityA = "NY";
            string expectedUserID = "1v3c5n6g";
            Boolean expected = true;
            //Actual Data
            string actualUserName;
            string actualPassword;
            string actualFirstName;
            string actualLastName;
            string actualCity;
            string actualState;
            string actualCountry;
            DateTime actualDOB;
            string actualSecurityQ;
            string actualSecurityA;
            string actualUserID;
            Boolean actual = false;

            //Act
            Dylan.Username = "BobJoe@yahoo.com";
            actualUserName = Dylan.Username;

            Dylan.Password = "qwerty";
            actualPassword = Dylan.Password;

            Dylan.Firstname = "Bob";
            actualFirstName = Dylan.Firstname;

            Dylan.Lastname = "Joe";
            actualLastName = Dylan.Lastname;

            Dylan.Cityloc = "Las Vegas";
            actualCity = Dylan.Cityloc;

            Dylan.Stateloc = "Nevada";
            actualState = Dylan.Stateloc;

            Dylan.Countryloc = "Canada";
            actualCountry = Dylan.Countryloc;

            Dylan.DOB = new DateTime(1996, 12, 25);
            actualDOB = Dylan.DOB;

            Dylan.SecurityQ = "What state were you born in?";
            actualSecurityQ = Dylan.SecurityQ;

            Dylan.SecurityA = "NY";
            actualSecurityA = Dylan.SecurityA;

            Dylan.UserID = "1v3c5n6g";
            actualUserID = Dylan.UserID;

            //Act
            if (actualUserName.Equals(expectedUserName) && actualPassword.Equals(expectedPassword) && actualFirstName.Equals(expectedFirstName) &&
               actualLastName.Equals(expectedLastName) && actualCity.Equals(expectedCity) && actualState.Equals(expectedState) && actualCountry.Equals(expectedCountry) &&
               actualDOB.Equals(expectedDOB) && actualSecurityQ.Equals(expectedSecurityQ) && actualSecurityA.Equals(expectedSecurityA) && actualUserID.Equals(expectedUserID))
            {
                actual = true;
            }

            //Assert

            Assert.AreEqual(expected, actual);


        }
        #endregion

        #region Fail Tests
        [TestMethod]
        public void UserAccountGetMethods_Fail()
        {
            //Arrange
            //Expected Data
            string expectedUserName = "winnmoo@gmail.com";
            string expectedPassword = "567";
            string expectedFirstName = "Winn";
            string expectedLastName = "moo";
            string expectedCity = "Lakewood";
            string expectedState = "CA";
            string expectedCountry = "USA";
            DateTime expectedDOB = new DateTime(1995, 12, 25);
            string expectedSecurityQ = "What is your favorite book?";
            string expectedSecurityA = "Where the wild things are";
            string expectedUserID = "12e223e";
            Boolean expected = true;
            //Actual Data
            string actualUserName;
            string actualPassword;
            string actualFirstName;
            string actualLastName;
            string actualCity;
            string actualState;
            string actualCountry;
            DateTime actualDOB;
            string actualSecurityQ;
            string actualSecurityA;
            string actualUserID;
            Boolean actual;

            //Act
            actualUserName = Dylan.Username;
            actualPassword = Dylan.Password;
            actualFirstName = Dylan.Firstname;
            actualLastName = Dylan.Lastname;
            actualCity = Dylan.Cityloc;
            actualState = Dylan.Stateloc;
            actualCountry = Dylan.Countryloc;
            actualDOB = Dylan.DOB;
            actualSecurityQ = Dylan.SecurityQ;
            actualSecurityA = Dylan.SecurityA;
            actualUserID = Dylan.UserID;

            //Act
            if (actualUserName.Equals(expectedUserName) && actualPassword.Equals(expectedPassword) && actualFirstName.Equals(expectedFirstName) &&
               actualLastName.Equals(expectedLastName) && actualCity.Equals(expectedCity) && actualState.Equals(expectedState) && actualCountry.Equals(expectedCountry) &&
               actualDOB.Equals(expectedDOB) && actualSecurityQ.Equals(expectedSecurityQ) && actualSecurityA.Equals(expectedSecurityA) && actualUserID.Equals(expectedUserID))
            {
                actual = true;
            }
            else
            {
                actual = false;
            }

            //Assert

            Assert.AreNotEqual(expected, actual);
        }
        [TestMethod]
        public void UserAccountSetMethods_Fail()
        {
            //Arrange
            //Expected Data
            string expectedUserName = "dylanchhin123@gmail.com";
            string expectedPassword = "123";
            string expectedFirstName = "Dylan";
            string expectedLastName = "Chin";
            string expectedCity = "Lakewood";
            string expectedState = "CA";
            string expectedCountry = "USA";
            DateTime expectedDOB = new DateTime(1996, 12, 25);
            string expectedSecurityQ = "What is your favorite book?";
            string expectedSecurityA = "Cat in the Hat";
            string expectedUserID = "1a2s3d4f";
            Boolean expected = true;
            //Actual Data
            string actualUserName;
            string actualPassword;
            string actualFirstName;
            string actualLastName;
            string actualCity;
            string actualState;
            string actualCountry;
            DateTime actualDOB;
            string actualSecurityQ;
            string actualSecurityA;
            string actualUserID;
            Boolean actual = false;

            //Act
            Dylan.Username = "BobJoe@yahoo.com";
            actualUserName = Dylan.Username;

            Dylan.Password = "qwerty";
            actualPassword = Dylan.Password;

            Dylan.Firstname = "Bob";
            actualFirstName = Dylan.Firstname;

            Dylan.Lastname = "Joe";
            actualLastName = Dylan.Lastname;

            Dylan.Cityloc = "Las Vegas";
            actualCity = Dylan.Cityloc;

            Dylan.Stateloc = "Nevada";
            actualState = Dylan.Stateloc;

            Dylan.Countryloc = "Canada";
            actualCountry = Dylan.Countryloc;

            Dylan.DOB = new DateTime(1996, 12, 25);
            actualDOB = Dylan.DOB;

            Dylan.SecurityQ = "What state were you born in?";
            actualSecurityQ = Dylan.SecurityQ;

            Dylan.SecurityA = "NY";
            actualSecurityA = Dylan.SecurityA;

            Dylan.UserID = "1v3c5n6g";
            actualUserID = Dylan.UserID;

            //Act
            if (actualUserName.Equals(expectedUserName) && actualPassword.Equals(expectedPassword) && actualFirstName.Equals(expectedFirstName) &&
               actualLastName.Equals(expectedLastName) && actualCity.Equals(expectedCity) && actualState.Equals(expectedState) && actualCountry.Equals(expectedCountry) &&
               actualDOB.Equals(expectedDOB) && actualSecurityQ.Equals(expectedSecurityQ) && actualSecurityA.Equals(expectedSecurityA) && actualUserID.Equals(expectedUserID))
            {
                actual = true;
            }

            //Assert

            Assert.AreNotEqual(expected, actual);


        }
        #endregion


        #region Variable Input Tests
        [TestMethod]
        public void SetMethods_InputEmptyString_Pass()
        {
            //Arrange
            //Expected Data
            string expectedUserName = "";
            string expectedPassword = "";
            string expectedFirstName = "";
            string expectedLastName = "";
            string expectedCity = "";
            string expectedState = "";
            string expectedCountry = "";
            DateTime expectedDOB = new DateTime();
            string expectedSecurityQ = "";
            string expectedSecurityA = "";
            string expectedUserID = "";
            Boolean expected = true;
            //Actual Data
            string actualUserName;
            string actualPassword;
            string actualFirstName;
            string actualLastName;
            string actualCity;
            string actualState;
            string actualCountry;
            DateTime actualDOB;
            string actualSecurityQ;
            string actualSecurityA;
            string actualUserID;
            Boolean actual = false;

            //Act
            Dylan.Username = "";
            actualUserName = Dylan.Username;

            Dylan.Password = "";
            actualPassword = Dylan.Password;

            Dylan.Firstname = "";
            actualFirstName = Dylan.Firstname;

            Dylan.Lastname = "";
            actualLastName = Dylan.Lastname;

            Dylan.Cityloc = "";
            actualCity = Dylan.Cityloc;

            Dylan.Stateloc = "";
            actualState = Dylan.Stateloc;

            Dylan.Countryloc = "";
            actualCountry = Dylan.Countryloc;

            Dylan.DOB = new DateTime();
            actualDOB = Dylan.DOB;

            Dylan.SecurityQ = "";
            actualSecurityQ = Dylan.SecurityQ;

            Dylan.SecurityA = "";
            actualSecurityA = Dylan.SecurityA;

            Dylan.UserID = "";
            actualUserID = Dylan.UserID;

            //Act
            if (actualUserName.Equals(expectedUserName) && actualPassword.Equals(expectedPassword) && actualFirstName.Equals(expectedFirstName) &&
               actualLastName.Equals(expectedLastName) && actualCity.Equals(expectedCity) && actualState.Equals(expectedState) && actualCountry.Equals(expectedCountry) &&
               actualDOB.Equals(expectedDOB) && actualSecurityQ.Equals(expectedSecurityQ) && actualSecurityA.Equals(expectedSecurityA) && actualUserID.Equals(expectedUserID))
            {
                actual = true;
            }

            //Assert

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetMethods_InputNull_Pass()
        {
            //Arrange
            //Expected Data
            string expectedUserName = null;
            string expectedPassword = null;
            string expectedFirstName = null;
            string expectedLastName = null;
            string expectedCity = null;
            string expectedState = null;
            string expectedCountry = null;
            DateTime expectedDOB = new DateTime();
            string expectedSecurityQ = null;
            string expectedSecurityA = null;
            string expectedUserID = null;
            Boolean expected = true;
            //Actual Data
            string actualUserName;
            string actualPassword;
            string actualFirstName;
            string actualLastName;
            string actualCity;
            string actualState;
            string actualCountry;
            DateTime actualDOB;
            string actualSecurityQ;
            string actualSecurityA;
            string actualUserID;
            Boolean actual = false;

            //Act
            Dylan.Username = null;
            actualUserName = Dylan.Username;

            Dylan.Password = null;
            actualPassword = Dylan.Password;

            Dylan.Firstname = null;
            actualFirstName = Dylan.Firstname;

            Dylan.Lastname = null;
            actualLastName = Dylan.Lastname;

            Dylan.Cityloc = null;
            actualCity = Dylan.Cityloc;

            Dylan.Stateloc = null;
            actualState = Dylan.Stateloc;

            Dylan.Countryloc = null;
            actualCountry = Dylan.Countryloc;

            Dylan.DOB = new DateTime();
            actualDOB = Dylan.DOB;

            Dylan.SecurityQ = null;
            actualSecurityQ = Dylan.SecurityQ;

            Dylan.SecurityA = null;
            actualSecurityA = Dylan.SecurityA;

            Dylan.UserID = null;
            actualUserID = Dylan.UserID;

            //Act
            if (actualUserName == expectedUserName && actualPassword == expectedPassword && actualFirstName == expectedFirstName &&
               actualLastName == expectedLastName && actualCity == expectedCity && actualState == expectedState && actualCountry == expectedCountry &&
               actualDOB.Equals(expectedDOB) && actualSecurityQ == expectedSecurityQ && actualSecurityA == expectedSecurityA && actualUserID == expectedUserID)
            {
                actual = true;
            }

            //Assert

            Assert.AreEqual(expected, actual);
        }

        
        #endregion
    }
}
