using Gucci.DataAccessLayer.Tables;
using Gucci.ServiceLayer.Requests;
using Gucci.ServiceLayer.Services;
using ManagerLayer.UserManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace UnitTest
{
    [TestClass]
    public class UserManagerUT
    {
        
        [TestMethod]
        public void DeleteUser_Pass()
        {
            //Arrange
            UserManager userMan = new UserManager("D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE");
            UserService _userService = new UserService();
            SSOUserRequest request = new SSOUserRequest();

            var user = new User(9999, null, null, "julianpoyo+22@gmail.com", null, null, null, DateTime.Now, false);
            
            request.email = "julianpoyo+22@gmail.com";
            request.signature = "4T5Csu2U9OozqN66Us+pEc5ODcBwPs1ldaq2fmBqtfo=";
            request.ssoUserId = "0743cd2c-fec3-4b79-a5b6-a6c52a752c71";
            request.timestamp = "1552766624957";

            var expected = true;

            //Act
            var actual = userMan.DeleteUserSSO(request);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeleteUser_Fail_InvalidRequest()
        {
            //Arrange
            UserManager userMan = new UserManager("D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE");
            UserService _userService = new UserService();
            SSOUserRequest request = new SSOUserRequest();

            var user = new User(9999, null, null, "julianpoyo+22@gmail.com", null, null, null, DateTime.Now, false);
            request.email = "julianpoyo+22@gmail.com";
            request.signature = "4T5Csu2U9OozqN66Us+pEc5ODcBwPs1ldaq2fmBqtfo=";
            request.ssoUserId = "0743cd2c-fec3-4b79-a5b6-a6c52a752c71";
            request.timestamp = "1552766624957";

            var expected = true;

            //Act
            var actual = userMan.DeleteUserSSO(request);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeleteUser_Fail_UserNotInDB()
        {
            //Arrange
            UserManager userMan = new UserManager("D078F2AFC7E59885F3B6D5196CE9DB716ED459467182A19E04B6261BBC8E36EE");
            UserService _userService = new UserService();
            SSOUserRequest request = new SSOUserRequest();

            var user = new User(9999, null, null, "julianpoyo+22@gmail.com", null, null, null, DateTime.Now, false);
            request.email = "julianpoyo+22@gmail.com";
            request.signature = "4T5Csu2U9OozqN66Us+pEc5ODcBwPs1ldaq2fmBqtfo=";
            request.ssoUserId = "0743cd2c-fec3-4b79-a5b6-a6c52a752c71";
            request.timestamp = "1552766624957";

            var expected = true;

            //Act
            var actual = userMan.DeleteUserSSO(request);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        
    }
}
