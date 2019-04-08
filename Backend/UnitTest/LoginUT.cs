using ManagerLayer.LoginManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class LoginUT
    {
        [TestMethod]
        public void Login_Pass()
        {
            //Arrange
            LoginManager lm = new LoginManager();
            SSOUserRequest request = new SSOUserRequest();

            request.email = "julianpoyo+22@gmail.com";
            request.signature = "4T5Csu2U9OozqN66Us+pEc5ODcBwPs1ldaq2fmBqtfo=";
            request.ssoUserId = "0743cd2c-fec3-4b79-a5b6-a6c52a752c71";
            request.timestamp = "1552766624957";

            var expected = "-1";
            //Act
            var actual = lm.Login(request);
            //Assert

            Assert.AreNotEqual(expected, actual);
        }
        [TestMethod]
        public void Login_Fail()
        {
            //Arrange
            LoginManager lm = new LoginManager();
            SSOUserRequest request = new SSOUserRequest();

            request.email = "julianpoyo+22@gmail.com";
            request.signature = "4T5Csu2U9OozqN66Us+pEc5ODcBwPs1ldaq2fmBqtfoa";
            request.ssoUserId = "0743cd2c-fec3-4b79-a5b6-a6c52a752c71";
            request.timestamp = "1552766624957";

            var expected = "-1";
            //Act
            var actual = lm.Login(request);
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }

}
