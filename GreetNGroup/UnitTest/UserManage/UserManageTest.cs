using System;
using GreetNGroup.User;
using GreetNGroup.UserManage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.UserManage
{
    [TestClass]
    public class UserManageTest
    {
        [TestMethod]
        public void CreateAccount_Pass()
        {
            UserAccount Dylan = UserManager.addAccount("dylan","lakewood","CA","USA","12/26/1996");

            UserAccount Dylan2 = new UserAccount("dylan", "", "", "", "lakewood", "CA", "USA", "12/26/1996", "", "", "");

            Assert.AreEqual(Dylan, Dylan2);

        }
    }
}
