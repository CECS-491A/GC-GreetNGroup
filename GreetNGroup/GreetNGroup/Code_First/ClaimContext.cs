using System.Data.Entity;

namespace GreetNGroup.Code_First
{
    public class ClaimContext : DbContext
    {
        public ClaimContext()
        {
            
        }

        public DbSet<ClaimPool> Claims { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
    }
}