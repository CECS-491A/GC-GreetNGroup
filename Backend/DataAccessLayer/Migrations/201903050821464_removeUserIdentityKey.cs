namespace GreetNGroup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUserIdentityKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Event", "UserId", "dbo.User");
            DropForeignKey("dbo.Attendee", "UserId", "dbo.User");
            DropForeignKey("dbo.Blocklist", "UserId1", "dbo.User");
            DropForeignKey("dbo.Blocklist", "UserId2", "dbo.User");
            DropForeignKey("dbo.Friend", "UserId1", "dbo.User");
            DropForeignKey("dbo.Friend", "UserId2", "dbo.User");
            DropForeignKey("dbo.UserAction", "UserId", "dbo.User");
            DropForeignKey("dbo.UserClaim", "UId", "dbo.User");
            DropForeignKey("dbo.UserRating", "RatedId1", "dbo.User");
            DropForeignKey("dbo.UserRating", "RaterId1", "dbo.User");
            DropForeignKey("dbo.UserTag", "UserId", "dbo.User");
            DropPrimaryKey("dbo.User");
            AlterColumn("dbo.User", "UserId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.User", "UserId");
            AddForeignKey("dbo.Event", "UserId", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Attendee", "UserId", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Blocklist", "UserId1", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Blocklist", "UserId2", "dbo.User", "UserId", cascadeDelete: false);
            AddForeignKey("dbo.Friend", "UserId1", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Friend", "UserId2", "dbo.User", "UserId", cascadeDelete: false);
            AddForeignKey("dbo.UserAction", "UserId", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.UserClaim", "UId", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.UserRating", "RatedId1", "dbo.User", "UserId", cascadeDelete: false);
            AddForeignKey("dbo.UserRating", "RaterId1", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.UserTag", "UserId", "dbo.User", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTag", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRating", "RaterId1", "dbo.User");
            DropForeignKey("dbo.UserRating", "RatedId1", "dbo.User");
            DropForeignKey("dbo.UserClaim", "UId", "dbo.User");
            DropForeignKey("dbo.UserAction", "UserId", "dbo.User");
            DropForeignKey("dbo.Friend", "UserId2", "dbo.User");
            DropForeignKey("dbo.Friend", "UserId1", "dbo.User");
            DropForeignKey("dbo.Blocklist", "UserId2", "dbo.User");
            DropForeignKey("dbo.Blocklist", "UserId1", "dbo.User");
            DropForeignKey("dbo.Attendee", "UserId", "dbo.User");
            DropForeignKey("dbo.Event", "UserId", "dbo.User");
            DropPrimaryKey("dbo.User");
            AlterColumn("dbo.User", "UserId", c => c.Int(nullable: false, identity: false));
            AddPrimaryKey("dbo.User", "UserId");
            AddForeignKey("dbo.UserTag", "UserId", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.UserRating", "RaterId1", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.UserRating", "RatedId1", "dbo.User", "UserId", cascadeDelete: false);
            AddForeignKey("dbo.UserClaim", "UId", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.UserAction", "UserId", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Friend", "UserId2", "dbo.User", "UserId", cascadeDelete: false);
            AddForeignKey("dbo.Friend", "UserId1", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Blocklist", "UserId2", "dbo.User", "UserId", cascadeDelete: false);
            AddForeignKey("dbo.Blocklist", "UserId1", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Attendee", "UserId", "dbo.User", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Event", "UserId", "dbo.User", "UserId", cascadeDelete: true);
        }
    }
}
