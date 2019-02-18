using System;
using System.Collections.Generic;
using GreetNGroup.SiteUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using GreetNGroup;
using GreetNGroup.Validation;

[TestClass]
public class UserManageTest
{
    UserAccount Dylan = new UserAccount();
    UserAccount Chris = new UserAccount();

    /*
    #region Adding Account
    [TestMethod]
    public void AddAccount_ValidParameters_Pass()
    {
        //Arange
        const bool expected = true;
        bool actual = false;
        //Act
        try
        {
            Dylan.AddAccount("x","HowdyYall@gmail.com", "Houston", "TX", "USA", new DateTime(2007, 5, 28));
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables.Single(s => s.UserName == "HowdyYall@gmail.com");
                if (user.UserName.Equals("HowdyYall@gmail.com") && user.City.Equals("Houston") && user.State.Equals("TX") && user.Country.Equals("USA") && (DateTime.Compare(user.DoB, new DateTime(2007, 5, 28)) == 0)) 
                {
                    actual = true;
                }
                else
                {
                    actual = false;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            actual = false;
        }
        //Assert
        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void AddAccount_UserNameExist_Fail()
    {
        //Arange
        const bool expected = true;
        var actual = true;
        //Act
        try
        {
            Dylan.AddAccount("x","HowdyYall@gmail.com", "Sacramento", "CA", "USA", new DateTime(2008, 5, 28));
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables.Single(s => s.UserName == "HowdyYall@gmail.com");
                if (user.UserName.Equals("HowdyYall@gmail.com") && user.City.Equals("Sacramento") && user.State.Equals("CA") && user.Country.Equals("USA") && (DateTime.Compare(user.DoB, new DateTime(2008, 5, 28)) == 0))
                {
                    Console.WriteLine("Pass");
                }
                else
                {
                    actual = false;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            actual = false;
        }
        //Assert
        Assert.AreNotEqual(actual, expected);
    }

    [TestMethod]
    public void AddAccount_WrongClaims_Fail()
    {
        //Arange
        const bool expected = true;
        var actual = true;
        //Act
        try
        {
            Chris.AddAccount("x","newAccount@gmail.com", "Sacramento", "CA", "USA", new DateTime(2008, 5, 28));
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables.Count(s => s.UserName == "newAccount@gmail.com");
                actual = user > 0;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            actual = false;
        }
        //Assert
        Assert.AreNotEqual(actual, expected);
    }


    [TestMethod]
    public void AddAccount_InvalidAttributesnull_Fail()
    {
        //Arange
        const bool expected = true;
        var actual = true;
        //Act
        try
        {
            ValidationManager.CheckAddAttributes(null, null, null, null, new DateTime(2008, 5, 28));
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables
                              .Where(s => s.UserName == null);
                foreach (var i in user)
                {
                    if (i.UserName.Equals(null) && i.City.Equals(null) && i.State.Equals(null) && i.Country.Equals(null) && (DateTime.Compare(i.DoB, new DateTime(2008, 5, 28)) == 0))
                    {
                        Console.WriteLine("Pass");
                        actual = true;
                    }
                    else
                    {
                        actual = false;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            actual = false;
        }
        //Assert
        Assert.AreNotEqual(actual, expected);
    }

    [TestMethod]
    public void AddAccount_InvalidAttributesEmptyString_Fail()
    {
        //Arange
        const bool expected = true;
        var actual = true;
        //Act
        try
        {
            ValidationManager.CheckAddAttributes("", "", "", "", new DateTime());
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables
                              .Where(s => s.UserName == "");
                foreach (var i in user)
                {
                    if (i.UserName.Equals("") && i.City.Equals("") && i.State.Equals("") && i.Country.Equals("") && (DateTime.Compare(i.DoB, new DateTime(2008, 5, 28)) == 0))
                    {
                        Console.WriteLine("Pass");
                        actual = true;
                    }
                    else
                    {
                        actual = false;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            actual = false;
        }
        //Assert
        Assert.AreNotEqual(actual, expected);
    }
    #endregion

    #region Deleting Account   
    [TestMethod]
    public void deleteAccount_Pass()
    {
        //Arange
        const bool expected = true;
        var actual = false;
        //Act
        try
        {
            Dylan.DeleteAccount("x", "test");
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables.Count(s => s.UserName == "test");
                actual = user > 0;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            actual = false;
        }
        //Assert
        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void deleteAccount_WrongClaims_Fail()
    {
        //Arange
        const bool expected = true;
        var actual = true;
        //Act
        Chris.DeleteAccount("x", "test");
        using (var ctx = new GreetNGroupContext())
        {
            var user = ctx.UserTables.Count(s => s.UserName == "dylanchhinn@gmail.com");

            actual = user > 0;
        }
        //Assert
        Assert.AreNotEqual(actual, expected);
    }

    [TestMethod]
    public void deleteAccount_AccountWrongClaims_Fail()
    {
        //Arange
        const bool expected = true;
        var actual = true;
        //Act
        Dylan.DeleteAccount("x", "p01q2w9o38ei4r");
        using (var ctx = new GreetNGroupContext())
        {
            var user = ctx.UserTables.Count(s => s.UserName == "dylanchhinn@gmail.com");

            actual = user > 0;
        }
        //Assert
        Assert.AreNotEqual(actual, expected);
    }

    [TestMethod]
    public void deleteAccount_InvalidUserIDNull_Fail()
    {
        //Arange
        const bool expected = true;
        bool actual = true;
        //Act
        var check = ValidationManager.CheckDeletedAttributes(null);

        actual = check == true;

        //Assert
        Assert.AreNotEqual(actual, expected);
    }

    [TestMethod]
    public void deleteAccount_InvalidUserIDEmptyString_Fail()
    {
        //Arange
        const bool expected = true;
        bool actual = true;
        //Act
        var check = ValidationManager.CheckDeletedAttributes("");

        actual = check == true;

        //Assert
        Assert.AreNotEqual(actual, expected);
    }
    #endregion

    #region Editing Account
    [TestMethod]
    public void editUser_ValidString_Pass()
    {
        //Arrange
        const bool expected = true;
        bool actual = true;
        var attributesToEdit = new List<string> { "Bob", "Dylan", "bobdylan@gmail.com", "Fountain Valley", "California"
            , "United States", ".", ".", ".", "." };
        //Act
        if (ValidationManager.CheckEditAttributes(attributesToEdit))
        {
            actual = true;
        }
        //Assert
        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void editUser_ValidString_Fail()
    {
        //Arrange
        const bool expected = false;
        bool actual = true;
        var attributesToEdit = new List<string> { "", "Dylan", "bobdylan@gmail.com", "Fountain Valley", "California"
            , "United States", ".", ".", ".", "." };
        //Act
        if (!ValidationManager.CheckEditAttributes(attributesToEdit))
        {
            actual = false;
        }
        //Assert
        Assert.AreEqual(actual, expected);
    }

    [TestMethod]
    public void changeEnable_TruetoFalse_Pass()
    {
        //Arange
        const bool expected = false;
        bool actual;
        var attributesToEdit = new List<string> { ".", ".", ".", ".", "."
            , ".", ".", ".", ".", "false" };
        //Act
        try
        {
            Dylan.UpdateAccount("x", "p03d928ej2838fo", attributesToEdit);
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables.Single(s => s.UserId == "p03d928ej2838fo");

                actual = user.isActivated == false;
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
    public void changeEnable_FalsetoTrue_Pass()
    {
        //Arange
        const bool expected = true;
        bool actual;
        //Act
        try
        {
            var attributesToEdit = new List<string> { ".", ".", ".", ".", "."
            , ".", ".", ".", ".", "true" };
            Dylan.UpdateAccount("x", "p0499dj238e92j2", attributesToEdit);
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables.Single(s => s.UserId == "p0499dj238e92j2");

                actual = user.isActivated == true;
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
    public void changeEnable_WrongClaim_Pass()
    {
        //Arange
        const bool expected = false;
        bool actual;
        //Act
        try
        {
            var attributesToEdit = new List<string> { ".",".",".",".",".",".",".",".",".", "false" };
            Chris.UpdateAccount("x", "test", attributesToEdit);
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables.Single(s => s.UserId == "test");
                if (user.isActivated == false)
                {
                    actual = false;
                }
                else
                {
                    actual = true;
                }
            }
        }
        catch (Exception e)
        {
            actual = true;
        }
        //Assert
        Assert.AreNotEqual(actual, expected);
    }

    [TestMethod]
    public void changeEnable_AccountWrongClaim_Fail()
    {
        //Arange
        Boolean expected = false;
        Boolean actual;
        //Act
        try
        {
            var attributesToEdit = new List<string> { ".", ".", ".", ".", ".", ".", ".", ".", ".", "false" };
            Chris.UpdateAccount("x", "p01dj9wjd99u3u", attributesToEdit);
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables.Single(s => s.UserId == "p01dj9wjd99u3u");

                actual = user.isActivated == false;
            }
        }
        catch (Exception e)
        {
            actual = true;
        }
        //Assert
        Assert.AreNotEqual(actual, expected);
    }
    //TODO: Add unit tests for editing other attributes

    [TestMethod]
    public void editUserAccount_Pass()
    {
        //Arrange
        Boolean expected = true;
        Boolean actual = true;
        var attributesToEdit = new List<string> { "ey", "Moo", "bob@gmail.com", "Fountain Valley", "California"
            , "United States", new DateTime(1996, 1, 1).ToString(), "What's your favorite food", "Chicken", "true" };
        //Act
        try
        {
            Chris.UpdateAccount("x", "test", attributesToEdit);
            using (var ctx = new GreetNGroupContext())
            {
                var user = ctx.UserTables.Single(s => s.UserId == "test");

                var afterUpdatedAttributes = new List<string>();
                afterUpdatedAttributes.Add(user.FirstName);
                afterUpdatedAttributes.Add(user.LastName);
                afterUpdatedAttributes.Add(user.UserName);
                afterUpdatedAttributes.Add(user.City);
                afterUpdatedAttributes.Add(user.State);
                afterUpdatedAttributes.Add(user.Country);
                afterUpdatedAttributes.Add(user.DoB.ToString());
                afterUpdatedAttributes.Add(user.SecurityQuestion);
                afterUpdatedAttributes.Add(user.SecurityAnswer);
                afterUpdatedAttributes.Add(user.isActivated.ToString());

                actual = attributesToEdit.SequenceEqual(afterUpdatedAttributes);
            }
        }
        catch (Exception e)
        {
            actual = false;
        }
        //Assert
        Assert.AreNotEqual(actual, expected);
    }
    #endregion
    */
}