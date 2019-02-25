using System.Data.Entity;

namespace GreetNGroup.DataAccess
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
        public DbSet<UserTag> UserTags { get; set; }
        public DbSet<EventRole> EventRoles { get; set; }
        public DbSet<Attendance> Attendees { get; set; }
    }
}