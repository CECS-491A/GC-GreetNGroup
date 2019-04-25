using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gucci.ManagerLayer.LoginManagement;
using Gucci.ServiceLayer.Requests;

namespace UnitTest
{
    [TestClass]
    public class LoginUT
    {
        [TestMethod]
        public void Login_Pass()
        {
            //Arrange
            LoginManager lm = new LoginManager("D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE");
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
            LoginManager lm = new LoginManager("D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE");
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
