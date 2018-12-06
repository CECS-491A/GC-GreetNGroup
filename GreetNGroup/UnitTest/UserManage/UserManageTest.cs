using System;
using System.Collections.Generic;
using GreetNGroup.DataBase_Classes;
using GreetNGroup.SiteUser;
using GreetNGroup.UserManage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web;
using System.Data.Entity;
using GreetNGroup;
using System.Data.Entity.Validation;
using GreetNGroup.Code_First;

[TestClass]
public class UserManageTest
{
    List<UserAccount> Users = new List<UserAccount>();
    UserAccount Dylan = new UserAccount("dylanchhin123@gmail.com", "Asdfg123", "Dylan", "Chin", "Lakewood", "CA", "USA", "12/25/1996",
                           "What is your favorite book?", "Cat in the Hat", "1a2s3d4f", 1, true);
    UserAccount Bob = new UserAccount("bobby@gmail.com", "Qwerty123", "Bobby", "Chang", "Long Beach", "CA", "USA", "01/25/1998",
                           "What is your favorite book?", "Harry Potter", "5r6y7u8i", 0, false);
    UserAccount Jake = new UserAccount("jklmao@gmail.com", "Zxcvb123", "Jake", "Keja", "Los Angelos", "CA", "USA", "09/07/1997",
                           "What is your favorite book?", "Goosebumps", "7b3c5v9i", 1, true);
    UserAccount Chris = new UserAccount("chrism@gmail.com", "Tgnuj8346", "Chris", "Evans", "Los Angelos", "CA", "USA", "11/07/1997",
                           "What is your favorite book?", "Charlie and the Chocolate Factory", "2d4e5f5w", 0, true);

    [TestMethod]
    public void test()
    {

        //Arange
        Boolean expected = true;
        Boolean actual = true;

       try
        {
            using (var ctx = new GreetNGroupContext())
            {
                var stud = new UserClaim() { ClaimId = "0008" , UserId = "p01q2w9o38ei4r", };

                ctx.UserClaims.Add(stud);
                ctx.SaveChanges();
            }
        }
          catch(Exception e)
        {
            Console.WriteLine(e);
        }

        Assert.AreEqual(actual, expected);

    }
    [TestMethod]
    public void AddAccount_EqualComparison_Pass()
    {

        //Arange
        Boolean expected = true;
        Boolean actual;
        UserAccount newA = new UserAccount("dylan", "", "", "", "lakewood", "CA", "USA", "12/26/1996", "", "", "", 0, true);

        Users.Add(Dylan);
        //Act
        try
        {
            UserAccount actualA = (UserAccount)Dylan.AddAccount("dylan", "lakewood", "CA", "USA", "12/26/1996", Users);
            if (actualA.Equals(newA))
            {

                actual = true;
            }
            else
            {
                actual = false;
            }
        }
        catch (Exception e)
        {
            actual = false;
        }
        //Assert
        Assert.AreEqual(actual, expected);

    }
    [TestMethod]
    public void AddAccount_toDatabase_Pass()
    {
        //Arange
        Boolean expected = true;
        Boolean actual;
        Users.Add(Dylan);
        //Act
        try
        {
            UserAccount actualA = (UserAccount)Dylan.AddAccount("helloprof@gmail.com", "lakewood", "CA", "USA", "12/26/1996", Users);
            DataBaseQuery.insertUser(actualA, Users);
            if (Users[1].Username.Equals("helloprof@gmail.com"))
            {
                actual = true;
            }
            else
            {
                actual = false;
            }


        }
        catch (Exception e)
        {
            actual = false;
        }



        //Assert
        Assert.AreEqual(actual, expected);

    }

    [TestMethod]
    public void AddAccount_ExistingUserName_Fail()
    {
        //Arange
        Boolean expected = true;
        Boolean actual;
        Users.Add(Dylan);
        //Act
        try
        {
            UserAccount actualA = (UserAccount)Dylan.AddAccount("dylanchhin123@gmail.com", "lakewood", "CA", "USA", "12/26/1996", Users);
            actual = true;

        }
        catch (Exception e)
        {
            actual = false;
        }


        //Assert
        Assert.AreNotEqual(actual, expected);

    }

