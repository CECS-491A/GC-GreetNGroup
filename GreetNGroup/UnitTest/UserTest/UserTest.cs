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
            actual = Dylan.GetUserName();

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
            actual = Dylan.GetPassword();

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
            actual = Dylan.GetCity();

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
            actual = Dylan.GetState();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
