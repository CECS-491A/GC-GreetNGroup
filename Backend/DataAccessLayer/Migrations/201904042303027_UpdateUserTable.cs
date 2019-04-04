namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Action",
                c => new
                    {
                        ActionId = c.Int(nullable: false),
                        ActionName = c.String(),
                    })
                .PrimaryKey(t => t.ActionId);
            
            CreateTable(
                "dbo.Attendee",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        CheckedIn = c.Boolean(nullable: false),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => new { t.EventId, t.UserId })
                .ForeignKey("dbo.Event", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EventName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        UserName = c.String(nullable: false),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        DoB = c.DateTime(nullable: false),
                        IsActivated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Blocklist",
                c => new
                    {
                        BlockId = c.Int(nullable: false),
                        UserId1 = c.Int(nullable: false),
                        UserId2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlockId)
                .ForeignKey("dbo.User", t => t.UserId1, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId2, cascadeDelete: true)
                .Index(t => t.UserId1)
                .Index(t => t.UserId2);
            
            CreateTable(
                "dbo.Claim",
                c => new
                    {
                        ClaimId = c.Int(nullable: false),
                        ClaimName = c.String(),
                    })
                .PrimaryKey(t => t.ClaimId);
            
            CreateTable(
                "dbo.EventRole",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        RoleName = c.String(nullable: false, maxLength: 128),
                        MaxRoleCount = c.Int(nullable: false),
                        RequiredRole = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventId, t.RoleName })
                .ForeignKey("dbo.Event", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.EventTag",
                c => new
                    {
                        EventId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventId, t.TagId })
                .ForeignKey("dbo.Event", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TagId = c.Int(nullable: false),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.Friend",
                c => new
                    {
                        FriendId = c.Int(nullable: false),
                        UserId1 = c.Int(nullable: false),
                        UserId2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FriendId)
                .ForeignKey("dbo.User", t => t.UserId1, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId2, cascadeDelete: true)
                .Index(t => t.UserId1)
                .Index(t => t.UserId2);
            
            CreateTable(
                "dbo.UserAction",
                c => new
                    {
                        ActionTime = c.DateTime(nullable: false),
                        SessionId = c.Int(nullable: false),
                        ActionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ActionTime, t.SessionId, t.ActionId })
                .ForeignKey("dbo.Action", t => t.ActionId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ActionId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        UId = c.Int(nullable: false),
                        ClaimId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UId, t.ClaimId })
                .ForeignKey("dbo.Claim", t => t.ClaimId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UId, cascadeDelete: true)
                .Index(t => t.UId)
                .Index(t => t.ClaimId);
            
            CreateTable(
                "dbo.UserRating",
                c => new
                    {
                        RaterId1 = c.Int(nullable: false),
                        RatedId1 = c.Int(nullable: false),
                        Rating = c.Single(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => new { t.RaterId1, t.RatedId1 })
                .ForeignKey("dbo.User", t => t.RatedId1, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.RaterId1, cascadeDelete: true)
                .Index(t => t.RaterId1)
                .Index(t => t.RatedId1);
            
            CreateTable(
                "dbo.UserTag",
                c => new
                    {
                        TagId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TagId, t.UserId })
                .ForeignKey("dbo.Tag", t => t.TagId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TagId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTag", "UserId", "dbo.User");
            DropForeignKey("dbo.UserTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.UserRating", "RaterId1", "dbo.User");
            DropForeignKey("dbo.UserRating", "RatedId1", "dbo.User");
            DropForeignKey("dbo.UserClaim", "UId", "dbo.User");
            DropForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim");
            DropForeignKey("dbo.UserAction", "UserId", "dbo.User");
            DropForeignKey("dbo.UserAction", "ActionId", "dbo.Action");
            DropForeignKey("dbo.Friend", "UserId2", "dbo.User");
            DropForeignKey("dbo.Friend", "UserId1", "dbo.User");
            DropForeignKey("dbo.EventTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.EventTag", "EventId", "dbo.Event");
            DropForeignKey("dbo.EventRole", "EventId", "dbo.Event");
            DropForeignKey("dbo.Blocklist", "UserId2", "dbo.User");
            DropForeignKey("dbo.Blocklist", "UserId1", "dbo.User");
            DropForeignKey("dbo.Attendee", "UserId", "dbo.User");
            DropForeignKey("dbo.Attendee", "EventId", "dbo.Event");
            DropForeignKey("dbo.Event", "UserId", "dbo.User");
            DropIndex("dbo.UserTag", new[] { "UserId" });
            DropIndex("dbo.UserTag", new[] { "TagId" });
            DropIndex("dbo.UserRating", new[] { "RatedId1" });
            DropIndex("dbo.UserRating", new[] { "RaterId1" });
            DropIndex("dbo.UserClaim", new[] { "ClaimId" });
            DropIndex("dbo.UserClaim", new[] { "UId" });
            DropIndex("dbo.UserAction", new[] { "UserId" });
            DropIndex("dbo.UserAction", new[] { "ActionId" });
            DropIndex("dbo.Friend", new[] { "UserId2" });
            DropIndex("dbo.Friend", new[] { "UserId1" });
            DropIndex("dbo.EventTag", new[] { "TagId" });
            DropIndex("dbo.EventTag", new[] { "EventId" });
            DropIndex("dbo.EventRole", new[] { "EventId" });
            DropIndex("dbo.Blocklist", new[] { "UserId2" });
            DropIndex("dbo.Blocklist", new[] { "UserId1" });
            DropIndex("dbo.Event", new[] { "UserId" });
            DropIndex("dbo.Attendee", new[] { "UserId" });
            DropIndex("dbo.Attendee", new[] { "EventId" });
            DropTable("dbo.UserTag");
            DropTable("dbo.UserRating");
            DropTable("dbo.UserClaim");
            DropTable("dbo.UserAction");
            DropTable("dbo.Friend");
            DropTable("dbo.Tag");
            DropTable("dbo.EventTag");
            DropTable("dbo.EventRole");
            DropTable("dbo.Claim");
            DropTable("dbo.Blocklist");
            DropTable("dbo.User");
            DropTable("dbo.Event");
            DropTable("dbo.Attendee");
            DropTable("dbo.Action");
        }
    }
}
