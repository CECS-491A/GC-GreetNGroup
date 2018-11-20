using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UserTest
    {
        User Dylan = new User("dylanchhin123@gmail.com", "123", "Lakewood", "CA", "USA", "12/25/1996",
                                "What is your favorite book?", "Cat in the Hat");
        [TestMethod]
        public void GetUserName_Pass()
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
        public void GetUserName_Fail()
        {
            // Arrange
            string expected = "bob@gmail.com";
            string actual;

            // Act
            actual = Dylan.GetUserName();

            // Assert
            Assert.AreNotEqual(expected, actual);
        }
    }
}
