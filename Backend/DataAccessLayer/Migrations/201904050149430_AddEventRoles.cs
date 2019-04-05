namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventRoles : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventRole", "EventId", "dbo.Event");
            DropIndex("dbo.EventRole", new[] { "EventId" });
            DropTable("dbo.EventRole");
        }
    }
}
