using System.Data.Entity;
using Gucci.DataAccessLayer.Tables;

namespace Gucci.DataAccessLayer.Context
{
    public class GreetNGroupContext : DbContext
    {
        /*
         * The following creates Db tables in the GreetNGroupContext
         */
        
        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EventTag> EventTags { get; set; }
        public DbSet<Attendance> Attendees { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<ActionsTable> Actions { get; set; }
        public DbSet<UserAction> UserAction { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<GreetNGroupContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}