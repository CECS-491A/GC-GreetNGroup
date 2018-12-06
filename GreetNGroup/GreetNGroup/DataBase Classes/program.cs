using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreetNGroup.DataBase_Classes
{
    class program
    {
        static void Main(string[] args)
        {

            using (var ctx = new UserContext())
            {
                var stud = new User() { Firstname = "Bill" };

                ctx.Users.Add(stud);
                ctx.SaveChanges();
            }
        }
    }
}