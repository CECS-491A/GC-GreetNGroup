namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttendees : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: false)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendee", "UserId", "dbo.User");
            DropForeignKey("dbo.Attendee", "EventId", "dbo.Event");
            DropIndex("dbo.Attendee", new[] { "UserId" });
            DropIndex("dbo.Attendee", new[] { "EventId" });
            DropTable("dbo.Attendee");
        }
    }
}
