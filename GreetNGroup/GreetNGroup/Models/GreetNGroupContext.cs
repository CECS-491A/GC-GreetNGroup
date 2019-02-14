namespace GreetNGroup.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GreetNGroupContext : DbContext
    {
        public GreetNGroupContext()
            : base("name=GreetNGroupContext")
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<BlockTable> BlockTables { get; set; }
        public virtual DbSet<ClaimsTable> ClaimsTables { get; set; }
        public virtual DbSet<EventRole> EventRoles { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<FriendsTable> FriendsTables { get; set; }
        public virtual DbSet<TagsTable> TagsTables { get; set; }
        public virtual DbSet<UserTable> UserTables { get; set; }
        public virtual DbSet<UserTag> UserTags { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                .Property(e => e.EventId)
                .IsUnicode(false);

            modelBuilder.Entity<Attendance>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<Attendance>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<BlockTable>()
                .Property(e => e.BlockId)
                .IsUnicode(false);

            modelBuilder.Entity<BlockTable>()
                .Property(e => e.UserId1)
                .IsUnicode(false);

            modelBuilder.Entity<BlockTable>()
                .Property(e => e.UserId2)
                .IsUnicode(false);

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

            modelBuilder.Entity<EventRole>()
                .Property(e => e.EventId)
                .IsUnicode(false);

            modelBuilder.Entity<EventRole>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Event>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<Event>()
                .Property(e => e.EventId)
                .IsUnicode(false);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.Event)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.EventRoles)
                .WithRequired(e => e.Event)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.TagsTables)
                .WithMany(e => e.Events)
                .Map(m => m.ToTable("EventTags").MapLeftKey("EventId").MapRightKey("TagId"));

            modelBuilder.Entity<FriendsTable>()
                .Property(e => e.FriendId)
                .IsUnicode(false);

            modelBuilder.Entity<FriendsTable>()
                .Property(e => e.UserId1)
                .IsUnicode(false);

            modelBuilder.Entity<FriendsTable>()
                .Property(e => e.UserId2)
                .IsUnicode(false);

            modelBuilder.Entity<TagsTable>()
                .Property(e => e.TagId)
                .IsUnicode(false);

            modelBuilder.Entity<TagsTable>()
                .Property(e => e.TagName)
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

            modelBuilder.Entity<UserTable>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.UserTable)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTable>()
                .HasMany(e => e.BlockTables)
                .WithRequired(e => e.UserTable)
                .HasForeignKey(e => e.UserId1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTable>()
                .HasMany(e => e.BlockTables1)
                .WithRequired(e => e.UserTable1)
                .HasForeignKey(e => e.UserId2)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTable>()
                .HasMany(e => e.Events)
                .WithRequired(e => e.UserTable)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTable>()
                .HasMany(e => e.FriendsTables)
                .WithRequired(e => e.UserTable)
                .HasForeignKey(e => e.UserId1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTable>()
                .HasMany(e => e.FriendsTables1)
                .WithRequired(e => e.UserTable1)
                .HasForeignKey(e => e.UserId2)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTable>()
                .HasMany(e => e.UserTags)
                .WithRequired(e => e.UserTable)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTag>()
                .Property(e => e.TagID)
                .IsUnicode(false);

            modelBuilder.Entity<UserTag>()
                .Property(e => e.UserID)
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
