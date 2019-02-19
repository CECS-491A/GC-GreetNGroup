using System.Data.Entity;

namespace GreetNGroup.DataAccess
{
    public class GreetNGroupContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}