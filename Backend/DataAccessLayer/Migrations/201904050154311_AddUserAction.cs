namespace Gucci.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAction : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAction", "UserId", "dbo.User");
            DropForeignKey("dbo.UserAction", "ActionId", "dbo.Action");
            DropIndex("dbo.UserAction", new[] { "UserId" });
            DropIndex("dbo.UserAction", new[] { "ActionId" });
            DropTable("dbo.UserAction");
        }
    }
}
