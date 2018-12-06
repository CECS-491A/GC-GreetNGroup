namespace GreetNGroup
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model2 : DbContext
    {
        public Model2()
            : base("name=Model2")
        {
        }

        public virtual DbSet<ClaimsTable> ClaimsTables { get; set; }
        public virtual DbSet<UserTable> UserTables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClaimsTable>()
                .Property(e => e.ClaimId)
                .IsUnicode(false);

            modelBuilder.Entity<ClaimsTable>()
                .Property(e => e.Claim)
                .IsUnicode(false);

            modelBuilder.Entity<ClaimsTable>()
                .HasMany(e => e.UserTables)
                .WithMany(e => e.ClaimsTables)
                .Map(m => m.ToTable("UserClaims").MapLeftKey("ClaimId").MapRightKey("UserId"));

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
        }
    }
}
