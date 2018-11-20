using System;
using GreetNGroup.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UserTest
    {
        User Dylan = new User("dylanchhin123@gmail.com", "123", "Lakewood", "CA", "USA", "12/25/1996",
                                "What is your favorite book?", "Cat in the Hat");
        [TestMethod]
        public void GetUserName_CorrectName()
        {
            // Arrange
            string expected = "dylanchhin123@gmail.com";
            string actual;

            // Act
            actual = Dylan.Username;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetPassword_CorrectPassWord()
        {
            // Arrange
            string expected = "123";
            string actual;

            // Act
            actual = Dylan.Password;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetCity_CorrectCity()
        {
            // Arrange
            string expected = "Lakewood";
            string actual;

            // Act
            actual = Dylan.Cityloc;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetState_CorrectState()
        {
            // Arrange
            string expected = "CA";
            string actual;

            // Act
            actual = Dylan.Stateloc;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetCountry_CorrectCountry()
        {
            // Arrange
            string expected = "USA";
            string actual;

            // Act
            actual = Dylan.Countryloc;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetDOB_CorrectDOB()
        {
            // Arrange
            string expected = "12/25/1996";
            string actual;

            // Act
            actual = Dylan.DOB;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetSecurityQuestion_CorrectSecurityQuestion()
        {
            // Arrange
            string expected = "What is your favorite book?";
            string actual;

            // Act
            actual = Dylan.SecurityQ;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetSecurityAnswer_CorrectSecurityAnswer()
        {
            // Arrange
            string expected = "Cat in the Hat";
            string actual;

            // Act
            actual = Dylan.SecurityA;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void SetUserName_ValidEntry_True()
        {
            // Arrange
            Boolean expected = true;
            Boolean actual = false;

            // Act
            try
            {
                Dylan.Username = "Bob";
                if(Dylan.Username == "Bob")
                {
                    actual = true;
                }
                else
                {
                    actual = false;
                }
            }
            catch(Exception ex)
            {

            }

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void SetUserName_InvalidEntryInteger_False()
        {
            // Arrange
            Boolean expected = false;
            Boolean actual = false;

            // Act
            try
            {
                //Dylan.Username = 1;
                if (Dylan.Username == "Bob")
                {
                    actual = true;
                }
                else
                {
                    actual = false;
                }
            }
            catch (Exception)
            {
                actual = false;
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
