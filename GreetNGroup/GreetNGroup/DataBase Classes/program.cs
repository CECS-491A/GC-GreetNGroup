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

            try
            {
                using (var ctx = new Model2())
                {
                    var stud = new UserTable() { UserId = "1q3e4r5t" };

                    ctx.UserTables.Add(stud);
                    ctx.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }
    }
}