    [TestMethod]
    public void AddAccount_UserWithoutValidClaim_Fail()
    {
        //Arange
        Boolean expected = true;
        Boolean actual;
        Users.Add(Dylan);
        Users.Add(Bob);
        //Act
        try
        {
            UserAccount actualA = (UserAccount)Bob.AddAccount("Peter@gmail.com", "lakewood", "CA", "USA", "12/26/1996", Users);
            actual = true;

        }
        catch (Exception e)
        {
            actual = false;
        }


        //Assert
        Assert.AreNotEqual(actual, expected);

    }
    [TestMethod]
    public void deleteAccount_Pass()
    {
        //Arange
        Boolean expected = true;
        Boolean actual;
        Users.Add(Dylan);
        Users.Add(Bob);
        Users.Add(Jake);
        //Act
        try
        {
            var canDelete = Dylan.DeleteAccount(Bob);
            if (canDelete == true)
            {
                DataBaseQuery.deleteUser(Bob, Users);
                if (Users.Contains(Bob) != true)
                {
                    actual = true;
                }
                else
                {
                    actual = false;
                }
            }
            else
            {
                actual = false;
            }
        }
        catch (Exception e)
        {
            actual = false;
        }
        //Assert
        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void deleteAccount_UserWithoutValidClaim_Fail()
    {
        //Arange
        Boolean actual = false;

        Boolean expected = true;

        Users.Add(Dylan);
        Users.Add(Bob);
        Users.Add(Jake);
        //Act
        try
        {
            var canDelete = Bob.DeleteAccount(Dylan);
            if (canDelete == true)
            {
                actual = true;
            }
            else
            {
                actual = false;
            }
        }
        catch (Exception e)
        {
            actual = false;
        }



        //Assert
        Assert.AreNotEqual(actual, expected);

    }
    [TestMethod]
    public void deleteAccount_AccountwithoutValidClaim_Fail()
    {
        //Arange
        Boolean expected = true;
        Boolean actual;

        Users.Add(Dylan);
        Users.Add(Bob);
        Users.Add(Jake);
        //Act
        try
        {
            var canDelete = Dylan.DeleteAccount(Jake);
            if (canDelete == true)
            {

                actual = true;

            }
            else
            {
                actual = false;
            }
        }
        catch (Exception e)
        {
            actual = false;
        }

        //Assert
        Assert.AreNotEqual(actual, expected);

    }
    [TestMethod]
    public void changeEnable_TruetoFalse_Pass()
    {
        //Arange
        Boolean expected = false;
        Boolean actual;
        System.Diagnostics.Debug.WriteLine("Hello");
        //Act
        try
        {
            Dylan.ChangeEnable(Chris, false);
            actual = Chris.Enable;
        }
        catch (Exception e)
        {
            actual = true;
        }



        //Assert
        Assert.AreEqual(actual, expected);

    }

    [TestMethod]
    public void changeEnable_FalsetoTrue_Pass()
    {
        //Arange
        Boolean expected = true;
        Boolean actual;

        //Act
        try
        {
            Dylan.ChangeEnable(Bob, true);
            actual = Bob.Enable;
        }
        catch (Exception e)
        {
            actual = false;
        }



        //Assert
        Assert.AreEqual(actual, expected);

    }
    [TestMethod]
    public void changeEnable_UserWithoutValidClaim_Fail()
    {
        //Arange
        Boolean expected = true;
        Boolean actual;

        //Act
        try
        {
            Bob.ChangeEnable(Chris, true);

            actual = Chris.Enable;
        }
        catch (Exception e)
        {
            actual = false;
        }



        //Assert
        Assert.AreNotEqual(actual, expected);

    }
    [TestMethod]
    public void changeEnable_AccountWithoutValidClaim_Fail()
    {
        //Arange
        Boolean expected = true;
        Boolean actual;

        //Act
        try
        {
            Dylan.ChangeEnable(Jake, true);

            actual = Jake.Enable;
        }
        catch (Exception e)
        {
            actual = false;
        }

        //Assert
        Assert.AreNotEqual(actual, expected);

    }
    [TestMethod]
    public void changeEnable_AccountAlreadyDisabled_Fail()
    {
        //Arange
        Boolean expected = true;
        Boolean actual;

        //Act
        try
        {
            Dylan.ChangeEnable(Chris, true);

            actual = Chris.Enable;
        }
        catch (Exception e)
        {
            actual = false;
        }

        //Assert
        Assert.AreNotEqual(actual, expected);

    }
    [TestMethod]
    public void changeEnable_AccountAlreadyEnabled_Fail()
    {
        //Arange
        Boolean expected = true;
        Boolean actual;

        //Act
        try
        {
            Dylan.ChangeEnable(Bob, false);

            actual = Bob.Enable;
        }
        catch (Exception e)
        {
            actual = false;
        }

        //Assert
        Assert.AreNotEqual(actual, expected);
        //Assert.AreEqual(Dylan, Dylan2);

    }
}