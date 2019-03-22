namespace GreetNGroup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttendeesAndRoles : DbMigration
    {
        public override void Up()
        {
            // Here cascade delete has been disabled -- in this case of many to many relation
            CreateTable(
                "dbo.Attendee",
                c => new
                    {
                        EventId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        CheckedIn = c.Boolean(nullable: false),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => new { t.EventId, t.UserId })
                .ForeignKey("dbo.Event", t => t.EventId, cascadeDelete: false)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: false)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EventRole",
                c => new
                    {
                        EventId = c.String(nullable: false, maxLength: 128),
                        RoleName = c.String(nullable: false, maxLength: 128),
                        MaxRoleCount = c.Int(nullable: false),
                        RequiredRole = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventId, t.RoleName })
                .ForeignKey("dbo.Event", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventRole", "EventId", "dbo.Event");
            DropForeignKey("dbo.Attendee", "UserId", "dbo.User");
            DropForeignKey("dbo.Attendee", "EventId", "dbo.Event");
            DropIndex("dbo.EventRole", new[] { "EventId" });
            DropIndex("dbo.Attendee", new[] { "UserId" });
            DropIndex("dbo.Attendee", new[] { "EventId" });
            DropTable("dbo.EventRole");
            DropTable("dbo.Attendee");
        }
    }
}
