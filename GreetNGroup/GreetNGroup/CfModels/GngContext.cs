using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GreetNGroup.CfModels
{
    public class GngContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
    }
}