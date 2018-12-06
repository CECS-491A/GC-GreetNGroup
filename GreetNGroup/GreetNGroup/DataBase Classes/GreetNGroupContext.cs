namespace GreetNGroup
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    public partial class GreetNGroupContext : DbContext
    {
        public GreetNGroupContext()
            : base("name=GreetNGroup")
        {
        }

        public virtual DbSet<ClaimsTable> ClaimsTables { get; set; }
        public virtual DbSet<UserTable> UserTables { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClaimsTable>()
                .Property(e => e.ClaimId)
                .IsUnicode(false);

            modelBuilder.Entity<ClaimsTable>()
                .Property(e => e.Claim)
                .IsUnicode(false);

            modelBuilder.Entity<UserTable>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<UserTable>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<UserTable>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<UserTable>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<UserTable>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<UserTable>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<UserTable>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<UserTable>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<UserTable>()
                .Property(e => e.SecurityQuestion)
                .IsUnicode(false);

            modelBuilder.Entity<UserTable>()
                .Property(e => e.SecurityAnswer)
                .IsUnicode(false);

            modelBuilder.Entity<UserClaim>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<UserClaim>()
                .Property(e => e.ClaimId)
                .IsUnicode(false);
        }
    }
}
