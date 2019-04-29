namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Events : DbMigration
    {
        public override void Up()
        {
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRating", "RaterId1", "dbo.User");
            DropForeignKey("dbo.UserRating", "RatedId1", "dbo.User");
            DropForeignKey("dbo.UserClaim", "UId", "dbo.User");
            DropForeignKey("dbo.UserClaim", "ClaimId", "dbo.Claim");
            DropForeignKey("dbo.UserAction", "UserId", "dbo.User");
            DropForeignKey("dbo.UserAction", "ActionId", "dbo.Action");
            DropForeignKey("dbo.EventTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.EventTag", "EventId", "dbo.Event");
            DropForeignKey("dbo.Attendee", "UserId", "dbo.User");
            DropForeignKey("dbo.Attendee", "EventId", "dbo.Event");
            DropForeignKey("dbo.Event", "UserId", "dbo.User");
            DropIndex("dbo.UserRating", new[] { "RatedId1" });
            DropIndex("dbo.UserRating", new[] { "RaterId1" });
            DropIndex("dbo.UserClaim", new[] { "ClaimId" });
            DropIndex("dbo.UserClaim", new[] { "UId" });
            DropIndex("dbo.UserAction", new[] { "UserId" });
            DropIndex("dbo.UserAction", new[] { "ActionId" });
            DropIndex("dbo.EventTag", new[] { "TagId" });
            DropIndex("dbo.EventTag", new[] { "EventId" });
            DropIndex("dbo.Event", new[] { "UserId" });
            DropIndex("dbo.Attendee", new[] { "UserId" });
            DropIndex("dbo.Attendee", new[] { "EventId" });
            DropTable("dbo.UserRating");
            DropTable("dbo.UserClaim");
            DropTable("dbo.UserAction");
            DropTable("dbo.Tag");
            DropTable("dbo.EventTag");
            DropTable("dbo.Claim");
            DropTable("dbo.User");
            DropTable("dbo.Event");
            DropTable("dbo.Attendee");
            DropTable("dbo.Action");
        }
    }
}
