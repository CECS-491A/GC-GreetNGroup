using System;
using GreetNGroup.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UserTest
    {
        User Dylan = new User("dylanchhin123@gmail.com", "123", "Dylan", "Chin", "Lakewood", "CA", "USA", "12/25/1996",
                                "What is your favorite book?", "Cat in the Hat", "1a2s3d4f");

        //Tests that are expected to pass

        [TestMethod]
        public void GetUserName_Pass()
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
        public void GetPassword_Pass()
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
        public void GetFirstName_Pass()
        {
            // Arrange
            string expected = "Dylan";
            string actual;

            // Act
            actual = Dylan.firstname;

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetLastName_Pass()
        {
            // Arrange
            string expected = "Chin";
            string actual;

            // Act
            actual = Dylan.lastname;

            // Assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void GetCity_Pass()
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
        public void GetState_Pass()
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
        public void GetCountry_Pass()
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
        public void GetDOB_Pass()
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
        public void GetSecurityQuestion_Pass()
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
        public void GetSecurityAnswer_Pass()
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
        public void SetUserName_Pass()
        {
            // Arrange
            Boolean expected = true;
            Boolean actual = false;

            // Act
            try
            {
                Dylan.Username = "Bob";
                if (Dylan.Username == "Bob")
                {
                    actual = true;
                }
                else
                {
                    actual = false;
                }
            }
            catch (Exception ex)
            {

            }

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetID_Pass()
        {
            // Arrange
            string expected = "1a2s3d4f";
            string actual;

            // Act
            actual = Dylan.userID;

            // Assert
            Assert.AreEqual(expected, actual);
        }




        //Tests that are expected to fail

        [TestMethod]
        public void GetUserName_Fail()
        {
            // Arrange
            string expected = "winnmoo@gmail.com";
            string actual;

            // Act
            actual = Dylan.Username;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void GetFirstName_Fail()
        {
            // Arrange
            string expected = "Winn";
            string actual;

            // Act
            actual = Dylan.firstname;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void GetLastName_Fail()
        {
            // Arrange
            string expected = "Moo";
            string actual;

            // Act
            actual = Dylan.firstname;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        public void GetCity_Fail()
        {
            // Arrange
            string expected = "Westminster";
            string actual;

            // Act
            actual = Dylan.Cityloc;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }
        [TestMethod]
        public void GetState_Fail()
        {
            // Arrange
            string expected = "AZ";
            string actual;

            // Act
            actual = Dylan.Stateloc;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }
        [TestMethod]
        public void GetCountry_Fail()
        {
            // Arrange
            string expected = "MY";
            string actual;

            // Act
            actual = Dylan.Countryloc;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }
        [TestMethod]
        public void GetDOB_Fail()
        {
            // Arrange
            string expected = "12/22/1996";
            string actual;

            // Act
            actual = Dylan.DOB;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }
        [TestMethod]
        public void GetSecurityQuestion_Fail()
        {
            // Arrange
            string expected = "Who is your favorite teacher?";
            string actual;

            // Act
            actual = Dylan.SecurityQ;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }
        [TestMethod]
        public void GetSecurityAnswer_Fail()
        {
            // Arrange
            string expected = "AyyLmao";
            string actual;

            // Act
            actual = Dylan.SecurityA;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }
        [TestMethod]
        public void SetUserName_Fail()
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
        [TestMethod]
        public void GetID_Fail()
        {
            // Arrange
            string expected = "1b2n3m5z";
            string actual;

            // Act
            actual = Dylan.userID;

            // Assert
            Assert.AreNotEqual(expected, actual);
        }
    }
}
