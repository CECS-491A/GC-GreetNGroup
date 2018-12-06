using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace GreetNGroup.DataBase_Classes
{
    public class UserContext : DbContext
    {
        public UserContext() : base("Model1")
        {

        }
        public DbSet<UserTable> Users { get; set; }
    }
        
}