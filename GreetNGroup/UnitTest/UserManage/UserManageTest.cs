using System;
using GreetNGroup.Account;
using GreetNGroup.SiteUser;
using GreetNGroup.UserManage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.UserManage
{
    [TestClass]
    public class UserManageTest
    {
        UserAccount[] UserDB = new UserAccount[5];
        [TestMethod]
        public void CreateAccount_EqualComparison_Pass()
        {
            //Arange
            UserAccount actual = UserManager.addAccount("dylan","lakewood","CA","USA","12/26/1996");
            UserAccount expected = new UserAccount("dylan", "", "", "", "lakewood", "CA", "USA", "12/26/1996", "", "", "");

            //Act
   

            //Assert
            Assert.AreEqual(actual, expected);

        }

        [TestMethod]
        public void CreateMultiAccount_Pass()
        {
            UserAccount Dylan = UserManager.addAccount("dylan", "lakewood", "CA", "USA", "12/26/1996");

            UserDB[1] = Dylan;
            

            //Assert.AreEqual(Dylan, Dylan2);

        }


    }
}
